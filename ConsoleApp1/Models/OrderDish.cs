using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class OrderDish : SerializableObject<OrderDish>
    {
        [Required(ErrorMessage = "Dish is required.")]
        public Dish Dish { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public int Quantity { get; set; }
        public decimal UnitPrice => Dish.Price;
        public decimal TotalPrice => Quantity * UnitPrice;

        public OrderDish(){}
        public OrderDish(Dish dish, int quantity)
        {
            if (dish == null) throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            Dish = dish;
            Quantity = quantity;
        }

        
        //METHODS
        public void DisplayOrderDish()
        {
            Console.WriteLine($"Dish: {Dish.Name}, Quantity: {Quantity}, Unit Price: {UnitPrice:C}, Total Price: {TotalPrice:C}");
        }
    }
}