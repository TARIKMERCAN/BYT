using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Table : SerializableObject<Table>
    {
        [Required(ErrorMessage = "Table ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Table ID must be a positive integer.")]
        public int IdTable { get; set; }

        [Required(ErrorMessage = "Number of chairs is required.")]
        [Range(1, 20, ErrorMessage = "Number of chairs must be between 1 and 20.")]
        public int NumberOfChairs { get; set; }

        [Required(ErrorMessage = "Table type is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Table type must be between 3 and 50 characters.")]
        public string TableType { get; set; } 

        public Table(){}
        public Table(int idTable, int numberOfChairs, string tableType)
        {
            IdTable = idTable;
            NumberOfChairs = numberOfChairs;
            TableType = tableType;
        }
        
       
        //OVERRIDES
        public override bool Equals(object? obj)
        {
            if (obj is Table other)
            {
                return IdTable == other.IdTable && NumberOfChairs == other.NumberOfChairs && TableType == other.TableType;
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(IdTable, NumberOfChairs, TableType);
        }
        
        public override string ToString()
        {
            return $"Table {IdTable} - Type: {TableType}, Chairs: {NumberOfChairs}";
        }
    }
}