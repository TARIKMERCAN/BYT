using System.ComponentModel.DataAnnotations;
using RestaurantManagementSystem.Services;

namespace RestaurantManagementSystem.Models
{
    public class Order : SerializableObject<Order>
    {
        private static readonly List<Order> OrderExtentList = new();
        public static IReadOnlyCollection<Order> OrderExtent => OrderExtentList.AsReadOnly();

        [Required(ErrorMessage = "Order ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Order ID must be a positive integer.")]
        public int IdOrder { get; private set; }

        [Required(ErrorMessage = "Timestamp is required.")]
        public DateTime TimeStamp { get; private set; } = DateTime.Now;
        
        public bool IsPaid { get; private set; } = false;

        public List<OrderDish> OrderDishes { get; private set; } = new();

        [Required]
        public Customer Customer { get; private set; }

        // Constructor
        public Order(int idOrder, Customer customer)
        {
            if (idOrder <= 0)
                throw new ArgumentException("Order ID must be greater than zero.", nameof(idOrder));

            if (OrderExtentList.Any(o => o.IdOrder == idOrder))
                throw new InvalidOperationException($"Order with ID {idOrder} already exists.");

            Customer = customer ?? throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");

            IdOrder = idOrder;
            OrderExtentList.Add(this);
            customer.AddOrder(this); // Reverse connection
        }

        // Method to Add an OrderDish to the Order
        public void AddOrderDish(OrderDish orderDish)
        {
            if (orderDish == null)
                throw new ArgumentNullException(nameof(orderDish), "OrderDish cannot be null.");

            if (orderDish.Quantity <= 0)
                throw new ArgumentException("OrderDish quantity must be greater than zero.");

            var existingItem = OrderDishes.FirstOrDefault(d => d.Dish.Name == orderDish.Dish.Name);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + orderDish.Quantity); // Increment quantity
                LogAction($"Updated quantity of {orderDish.Dish.Name} in Order {IdOrder}.");
            }
            else
            {
                OrderDishes.Add(orderDish);
                LogAction($"Added {orderDish.Quantity}x {orderDish.Dish.Name} to Order {IdOrder}.");
            }
        }

        // Method to Remove an OrderDish from the Order
        public void RemoveOrderDish(OrderDish orderDish)
        {
            if (orderDish == null)
                throw new ArgumentNullException(nameof(orderDish), "OrderDish cannot be null.");

            if (!OrderDishes.Remove(orderDish))
                throw new InvalidOperationException($"OrderDish {orderDish.Dish.Name} not found in Order {IdOrder}.");

            orderDish.RemoveFromDish(); // Cleanup reverse connection
            LogAction($"Removed {orderDish.Dish.Name} from Order {IdOrder}.");
        }

        // Method to Cancel the Order
        public void CancelOrder()
        {
            if (IsPaid)
                throw new InvalidOperationException($"Order {IdOrder} is already paid and cannot be canceled.");

            foreach (var orderDish in OrderDishes.ToList())
                RemoveOrderDish(orderDish); // Ensure reverse connections are cleaned

            OrderExtentList.Remove(this);
            Customer.RemoveOrder(this); // Reverse connection cleanup
            LogAction($"Order {IdOrder} has been canceled.");
        }

        // Method to Calculate the Total
        public decimal CalculateTotal()
        {
            return OrderDishes.Sum(item => item.TotalPrice);
        }

        // Method to Display the Order
        public string DisplayOrder()
        {
            var items = string.Join("\n", OrderDishes.Select(item =>
                $"- {item.Quantity}x {item.Dish.Name} (${item.UnitPrice} each)"));

            return $"Order ID: {IdOrder}\nCustomer: {Customer.IdCustomer}\nTimestamp: {TimeStamp}\nItems:\n{items}\nTotal: ${CalculateTotal():0.00}";
        }
        
        public void MarkAsPaid()
        {
            if (IsPaid)
                throw new InvalidOperationException($"Order {IdOrder} is already paid.");

            IsPaid = true;
            LogAction($"Order {IdOrder} marked as paid.");
        }

        // Static Method to Cancel All Orders for a Specific Customer
        public static void CancelOrdersForCustomer(int customerId)
        {
            foreach (var order in OrderExtentList.Where(o => o.Customer.IdCustomer == customerId).ToList())
            {
                order.CancelOrder();
            }
        }

        // Static Method to Clear Extent
        public static void ClearExtent()
        {
            Console.WriteLine($"Clearing OrderExtentList. Current count: {OrderExtentList.Count}");
            OrderExtentList.Clear();
        }
        
        public void UnassignCustomer()
        {
            if (Customer == null)
                throw new InvalidOperationException($"Order {IdOrder} is not associated with any customer.");

            Customer.RemoveOrder(this); // Cleanup reverse connection

            // Assign a placeholder customer or throw exception based on your requirements
            throw new InvalidOperationException("Customer cannot be null after unassignment.");
        }

        private void LogAction(string message)
        {
            Console.WriteLine(message); // Replace with structured logging in production
        }

        public override string ToString()
        {
            return $"Order [ID: {IdOrder}, Customer: {Customer.IdCustomer}, Total: ${CalculateTotal():0.00}]";
        }
    }
}
