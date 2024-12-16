using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Reservation : SerializableObject<Reservation>
    {
        [Required(ErrorMessage = "Reservation ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Reservation ID must be a positive integer.")]
        public int IdReservation { get; set; }

        [Required(ErrorMessage = "Date of reservation is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date and time format.")]
        public DateTime DateOfReservation { get; set; }
        
        public Table ReservedTable { get; set; } 

        public Reservation() { }

        public Reservation(int idReservation, DateTime dateOfReservation)
        {
            IdReservation = idReservation;
            DateOfReservation = dateOfReservation;
        }

        
        // METHODS
        public bool ReserveTable(Table table, int minCapacity, int maxCapacity)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table), "Table cannot be null.");

            if (table.NumberOfChairs < minCapacity || table.NumberOfChairs > maxCapacity)
            {
                Console.WriteLine($"Table {table.IdTable} does not meet capacity requirements: " +
                                  $"Required: {minCapacity}-{maxCapacity}, Available: {table.NumberOfChairs}.");
                return false;
            }

            if (DateOfReservation.Date < DateTime.Now.Date)
            {
                Console.WriteLine($"Cannot reserve table for past dates. Attempted reservation for {DateOfReservation}.");
                return false;
            }

            bool isAvailable = CheckTableAvailability(table, DateOfReservation);

            if (!isAvailable)
            {
                Console.WriteLine($"Table {table.IdTable} is not available on {DateOfReservation}.");
                return false;
            }

            ReservedTable = table;
            Console.WriteLine($"Reservation {IdReservation} confirmed for Table {table.IdTable}.");
            return true;
        }



        private bool CheckTableAvailability(Table table, DateTime reservationDate)
        {
            var existingReservations = Reservation.Instances;
            foreach (var reservation in existingReservations)
            {
                if (reservation.ReservedTable.IdTable == table.IdTable &&
                    reservation.DateOfReservation.Date == reservationDate.Date)
                {
                    Console.WriteLine($"Conflict found: Table {table.IdTable} already reserved for {reservation.DateOfReservation}.");
                    return false; 
                }
            }
            return true; 
        }

        
        
        //OVERRIDES
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
    
            var other = (Reservation)obj;
            return IdReservation == other.IdReservation && DateOfReservation == other.DateOfReservation && ReservedTable.Equals(other.ReservedTable);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdReservation, DateOfReservation, ReservedTable);
        }

        public override string ToString()
        {
            return $"Reservation [ID: {IdReservation}, Date: {DateOfReservation}, Table ID: {ReservedTable?.IdTable}]";
        }
    }
}
