using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Enums;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Customer : SerializableObject<Customer>
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Customer ID must be a positive integer.")]
        public int IdCustomer { get; set; }


        //METHODS
        public Order PlaceOrder(Dish[] dishes)
        {
            if (dishes == null || dishes.Length == 0)
            {
                throw new ArgumentException("At least one dish must be ordered.");
            }

            Order order = new Order { IdOrder = Order.Instances.Count + 1 };
            foreach (var dish in dishes)
            {
                int quantity = 1;
                order.AddItem(dish, quantity);
            }

            Order.AddInstance(order);

            Console.WriteLine($"Order {order.IdOrder} placed by Customer {IdCustomer} with {dishes.Length} dishes.");
            return order;
        }

        public bool MakePayment(Order order, PaymentMethod paymentMethod)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            decimal amount = order.CalculateTotal();

            Payment payment = new Payment
            {
                IdPayment = Payment.Instances.Count + 1,
                Amount = amount,
                Method = paymentMethod
            };

            bool paymentSuccess = payment.ProcessPayment();
            if (paymentSuccess)
            {
                Payment.AddInstance(payment);
                Console.WriteLine(
                    $"Payment of {payment.Amount} for Order {order.IdOrder} completed by Customer {IdCustomer}.");
                return true;
            }
            else
            {
                Console.WriteLine($"Payment failed for Order {order.IdOrder} by Customer {IdCustomer}.");
                return false;
            }
        }
        
        //OVERRIDES
        public override bool Equals(object obj)
        {
            if (obj is not Customer other)
                return false;

            return IdCustomer == other.IdCustomer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdCustomer);
        }

        public override string ToString()
        {
            return $"Customer(IdCustomer={IdCustomer})";
        }
    }
}