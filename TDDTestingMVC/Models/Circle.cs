namespace TDDTestingMVC.Models
{
    public class Circle
    {
       public float radius { get; set; }
       public float area { get; set; }
        public float CalculateArea()
        {
            return 3.14f * radius * radius;
        }
    }
}
