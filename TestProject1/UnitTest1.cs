using ConsoleApp1;

namespace TestProject1;

public class UnitTest1
{
    public class OrderTests
    {
        private Order _order;

        [SetUp]
        public void Setup()
        {
            _order = new Order
            {
                Dishes = new List<Dish>(),
                Table = new Table { TableId = 1, NumberOfChairs = 4 },
                Payment = new Payment { Amount = 0, Method = PaymentMethod.Cash }
            };
        }

        [Test]
        public void CalculateTotal_SumsDishPricesCorrectly()
        {
            _order.Dishes.Add(new Dish { Name = "Pizza", Price = 15.0m });
            _order.Dishes.Add(new Dish { Name = "Salad", Price = 5.0m });
            _order.Dishes.Add(new Dish { Name = "Water", Price = 2.0m });

            Assert.AreEqual(22.0m, _order.CalculateTotal());
        }

        [Test]
        public void CalculateTotal_ReturnsZeroWhenNoDishes()
        {
            Assert.AreEqual(0m, _order.CalculateTotal());
        }

        [Test]
        public void CalculateTotal_HandlesNegativePrices() 
        {
            _order.Dishes.Add(new Dish { Name = "Gift Card", Price = -10.0m });
            _order.Dishes.Add(new Dish { Name = "Cake", Price = 20.0m });

            Assert.AreEqual(10.0m, _order.CalculateTotal());
        }

        [Test]
        public void CalculateTotal_WithHighPrecisionPrices()
        {
            _order.Dishes.Add(new Dish { Name = "Truffle", Price = 99.995m });
            _order.Dishes.Add(new Dish { Name = "Pasta", Price = 20.005m });

            Assert.AreEqual(120.00m, _order.CalculateTotal()); 
        }
        
        [Test]
        public void CalculateTotal_IncludesAllDishTypes()
        {
            _order.Dishes.Add(new Dish { Name = "Vegan Burger", Price = 8.99m, IsVegetarian = true });
            _order.Dishes.Add(new Dish { Name = "Chicken Wings", Price = 12.50m, IsVegetarian = false });

            Assert.AreEqual(21.49m, _order.CalculateTotal());
        }

        [Test]
        public void CalculateTotal_WithEmptyDishes_ShouldReturnZero()
        {
            Assert.AreEqual(0m, _order.CalculateTotal());
        }
        
        [Test]
        public void CalculateTotal_WithDiscountApplied()
        {
            _order.Dishes.Add(new Dish { Name = "Steak", Price = 25.0m });
            _order.Dishes.Add(new Dish { Name = "Wine", Price = 45.0m });
            
            decimal expectedTotal = (25.0m + 45.0m) * 0.9m;
            Assert.AreEqual(expectedTotal, _order.CalculateTotal() * 0.9m);
        }

        [Test]
        public void CalculateTotal_WhenAddingAndRemovingDishes()
        {
            _order.Dishes.Add(new Dish { Name = "Pasta", Price = 15.0m });
            _order.Dishes.Add(new Dish { Name = "Ice Cream", Price = 5.0m });

            _order.Dishes.RemoveAt(1);  

            Assert.AreEqual(15.0m, _order.CalculateTotal());
        }

        [Test]
        public void CalculateTotal_WithRounding()
        {
            _order.Dishes.Add(new Dish { Name = "Coffee", Price = 2.995m });
            _order.Dishes.Add(new Dish { Name = "Bagel", Price = 2.995m });

            Assert.AreEqual(5.99m, _order.CalculateTotal());  
        }

        [Test]
        public void CalculateTotal_ReflectsUpdatesToDishPrices()
        {
            var dish = new Dish { Name = "Sushi", Price = 10.0m };
            _order.Dishes.Add(dish);
            
            dish.Price = 12.0m;

            Assert.AreEqual(12.0m, _order.CalculateTotal());
        }
    }
}
