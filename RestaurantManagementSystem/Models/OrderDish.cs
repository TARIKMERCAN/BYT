using System.ComponentModel.DataAnnotations;
using RestaurantManagementSystem.Services;

namespace RestaurantManagementSystem.Models
{
   public class OrderDish : SerializableObject<OrderDish>
    {
        [Required(ErrorMessage = "Dish is required.")]
        public Dish Dish { get; private set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public int Quantity { get; private set; }

        public decimal UnitPrice => Dish.Price;
        public decimal TotalPrice => Quantity * UnitPrice;

        // Constructor
        public OrderDish(Dish dish, int quantity)
        {
            Dish = dish ?? throw new ArgumentNullException(nameof(dish));
            if (dish.Price < 0)
                throw new ArgumentException("Dish price cannot be negative.", nameof(dish));

            Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            dish.AddOrderDish(this); // Reverse connection
        }

        // Method to Update the Quantity
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

            Quantity = newQuantity;
            LogAction($"Quantity updated to {newQuantity} for Dish {Dish.Name}.");
        }

        // Method to Remove Reverse Connection
        public void RemoveFromDish()
        {
            Dish.RemoveOrderDish(this); // Ensure `Dish` class supports this method
            LogAction($"Removed OrderDish for Dish {Dish.Name}.");
        }

        // Bulk Update Method
        public static void UpdateQuantities(IEnumerable<OrderDish> orderDishes, int newQuantity)
        {
            foreach (var orderDish in orderDishes)
            {
                orderDish.UpdateQuantity(newQuantity);
            }
        }

        private void LogAction(string message)
        {
            Console.WriteLine(message); // Replace with structured logging in production
        }

        // Overrides
        public override bool Equals(object? obj)
        {
            return obj is OrderDish other && Dish.Equals(other.Dish) && Quantity == other.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Dish, Quantity);
        }

        public override string ToString()
        {
            return $"OrderDish [Dish: {Dish.Name}, Quantity: {Quantity}, Total Price: {TotalPrice:C}]";
        }
    }
}