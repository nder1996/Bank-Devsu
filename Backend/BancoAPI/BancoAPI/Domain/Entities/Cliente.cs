using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoAPI.Domain.Entities
{
    public class Cliente
    {

        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, ErrorMessage = "La contraseña no debe exceder los 100 caracteres")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "La persona es requerida")]
        public long PersonaId { get; set; }

        [ForeignKey("PersonaId")]
        public virtual Persona Persona { get; set; }

        public virtual ICollection<Cuenta> Cuentas { get; set; }
    }
}
