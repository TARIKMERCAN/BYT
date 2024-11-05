using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Chef : SerializableObject<Chef>
    {
        [Required(ErrorMessage = "Chef ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Chef ID must be a positive integer.")]
        public int IdChef { get; set; }

        [Required(ErrorMessage = "Cuisine type is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Cuisine type must be between 3 and 50 characters.")]
        public string CuisineType { get; set; }
        
        
        public Chef(){}
        public Chef(int idChef, string cuisineType)
        {
            IdChef = idChef;
            CuisineType = cuisineType;
        }
        
        
        //METHODS
        public void AssignTask(string task)
        {
            if (string.IsNullOrWhiteSpace(task))
            {
                throw new ArgumentException("Task cannot be null or empty.", nameof(task));
            }
            Console.WriteLine($"Chef {IdChef} (Cuisine: {CuisineType}) has been assigned the task: {task}");
        }
    }
}