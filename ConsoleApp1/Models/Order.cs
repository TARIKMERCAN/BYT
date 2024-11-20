using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Models;
using ConsoleApp1.Services;

public class Order : SerializableObject<Order>
{
    [Required(ErrorMessage = "Order ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Order ID must be a positive integer.")]
    public int IdOrder { get; set; }

    [Required(ErrorMessage = "Timestamp is required.")]
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    
    public decimal TotalAmount => CalculateTotal();  
    
    public List<OrderDish> OrderDishes { get; private set; } = new List<OrderDish>();  
    
    public int TotalItems => OrderDishes.Sum(orderDish => orderDish.Quantity);  
    
    public void AddItem(Dish dish, int quantity)
    {
        if (dish == null)
        {
            throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
        }
        
        var existingOrderDish = OrderDishes.FirstOrDefault(od => od.Dish.Equals(dish) && od.Quantity == quantity);
        
        if (existingOrderDish != null)
        {
            Console.WriteLine($"Dish '{dish.Name}' with quantity {quantity} is already added to Order {IdOrder}.");
        }
        else
        {
            var orderDish = new OrderDish(dish, quantity);  
            OrderDishes.Add(orderDish);  
            Console.WriteLine($"Dish '{dish.Name}' (Quantity: {quantity}) added to Order {IdOrder}. Current total: {TotalAmount:C}");
        }
    }
    
    public decimal CalculateTotal()
    {
        return OrderDishes.Sum(orderDish => orderDish.TotalPrice);  
    }

    // OVERRIDES 
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Order)obj;
        return IdOrder == other.IdOrder && TimeStamp == other.TimeStamp && OrderDishes.SequenceEqual(other.OrderDishes);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdOrder, TimeStamp, OrderDishes);
    }

    public override string ToString()
    {
        return $"Order(ID: {IdOrder}, TimeStamp: {TimeStamp}, Total Amount: {TotalAmount:C})";
    }
}
