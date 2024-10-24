using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    public class Waiter : Person
    {
        [Required] public Table AssignedTable { get; set; } = null!;
    }
}