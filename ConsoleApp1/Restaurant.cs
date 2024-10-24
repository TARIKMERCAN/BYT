namespace ConsoleApp1
{
    public class Restaurant
    {
        public string Name { get; set; } = null!;
        public List<Table> Tables { get; set; } = null!;
        public List<Order> Orders { get; set; } = null!;
        public Menu Menu { get; set; } = null!;
    }
}