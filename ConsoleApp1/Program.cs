using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using ConsoleApp1.Services;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TestSerialization();

            var member = new Member(7, 5) { Email = "s12345@pjwstk.edu.pl" };
            var dishes = new List<Dish>
            {
                new Dish { IdDish = 1, Name = "Fettuccine Alfredo", Price = 12.99m },
                new Dish { IdDish = 2, Name = "Caesar Salad", Price = 8.50m }
            }.ToArray();

            var order = member.PlaceOrder(dishes);
            Console.WriteLine($"Order ID: {order.IdOrder} has been placed with {order.Items.Count} dishes totaling {order.TotalAmount:C}.");

            
            var table = new Table(10, 4, "Standard");
            var reservation = new Reservation(1, DateTime.Now.AddDays(1));
            bool reservationResult = reservation.ReserveTable(table);
            Console.WriteLine(reservationResult ? "Table reserved successfully." : "Table reservation failed.");
            
            await ShowSerializationDemo();
        }

        static async Task ShowSerializationDemo()
        {
            var reservations = await SerializationManager.DeserializeFromXmlAsync<Reservation>();
            Console.WriteLine($"Loaded {reservations.Count} reservations from XML.");
            await SerializationManager.SerializeToXmlAsync(reservations);
            Console.WriteLine("Reservations have been serialized to XML.");
            
            var members = await SerializationManager.DeserializeFromJsonAsync<Member>();
            Console.WriteLine($"Loaded {members.Count} members from JSON.");
            await SerializationManager.SerializeToJsonAsync(members);
            Console.WriteLine("Members have been serialized to JSON.");
        }

        static async Task TestSerialization()
        {
            var reservations = await SerializationManager.DeserializeFromXmlAsync<Reservation>();
            Console.WriteLine($"Loaded {reservations.Count} reservations from XML.");

            var members = await SerializationManager.DeserializeFromJsonAsync<Member>();
            Console.WriteLine($"Loaded {members.Count} members from JSON.");
        }
    }
}
