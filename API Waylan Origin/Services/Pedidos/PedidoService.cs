using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.PedidosDto;
using API_Waylan_Origin.Enums;
using API_Waylan_Origin.Interfaces.Pedidos;
using API_Waylan_Origin.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace API_Waylan_Origin.Services.Pedidos
{
    public class PedidoService : IPedidoService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public PedidoService(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<PedidoReadDto> CrearPedido(int usuarioId, PedidoCreateDto pedidoDto)
        {

            var nuevoPedido = _mapper.Map<Pedido>(pedidoDto);

            nuevoPedido.IdUsuario = usuarioId;

            decimal TotalCompra = 0;


            foreach (var detalle in nuevoPedido.DetallesPedido)
            {
                var producto = await ValidacionProducto(detalle.IdProducto, detalle.Cantidad);

                // Restamos la cantidad solicitada al Stock del producto
                producto.Stock -= detalle.Cantidad;

                // Le asignamos el precio real de la base de datos al detalle que ya existe
                detalle.PrecioUnitario = producto.Precio;

                // Sumamos al total de la compra
                TotalCompra += (producto.Precio * detalle.Cantidad);
            }

            //Despues de hacer el proceso con todos los items de la lista asignamos el total a la variable de la entidad
            nuevoPedido.Total = TotalCompra;

            //guardo en la DB
            _appDbContext.pedidos.Add(nuevoPedido);
            await _appDbContext.SaveChangesAsync();

            //esta linea permite mapear el detalle del pedido con el nombre y la url del producto 
            await _appDbContext.Entry(nuevoPedido)
                .Collection(p=> p.DetallesPedido)
                .Query()
                .Include(d => d.Producto)
                .LoadAsync();

            return _mapper.Map<PedidoReadDto>(nuevoPedido);
        }

        public async Task<IEnumerable<PedidoReadDto>> ListarPedidos(int usuarioId)
        {
            var pedidos = await _appDbContext.pedidos
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d  => d.Producto)
                .Where(p => p.IdUsuario == usuarioId)
                .ToListAsync();

            if(pedidos == null)
                return new List<PedidoReadDto>();

            return _mapper.Map<IEnumerable<PedidoReadDto>>(pedidos);
        }

        public async Task<IEnumerable<PedidoReadAdminDto>> ListarTodosLosPedidos()
        {
            var pedidos = await _appDbContext.pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();

            if (pedidos == null)
                return new List<PedidoReadAdminDto>();

            return _mapper.Map<IEnumerable<PedidoReadAdminDto>>(pedidos);
        }

        public async Task<PedidoReadAdminDto> PedidoCodigo(string codigo)
        {
            var pedido = await _appDbContext.pedidos
               .Include(p => p.Usuario)
               .Include(p => p.DetallesPedido)
                   .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.CodigoSeguimiento == codigo);

            if (pedido == null)
                throw new KeyNotFoundException($"El pedido con el codigo {codigo} no existe");

            return _mapper.Map<PedidoReadAdminDto>(pedido);
        }

        public async Task CambiarEstadoPedido(string codigo, EstadoPedido NuevoEstado)
        {
            var pedido = await _appDbContext.pedidos
                .FirstOrDefaultAsync(p => p.CodigoSeguimiento == codigo);

            if (pedido == null)
                throw new KeyNotFoundException($"El pedido con el codigo {codigo} No existe");

            pedido.Estado = NuevoEstado;

            await _appDbContext.SaveChangesAsync();
        }

        //Metodos secundarios
        private async Task<Producto> ValidacionProducto(int id, int cantidad)
        {
            var producto = await _appDbContext.Productos
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo == true);

            if (producto == null)
                throw new KeyNotFoundException($"El Producto con el ID {id} No Existe");

            if (producto.Stock < cantidad)
                throw new InvalidOperationException($"El Producto {producto.Nombre} se encuentra agotado");

            return producto;

        }

       
    }
}
