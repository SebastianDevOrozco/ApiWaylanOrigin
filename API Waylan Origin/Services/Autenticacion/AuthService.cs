using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.UsuarioDto;
using API_Waylan_Origin.Interfaces.Usuarios;
using API_Waylan_Origin.Models;
using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
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

            // Asignar el rol y el estado 
            usuarioNuevo.IdRol = 2;
            usuarioNuevo.Activo = false;


            string tokenSecreto = RandomNumberGenerator.GetHexString(6).ToUpper();
            usuarioNuevo.TokenVerificacion = tokenSecreto;
            usuarioNuevo.TokenExpiracion = DateTime.UtcNow.AddMinutes(30); // Expira en 30 minutos

            // Encriptar la contraseña usando BCrypt
            usuarioNuevo.Password = BCrypt.Net.BCrypt.HashPassword(usuarioCreateDto.Password);

            //guardar en la base de datos 
            _appDbContext.Usuarios.Add(usuarioNuevo);
            await _appDbContext.SaveChangesAsync();

            try
            {
                string asunto = "Activa tu cuenta Waylan Origin";
                string cuerpoHtml = $@"
            <div style='text-align: center; font-family: Arial;'>
                <h2>¡Bienvenido, {usuarioNuevo.Nombre}!</h2>
                <p>Usa este código de 6 dígitos en la aplicación para verificar tu cuenta:</p>
                <h1 style='color: #4A3B32; background: #eee; padding: 10px; display: inline-block;'>{tokenSecreto}</h1>
                <p>Este código expira en 30 minutos.</p>
            </div>";

                
                await EnviarCorreo(usuarioNuevo.Email, asunto, cuerpoHtml);
            }
            catch (Exception ex)
            {
                // Si el correo falla (ej. caída de red), el usuario igual se guarda en BD.
                // Aquí podrías usar un ILogger para registrar el error.
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
            }

            //rebuscar el usuario para devolver con el rol
            var usuarioConRol = await _appDbContext.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == usuarioNuevo.Id);


            //Mapeo de la entidad guardada al DTO de lectura 
            return _mapper.Map<UsuarioReadDto>(usuarioConRol);
        }
        public async Task<bool> VerificarCorreo(string email, string token)
        {
            // 1. Buscar al usuario por su correo
            var usuario = await _appDbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                throw new ArgumentException("El usuario no existe.");
            

            // 2. Si ya está verificado, no hace falta volver a hacerlo
            if (usuario.EmailVerificado)
                throw new InvalidOperationException("Este correo ya ha sido verificado previamente.");


            // 3. Validar si el token coincide
            if (usuario.TokenVerificacion != token.ToUpper())
                throw new ArgumentException("El código de verificación es incorrecto.");


            // 4. Validar si el token ya expiró
            if (usuario.TokenExpiracion < DateTime.UtcNow)
                throw new InvalidOperationException("El código de verificación ha expirado. Por favor, solicita uno nuevo.");


            // 5. Si Todo está correcto Actualizamos el estado del usuario
            usuario.EmailVerificado = true;
            usuario.Activo = true;

            // Limpiamos los campos del token por seguridad (ya no se necesitan)
            usuario.TokenVerificacion = null;
            usuario.TokenExpiracion = null;

            // Guardamos los cambios de la entidad en la base de datos
            await _appDbContext.SaveChangesAsync();

            return true;
        }
        public async Task<string?> Login(string email, string password)
        {
            //busca el usuario por el correo
            var usuario = await _appDbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            // 2. Si no existe, o la contraseña NO coincide (BCrypt.Verify desencripta y compara)
            if (usuario == null ||!BCrypt.Net.BCrypt.Verify(password, usuario.Password))
                throw new UnauthorizedAccessException("Correo o Contraseña incorrecta.");// Credenciales inválidas

            if (!usuario.Activo)
                throw new InvalidOperationException("Tu cuenta aun no ha sido activada.");

            if (!usuario.EmailVerificado)
                throw new InvalidOperationException("Tu correo electronico no es valido");

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

        private async Task EnviarCorreo(string correoDestino, string asunto, string mensajeHtml)
        {
            var email = new MimeMessage();

            // 1. Configurar el Remitente (Tú)
            string senderName = _config["SmtpSettings:SenderName"]!;
            string senderEmail = _config["SmtpSettings:SenderEmail"]!;
            email.From.Add(new MailboxAddress(senderName, senderEmail));

            // 2. Configurar el Destinatario (El usuario que se registra)
            email.To.Add(MailboxAddress.Parse(correoDestino));

            // 3. Configurar el Asunto y el Cuerpo (HTML)
            email.Subject = asunto;
            var bodyBuilder = new BodyBuilder { HtmlBody = mensajeHtml };
            email.Body = bodyBuilder.ToMessageBody();

            // 4. Conectarse al servidor SMTP de Gmail de forma segura y enviar
            using var smtp = new SmtpClient();
            try
            {
                // Nos conectamos usando STARTTLS (Puerto 587)
                await smtp.ConnectAsync(
                    _config["SmtpSettings:Server"],
                    int.Parse(_config["SmtpSettings:Port"]!),
                    SecureSocketOptions.StartTls
                );

                // Iniciamos sesión con tu correo y la clave mágica de 16 letras
                await smtp.AuthenticateAsync(senderEmail, _config["SmtpSettings:Password"]);

                // Disparamos el correo
                await smtp.SendAsync(email);
            }
            finally
            {
                // Nos desconectamos limpiamente para no dejar conexiones abiertas
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
