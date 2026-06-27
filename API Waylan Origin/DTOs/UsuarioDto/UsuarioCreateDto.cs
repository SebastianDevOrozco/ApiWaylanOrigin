using System.ComponentModel.DataAnnotations;

namespace API_Waylan_Origin.DTOs.UsuarioDto
{
    public class UsuarioCreateDto
    {

        [Required(ErrorMessage = "El Nombre es Obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Correo es Obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "La Contraseña es Obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string Password { get; set; }= string.Empty;
   
    }
}
