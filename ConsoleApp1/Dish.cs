using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    public class Dish
    {
        [Required] public string Name { get; set; } = null!;
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        
        public bool IsVegetarian { get; set; }
    }
}