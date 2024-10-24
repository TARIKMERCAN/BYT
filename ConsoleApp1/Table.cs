using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    public class Table
    {
        [Required]
        public int TableId { get; set; }
        
        [Range(1, 20, ErrorMessage = "Number of chairs must be between 1 and 20.")]
        public int NumberOfChairs { get; set; }
    }
}