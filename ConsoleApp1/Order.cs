using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1 {
    public class Order
    {
        public Table Table { get; set; } = null!;
        
        [Required]
        public List<Dish> Dishes { get; set; }

        [Required] public Payment Payment { get; set; } = null!;
        
        public decimal CalculateTotal() => Dishes.Sum(d => d.Price);
    }
}