using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1 {
    public abstract class Employee : Person
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        public int EmployeeId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateOfEmployment { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? DateOfLeave { get; set; }
        
    }
}