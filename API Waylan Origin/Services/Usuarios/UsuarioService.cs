using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Interfaces.Usuarios;
using API_Waylan_Origin.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Waylan_Origin.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public UsuarioService(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<UsuarioReadDto> InfoUsuario(int usuarioId)
        {
            var usuario = await ValidarExistenciaUsuario(usuarioId,incluirRol: true); 

            return _mapper.Map<UsuarioReadDto>(usuario);
        }

        public async Task<IEnumerable<UsuarioReadDto>> ListaUsuarios()
        {
            var usuarios = await _appDbContext.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();

            if (usuarios == null)
                return new List<UsuarioReadDto>();

            return _mapper.Map<IEnumerable<UsuarioReadDto>>(usuarios);
        }

        public async Task<bool> DeleteUsuario(int usuarioId)
        {
            var usuario = await ValidarExistenciaUsuario(usuarioId); //lo dejamos vacio porque esta en false por defecto en el metodo

            //cambio de estado activo a = false. borrado logico
            usuario.Activo = false;

            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task EditarEstadoUsuario(int usuarioId, bool nuevoEstado)
        {
            var usuario = await ValidarExistenciaUsuario(usuarioId); //lo dejamos vacio porque esta en false por defecto en el metodo

            usuario.Activo = nuevoEstado;
            await _appDbContext.SaveChangesAsync();

        }

        //METODOS SECUNDARIOS


        /*este metodo me permite hacer la validacion en tres metodos distintos pero uno de ellos debe incluir el rol
         * por eso implemente una consulta paso a paso para incluir parametros a la consulta si  es necesario*/
        private async Task<Usuario> ValidarExistenciaUsuario(int id, bool incluirRol = false)
        {
            //preparo la consulta 
            IQueryable<Usuario> query = _appDbContext.Usuarios;

            //en caso de que incluir rol sea true se agrega el rol
            if (incluirRol)
                query = query.Include(u => u.Rol);

            var usuario = await query.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new KeyNotFoundException($"El usuario con el ID {id} NO existe");

            return usuario;
        }
    }
}
