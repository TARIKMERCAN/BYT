using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models {
    public class Order : SerializableObject<Order>
    {
        [Required(ErrorMessage = "Order ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Order ID must be a positive integer.")]
        public int IdOrder { get; set; }

        [Required(ErrorMessage = "Timestamp is required.")]
        public DateTime TimeStamp { get; set; } = DateTime.Now; 
        public decimal TotalAmount => CalculateTotal();
        public List<Dish> Items { get; private set; } = new List<Dish>();

        
        //METHODS
        public void AddItem(Dish dish)
        {
            if (dish == null)
            {
                throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
            }

            Items.Add(dish);
            Console.WriteLine($"Dish '{dish.Name}' added to Order {IdOrder}. Current total: {TotalAmount:C}");
        }
        
        public decimal CalculateTotal()
        {
            return Items.Sum(dish => dish.Price);
        }
    }
}