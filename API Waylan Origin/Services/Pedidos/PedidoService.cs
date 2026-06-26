using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.PedidosDto;
using API_Waylan_Origin.Interfaces.Pedidos;
using API_Waylan_Origin.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
            var nuevoPedido = new Pedido
            {
                EstadoPedido = "Pendiente",
                IdUsuario = usuarioId,
            };

            decimal TotalCompra = 0;

            foreach(var item in pedidoDto.Detalles)
            {
                var producto = await ValidacionProducto(item.IdProducto, item.Cantidad);

                //Restamos la cantidad solicitada al Stock del producto
                producto.Stock -= item.Cantidad;

                //Calculamos el precio de esta linea
                TotalCompra += (producto.Precio * item.Cantidad);

                //Mapear detalle de pedido
                var nuevoDetalle = _mapper.Map<DetallePedido>(item);
                nuevoDetalle.PrecioUnitario = producto.Precio;

                //añadimos a la lista del pedido
                nuevoPedido.DetallesPedido.Add(nuevoDetalle);
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
}}
