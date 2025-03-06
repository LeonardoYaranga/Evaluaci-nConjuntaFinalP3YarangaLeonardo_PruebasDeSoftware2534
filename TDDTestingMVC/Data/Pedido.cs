using System.ComponentModel.DataAnnotations;

namespace TDDTestingMVC.Data
{
    public class Pedido
    {
        public int? PedidoID { get; set; } 

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int ClienteID { get; set; } 

        [Required(ErrorMessage = "La fecha del pedido es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaPedido { get; set; } = DateTime.Now; 

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, 99999999.99, ErrorMessage = "El monto debe estar entre 0.01 y 99,999,999.99.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(50, ErrorMessage = "El estado no debe exceder los 50 caracteres.")]
        public string Estado { get; set; } = "Pendiente"; 
    }
}