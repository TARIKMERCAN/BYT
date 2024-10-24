using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1
{
    public class Chef : Person
    {
        [Required] public string CuisineType { get; set; } = null!;
    }
}