using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Domain.Entities
{
    public class Cliente : Persona
    {

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, ErrorMessage = "La contraseña no debe exceder los 100 caracteres")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; } = true;

        public virtual ICollection<Cuenta> Cuentas { get; set; }
    }
}
