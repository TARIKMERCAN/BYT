using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Models;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class NonMember : SerializableObject<NonMember>
    {
        [Required(ErrorMessage = "NonMember ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "NonMember ID must be a positive integer.")]
        public int Id { get; set; }

        
        //METHODS
        public Member BeMember(int idMember, int initialCreditPoints = 0, string? email = null)
        {
            Member newMember = new Member(idMember, initialCreditPoints)
            {
                Email = email
            };
            
            Member.AddInstance(newMember);

            Console.WriteLine($"NonMember {Id} has become a Member with ID {idMember}.");
            
            return newMember;
        }
    }
}