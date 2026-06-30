using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Interfaces.Usuarios;
using API_Waylan_Origin.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace API_Waylan_Origin.Services.Autenticacion
{
    public class AuthService : IAuthService
    {

        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config; // esto es necesario para leer la clave jwt

        public AuthService(IMapper mapper, AppDbContext appDbContext, IConfiguration config)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _config = config;
        }

        public async Task<UsuarioReadDto> Registrar(UsuarioCreateDto usuarioCreateDto)
        {
            await ValidarUsuario(usuarioCreateDto);
           
            //Mapeo del UsuarioDto --> Usuario
            var usuarioNuevo = _mapper.Map<Usuario>(usuarioCreateDto);

            // Asignar el rol y el estado activo
            usuarioNuevo.IdRol = 2;
            usuarioNuevo.Activo = true;

            // Encriptar la contraseña usando BCrypt
            usuarioNuevo.Password = BCrypt.Net.BCrypt.HashPassword(usuarioCreateDto.Password);

            //guardar en la base de datos 
            _appDbContext.Usuarios.Add(usuarioNuevo);
            await _appDbContext.SaveChangesAsync();

            //rebuscar el usuario para devolver con el rol
            var usuarioConRol = await _appDbContext.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == usuarioNuevo.Id);


            //Mapeo de la entidad guardada al DTO de lectura para devolverlo (sin password)
            return _mapper.Map<UsuarioReadDto>(usuarioConRol);
        }

        public async Task<string?> Login(string email, string password)
        {
            //busca el usuario por el correo
            var usuario = await _appDbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            // 2. Si no existe, o la contraseña NO coincide (BCrypt.Verify desencripta y compara)
            if (usuario == null ||!BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                throw new UnauthorizedAccessException("Correo o Contraseña incorrecta.");// Credenciales inválidas
            }
            if (!usuario.Activo)
            {
                throw new InvalidOperationException("Tu cuenta ha sido desactivada por un administrador.");
            }

            return GenerarToken(usuario);
        }



        //METODOS SECUNDARIOS

        private async Task ValidarUsuario(UsuarioCreateDto usuario)
        {
            //validar si el correo ya existe
            var existeUsuario = await _appDbContext.Usuarios
                .AnyAsync(u => u.Email.ToLower() == usuario.Email.ToLower());

            if (existeUsuario)
                throw new ArgumentException("El correo electrónico ya está registrado.");
        }

        private string GenerarToken(Usuario usuario)
        {
            // Leemos la configuración del appsettings.json
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Información que irá dentro del token (No pongas contraseñas aquí)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.IdRol.ToString())
            };

            // Armamos el token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // El token expira en 1 día
                signingCredentials: creds
            );

            // Lo devolvemos como texto (string)
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
