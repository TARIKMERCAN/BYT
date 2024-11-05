using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Menu : SerializableObject<Menu>
    {
        [Required(ErrorMessage = "Menu name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Menu name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Menu type is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Menu type must be between 3 and 50 characters.")]
        public string MenuType { get; set; }

        public List<string> AvailableLanguages => new List<string> { "English", "Polish", "Turkish" };

        public List<Dish> Dishes { get; private set; } = new List<Dish>();

        public Menu()
        {
        }

        public Menu(string name, string menuType)
        {
            Name = name;
            MenuType = menuType;
        }


        //METHODS
        public void ChangeMenu(string? newName = null, string? newMenuType = null)
        {
            if (!string.IsNullOrEmpty(newName))
            {
                Name = newName;
            }

            if (!string.IsNullOrEmpty(newMenuType))
            {
                MenuType = newMenuType;
            }

            Console.WriteLine($"Menu '{Name}' has been updated.");
        }

        public int GetNumberOfPositions()
        {
            return Dishes.Count;
        }

        public void AddDish(Dish dish)
        {
            if (dish == null) throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
            Dishes.Add(dish);
            Console.WriteLine($"Dish '{dish.Name}' added to the menu '{Name}'.");
        }

        public bool RemoveDish(int dishId)
        {
            var dish = Dishes.Find(d => d.IdDish == dishId);
            if (dish != null)
            {
                Dishes.Remove(dish);
                Console.WriteLine($"Dish '{dish.Name}' removed from the menu '{Name}'.");
                return true;
            }

            Console.WriteLine($"Dish with ID {dishId} not found in the menu '{Name}'.");
            return false;
        }
    }
}