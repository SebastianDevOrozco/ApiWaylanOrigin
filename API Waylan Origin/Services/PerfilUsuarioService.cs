using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Interfaces.Usuario;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Waylan_Origin.Services
{
    public class PerfilUsuarioService : IPerfilUsuarioService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public PerfilUsuarioService(IMapper mapper, AppDbContext appDbContext)
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
    }
}
