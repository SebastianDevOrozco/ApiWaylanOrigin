using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Interfaces.Usuario;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Waylan_Origin.Services
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
            var usuario = await _appDbContext.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                return null;

            return _mapper.Map<UsuarioReadDto>(usuario);
        }

        public async Task<IEnumerable<UsuarioReadDto>> ListaUsuarios()
        {
            var usuarios = await _appDbContext.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();

            if (usuarios == null)
                return null;

            return _mapper.Map<IEnumerable<UsuarioReadDto>>(usuarios);
        }

        public async Task<bool> DeleteUsuario(int usuarioId)
        {
            var usuario = await _appDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
                return false;

            //cambio de estado activo a = false. borrado logico
            usuario.Activo = false;

            await _appDbContext.SaveChangesAsync();

            return true;

        }
    }
}
