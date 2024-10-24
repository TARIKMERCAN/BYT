namespace ConsoleApp1
{

    public abstract class Customer : Person
    {
        public List<Reservations> Reservations { get; set; } = new List<Reservations>();

        public virtual void MakePayment(Payment payment)
        {
        }
    }
}