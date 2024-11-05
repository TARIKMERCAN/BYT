using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Models
{
    public class ExecutiveChef : Chef
    {
        [Required(ErrorMessage = "Kitchen experience is required.")]
        [Range(1, 50, ErrorMessage = "Kitchen experience must be between 1 and 50 years.")]
        public int KitchenExperience { get; set; } 
        
        public ExecutiveChef(int idChef, string cuisineType, int kitchenExperience)
            : base(idChef, cuisineType)
        {
            KitchenExperience = kitchenExperience;
        }

        
        //METHODS
        public void OverseeKitchen()
        {
            Console.WriteLine($"Executive Chef {IdChef} is overseeing the kitchen operations.");
        }
        
        public void ApproveMenuChanges(Menu menu)
        {
            if (menu == null)
            {
                throw new ArgumentNullException(nameof(menu), "Menu cannot be null.");
            }

            Console.WriteLine($"Executive Chef {IdChef} has approved changes to the {menu.Name} menu.");
        }
        
        public void TrainSousChef(Chef sousChef)
        {
            if (sousChef == null)
            {
                throw new ArgumentNullException(nameof(sousChef), "Sous Chef cannot be null.");
            }

            Console.WriteLine($"Executive Chef {IdChef} is training Sous Chef with ID {sousChef.IdChef} in {sousChef.CuisineType} cuisine.");
        }
    }
}