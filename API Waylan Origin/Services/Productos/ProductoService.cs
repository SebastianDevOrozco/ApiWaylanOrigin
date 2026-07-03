using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.ProductoDto;
using API_Waylan_Origin.Enums;
using API_Waylan_Origin.Interfaces.Producto;
using API_Waylan_Origin.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Waylan_Origin.Services.Productos
{
    public class ProductoService : IProductoService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public ProductoService(IMapper mapper, AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _env = env;
        }

        public async Task<ProductoReadAdminDto> CrearProducto(ProductoCreateDto productoCreate)
        {
            await ValidarCategoria(productoCreate.IdCategoria);
            var notasExistentes = await validacionNotas(productoCreate.IdNotas);
    
            var productoNuevo = _mapper.Map<Producto>(productoCreate);

            productoNuevo.Activo = true;
            productoNuevo.Notas = notasExistentes;

          
            if (productoCreate.Imagen != null && productoCreate.Imagen.Length > 0)
            {
                // A. Creamos un nombre único para el archivo (Ej: 9b1deb4d-3b7d.jpg)
                string nombreUnico = Guid.NewGuid().ToString() + Path.GetExtension(productoCreate.Imagen.FileName);

                // B. Buscamos la ruta física de la carpeta: wwwroot/imagenes/nombreUnico.jpg
                string rutaCarpeta = Path.Combine(_env.WebRootPath, "imagenes");
                string rutaCompletaArchivo = Path.Combine(rutaCarpeta, nombreUnico);

                // C. Guardamos físicamente el archivo en el disco duro de la PC
                using (var stream = new FileStream(rutaCompletaArchivo, FileMode.Create))
                {
                    await productoCreate.Imagen.CopyToAsync(stream);
                }

                // D. Guardamos la URL relativa en la propiedad del Producto
                // Guardar "/imagenes/foto.jpg" es la mejor práctica porque funcionará en local y en la nube.
                productoNuevo.ImagenUrl = $"/imagenes/{nombreUnico}";
            }
            else
            {
                // Si no subió foto, se pone una imagen por defecto o se deja nula
                productoNuevo.ImagenUrl = "/imagenes/default-producto.png";
            }

            _appDbContext.Productos.Add(productoNuevo);
            await _appDbContext.SaveChangesAsync();

            //esta linea permite mapear el producto con el nombre de la categoria
            await _appDbContext.Entry(productoNuevo).Reference(p => p.Categoria).LoadAsync();

            return _mapper.Map<ProductoReadAdminDto>(productoNuevo);
        }

        public async Task<IEnumerable<ProductoReadAdminDto>> ListarProductosAdmin()
        {
            var productos = await _appDbContext.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Notas)
                .ToListAsync();

            if(productos == null)
                return new List<ProductoReadAdminDto>();

            return _mapper.Map<IEnumerable<ProductoReadAdminDto>>(productos);
        }

        public async Task<IEnumerable<ProductoReadDto>> ListarProductos()
        {
            var productos = await _appDbContext.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Notas)
                .Where(p => p.Activo == true)
                .ToListAsync();

            if (productos == null)
                return new List<ProductoReadDto>();

            return _mapper.Map<IEnumerable<ProductoReadDto>>(productos);
        }
        public async Task<IEnumerable<ProductoReadDto>> ListarProductosTueste(Tueste tueste)
        {
            var productos = await _appDbContext.Productos
              .Include(p => p.Categoria)
              .Include(p => p.Notas)
              .Where(p => p.Activo == true && p.tueste == tueste)
              .ToListAsync();

            if (productos == null)
                return new List<ProductoReadDto>();

            return _mapper.Map<IEnumerable<ProductoReadDto>>(productos);
        }
        public async Task<IEnumerable<ProductoReadDto>> ListarProductosProceso(Proceso proceso)
        {
            var productos = await _appDbContext.Productos
             .Include(p => p.Categoria)
             .Include(p => p.Notas)
             .Where(p => p.Activo == true && p.proceso == proceso)
             .ToListAsync();

            if (productos == null)
                return new List<ProductoReadDto>();

            return _mapper.Map<IEnumerable<ProductoReadDto>>(productos);
        }

        public async Task<ProductoReadAdminDto> ActualizarProducto(int id, ProductoUpdateDto productoUpdate)
        {
            //validaciones
            var producto = await ValidarExistenciaProducto(id);
            await ValidarCategoria(productoUpdate.IdCategoria);
            var notasExistentes = await validacionNotas(productoUpdate.IdNotas);


            string? rutaFotoVieja = producto.ImagenUrl;

            //mapeo los datos de texto
            _mapper.Map(productoUpdate, producto);

            producto.Notas = notasExistentes;

            // 3. ¿El administrador subió una NUEVA imagen en los cambios?
            if (productoUpdate.Imagen != null && productoUpdate.Imagen.Length > 0)
            {
                // A. Creamos el nuevo nombre único para la nueva foto
                string nombreUnico = Guid.NewGuid().ToString() + Path.GetExtension(productoUpdate.Imagen.FileName);
                string rutaCarpeta = Path.Combine(_env.WebRootPath, "imagenes");
                string rutaCompletaArchivo = Path.Combine(rutaCarpeta, nombreUnico);

                // B. Guardamos la nueva foto físicamente en el disco
                using (var stream = new FileStream(rutaCompletaArchivo, FileMode.Create))
                {
                    await productoUpdate.Imagen.CopyToAsync(stream);
                }

                // C. Le asignamos la nueva URL al producto
                producto.ImagenUrl = $"/imagenes/{nombreUnico}";

                // 🔥 MOVIMIENTO SENIOR: Borramos la foto anterior del disco duro para no acumular basura
                // (Validamos que no sea la imagen por defecto)
                if (!string.IsNullOrEmpty(rutaFotoVieja) && rutaFotoVieja != "/imagenes/default-producto.png")
                {
                    // Convertimos la URL relativa (/imagenes/foto.jpg) en una ruta física real de Windows/Linux
                    string rutaFisicaFotoVieja = Path.Combine(_env.WebRootPath, rutaFotoVieja.TrimStart('/'));

                    if (File.Exists(rutaFisicaFotoVieja))
                    {
                        File.Delete(rutaFisicaFotoVieja); // 🗑️ Borrado físico del disco
                    }
                }
            }
            else
            {
                // ⚠️ MUY IMPORTANTE: Si el admin no subió ninguna foto en el update, 
                // mantenemos la foto que ya tenía el producto originalmente.
                producto.ImagenUrl = rutaFotoVieja;
            }


            await _appDbContext.SaveChangesAsync();

            //esta linea permite mapear el producto con el nombre de la categoria
            await _appDbContext.Entry(producto).Reference(p => p.Categoria).LoadAsync();

            return _mapper.Map<ProductoReadAdminDto>(producto);
        }

        public async Task EliminarProducto(int id)
        {
            var producto = await ValidarExistenciaProducto(id);

            producto.Activo = false;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task EditarEstadoProducto(int id, bool nuevoEstado)
        {
            var producto = await ValidarExistenciaProducto(id);

            await ValidarCategoria(producto.IdCategoria);

            producto.Activo = nuevoEstado;
            await _appDbContext.SaveChangesAsync();
        }

       


        //METODOS SECUNDARIOS
        private async Task ValidarCategoria(int id)
        {
            var existeCategoria = await _appDbContext.categorias
                .AnyAsync(c => c.Id == id && c.Activo);

            if (!existeCategoria)
                throw new KeyNotFoundException($"La categoria con el ID {id} NO existe");

        }

        private async Task<Producto> ValidarExistenciaProducto(int id)
        {
            var producto = await _appDbContext.Productos
                .Include(p => p.Notas)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                throw new KeyNotFoundException($"El Producto con el ID {id} No Existe");

            return producto;
        }

        private async Task<List<Nota>> validacionNotas(List<int> IdNotas)
        {
            //Valido si la nota tiene elementos 
            if (IdNotas == null || !IdNotas.Any())
            {
                return new List<Nota>();
            }

            //verificamos que los Ids no esten repetidos 
            var idsUnicos = IdNotas.Distinct().ToList();

            // Traemos de la base de datos las entidades 'Nota' cuyos IDs coincidan
            var notasExistentes = await _appDbContext.Notas
                .Where(n => idsUnicos.Contains(n.id))
                .ToListAsync();

            //comparamos la cantidad 
            if (notasExistentes.Count != idsUnicos.Count)
                throw new ArgumentException("Uno o más IDs de notas no existen en la base de datos.");

            return notasExistentes;
        }
    }
}
