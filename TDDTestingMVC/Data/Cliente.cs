using System.ComponentModel.DataAnnotations;

namespace TDDTestingMVC.Data
{

    public class Cliente
    {
        public int? Codigo { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe tener 10 dígitos.")]
        public string? Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no debe exceder los 50 caracteres.")]
        public string? Nombres { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no debe exceder los 50 caracteres.")]
        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "Debe ingresar una fecha de nacimiento.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        [StringLength(50, ErrorMessage = "El correo no debe exceder los 50 caracteres.")]
        public string? Mail { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El numero de telefono debe tener 10 dígitos.")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(50, ErrorMessage = "La direccion no debe exceder los 50 caracteres.")]
        public string? Direccion { get; set; }

        public Boolean Estado { get; set; }
    }
}
