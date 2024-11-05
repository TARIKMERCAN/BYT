using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Member : SerializableObject<Member>
    {
        [Required(ErrorMessage = "Member ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Member ID must be a positive integer.")]
        public int IdMember { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        private int creditPoints;

        [Range(0, int.MaxValue, ErrorMessage = "Credit points cannot be negative.")]
        public int CreditPoints
        {
            get { return creditPoints; }
            private set { creditPoints = Math.Max(0, value); }
        }
        
        public Member() { }
        public Member(int idMember, int initialCreditPoints = 0)
        {
            IdMember = idMember;
            CreditPoints = initialCreditPoints;
        }
        
        
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
                order.AddItem(dish);
            }

            Order.AddInstance(order);
            CreditPoints++;
            Console.WriteLine($"Order {order.IdOrder} placed by Member {IdMember}. Credit points increased to {CreditPoints}.");
            return order;
        }

        public bool UseCredits(int pointsToUse)
        {
            if (pointsToUse <= 0)
            {
                Console.WriteLine("Points to use must be greater than zero.");
                return false;
            }

            if (CreditPoints < 10)
            {
                Console.WriteLine($"Insufficient credit points to use. Minimum required: 10, Available: {CreditPoints}");
                return false;
            }

            if (pointsToUse > CreditPoints)
            {
                Console.WriteLine($"Insufficient credit points. Available: {CreditPoints}, Requested: {pointsToUse}");
                return false;
            }

            CreditPoints -= pointsToUse;
            Console.WriteLine($"{pointsToUse} credit points used. Remaining points: {CreditPoints}");
            return true;
        }
    }
}
