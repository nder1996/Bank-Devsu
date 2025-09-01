using System.ComponentModel.DataAnnotations;

namespace BancoAPI.Domain.Entities
{
    public class Persona
    {
        [Key]
        public virtual long Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres")]
        public virtual string Nombre { get; set; }

        [Required(ErrorMessage = "El género es requerido")]
        [StringLength(20, ErrorMessage = "El género no debe exceder los 20 caracteres")]
        public virtual string Genero { get; set; }

        [Required(ErrorMessage = "La edad es requerida")]
        [Range(18, 120, ErrorMessage = "La edad debe estar entre 18 y 120 años")]
        public virtual int Edad { get; set; }

        [Required(ErrorMessage = "La identificación es requerida")]
        [StringLength(20, ErrorMessage = "La identificación no debe exceder los 20 caracteres")]
        public virtual string Identificacion { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(255, ErrorMessage = "La dirección no debe exceder los 255 caracteres")]
        public virtual string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(20, ErrorMessage = "El teléfono no debe exceder los 20 caracteres")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El teléfono debe contener solo números")]
        public virtual string Telefono { get; set; }
    }
}
