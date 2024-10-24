using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    public class Payment
    {
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public PaymentMethod Method { get; set; }
    }
}