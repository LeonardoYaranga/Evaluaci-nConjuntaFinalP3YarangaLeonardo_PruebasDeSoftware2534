namespace TDDTestingMVC.Models
{
    public class Calculadora
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }
        public int Sumar()
        {
            return FirstNumber + SecondNumber;
        }


    }
}
