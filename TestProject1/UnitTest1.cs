using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Enums;
using ConsoleApp1.Models;
using ConsoleApp1.Services;

namespace TestProject1;

public class UnitTest1
{
    [TestFixture]
    public class ChefTests
    {
        [Test]
        public void Constructor_InitializesNewChef_Successfully()
        {
            var chef = new Chef(1, "Italian");
            Assert.That(chef.IdChef, Is.EqualTo(1));
            Assert.That(chef.CuisineType, Is.EqualTo("Italian"));
        }

        [Test]
        public void AssignTask_ValidTask_AssignsSuccessfully()
        {
            var chef = new Chef(1, "Italian");
            Assert.DoesNotThrow(() => chef.AssignTask("Prepare pizza"));
        }

        [Test]
        public void AssignTask_NullOrEmptyTask_ThrowsArgumentException()
        {
            var chef = new Chef(1, "Italian");
            var ex = Assert.Throws<ArgumentException>(() => chef.AssignTask(null));
            Assert.That(ex.Message, Does.Contain("Task cannot be null or empty."));
            Assert.That(ex.ParamName, Is.EqualTo("task"));
        }
    }

    [TestFixture]
    public class CustomerTests
    {
        private Customer _customer;
        private Dish[] _dishes;
     
        [SetUp]
        public void Setup()
        {
            _customer = new Customer { IdCustomer = 1 };
            _dishes = new[] { new Dish { IdDish = 1, Name = "Pizza", Price = 10.00m } };
        }

        [Test]
        public void PlaceOrder_WithValidDishes_ReturnsOrder()
        {
            var order = _customer.PlaceOrder(_dishes);
            Assert.That(order.TotalItems, Is.EqualTo(1));
            Assert.That(order.TotalAmount, Is.EqualTo(10.00m));
        }

        [Test]
        public void MakePayment_ValidOrder_ReturnsTrue()
        {
            var order = _customer.PlaceOrder(_dishes);
            bool result = _customer.MakePayment(order, PaymentMethod.Card);
            Assert.IsTrue(result);
        }

        [Test]
        public void PlaceOrder_EmptyDishesArray_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _customer.PlaceOrder(new Dish[0]));
            Assert.That(ex.Message, Is.EqualTo("At least one dish must be ordered."));
        }
    }

    [TestFixture]
    public class DishTests
    {
        [Test]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            var dish = new Dish
            {
                IdDish = 1,
                Name = "Spaghetti Carbonara",
                Cuisine = "Italian",
                IsVegetarian = false,
                IsVegan = false,
                Price = 12.50m,
                Ingredients = new List<string> { "Pasta", "Eggs", "Cheese", "Pancetta" }
            };
            
            Assert.That(dish.Name, Is.EqualTo("Spaghetti Carbonara"));
            Assert.That(dish.Price, Is.EqualTo(12.50m));
            Assert.IsFalse(dish.IsVegan);
        }

        [Test]
        public void ChangeDish_ChangeNameAndPrice_UpdatesCorrectly()
        {
            var dish = new Dish { IdDish = 1, Name = "Burger", Price = 8.00m };
            dish.ChangeDish("Vegan Burger", price: 10.00m);
            Assert.That(dish.Name, Is.EqualTo("Vegan Burger"));
            Assert.That(dish.Price, Is.EqualTo(10.00m));
        }
    }

    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void AddItem_ValidDish_IncreasesItemCount()
        {
            var order = new Order();
            var dish = new Dish { IdDish = 1, Name = "Pizza", Price = 15.00m };
            order.AddItem(dish,1);
            Assert.That(order.TotalItems, Is.EqualTo(1));
            Assert.That(order.TotalAmount, Is.EqualTo(15.00m));
        }

        [Test]
        public void CalculateTotal_MultipleDishes_CalculatesCorrectTotal()
        {
            var order = new Order();
            order.AddItem(new Dish { IdDish = 1, Name = "Pizza", Price = 15.00m },1);
            order.AddItem(new Dish { IdDish = 2, Name = "Salad", Price = 5.00m },1);
            var total = order.CalculateTotal();
            Assert.That(total, Is.EqualTo(20.00m));
        }
    }

    [TestFixture]
    public class PaymentTests
    {
        [Test]
        public void ProcessPayment_WhenPending_SetsStatusToCompleted()
        {
            var payment = new Payment { IdPayment = 1, Amount = 100.00m, Method = PaymentMethod.Card };
            var result = payment.ProcessPayment();
            Assert.IsTrue(result);
            Assert.That(payment.Status, Is.EqualTo(PaymentStatus.Completed));
        }

        [Test]
        public void RefundPayment_CompletedPayment_RefundsSuccessfully()
        {
            var payment = new Payment
                { IdPayment = 1, Amount = 100.00m, Method = PaymentMethod.Card, Status = PaymentStatus.Completed };
            var result = payment.RefundPayment();
            Assert.IsTrue(result);
            Assert.That(payment.Status, Is.EqualTo(PaymentStatus.Refunded));
        }
    }

    [TestFixture]
    public class ExecutiveChefTests
    {
        private ExecutiveChef _chef;

        [SetUp]
        public void Setup()
        {
            _chef = new ExecutiveChef(1, "French", 15);
        }

        [Test]
        public void OverseeKitchen_CallsMethod_PrintsMessage()
        {
            _chef.OverseeKitchen();
        }

        [Test]
        public void TrainSousChef_ValidSousChef_TrainsSuccessfully()
        {
            var sousChef = new Chef(2, "French");
            Assert.DoesNotThrow(() => _chef.TrainSousChef(sousChef));
        }
    }

    [TestFixture]
    public class EmployeeTests
    {
        [Test]
        public void GetEmployedTime_WithLeavingDate_CalculatesCorrectDuration()
        {
            var hiringDate = new DateTime(2020, 1, 1);
            var leavingDate = new DateTime(2021, 1, 1);
            var employee = new Employee { IdEmployee = 1, DateOfHiring = hiringDate, DateOfLeaving = leavingDate };
            var employedTime = employee.GetEmployedTime();
            Assert.That(employedTime, Is.EqualTo(366));
        }

        [Test]
        public void GetEmployedTime_WithoutLeavingDate_UsesCurrentDate()
        {
            var hiringDate = DateTime.Now.AddDays(-100);
            var employee = new Employee { IdEmployee = 1, DateOfHiring = hiringDate };
            var employedTime = employee.GetEmployedTime();
            Assert.That(employedTime, Is.EqualTo(100));
        }
    }

    [TestFixture]
    public class ManagerTests
    {
        private Manager _manager;
        private Waiter _waiter;
        private Table _table;

        [SetUp]
        public void Setup()
        {
            _manager = new Manager { IdManager = 1, Level = ManagerLevel.MidLevel };
            _waiter = new Waiter { IdWaiter = 1 };
            _table = new Table { IdTable = 1, NumberOfChairs = 4, TableType = "Standard" };
        }

        [Test]
        public void AssignTableToWaiter_ValidParameters_AssignsTableSuccessfully()
        {
            bool result = _manager.AssignTableToWaiter(_waiter, _table);
            Assert.IsTrue(result);
            Assert.IsTrue(_waiter.AssignedTables.Contains(_table));
        }

        [Test]
        public void AssignTableToWaiter_AlreadyAssignedTable_ReturnsFalse()
        {
            _waiter.AssignTable(_table);
            bool result = _manager.AssignTableToWaiter(_waiter, _table);
            Assert.IsFalse(result);
        }
    }

    [TestFixture]
    public class MemberTests
    {
        private Member _member;
        private Dish[] _dishes;

        [SetUp]
        public void Setup()
        {
            _member = new Member(1, 100);
            _dishes = new[] { new Dish { IdDish = 1, Name = "Pasta", Price = 12.99m } };
        }

        [Test]
        public void PlaceOrder_WithValidDishes_IncreasesCreditPoints()
        {
            var order = _member.PlaceOrder(_dishes);
            Assert.That(_member.CreditPoints, Is.EqualTo(101));
        }

        [Test]
        public void UseCredits_EnoughCredits_ReturnsTrue()
        {
            bool result = _member.UseCredits(50);
            Assert.IsTrue(result);
            Assert.That(_member.CreditPoints, Is.EqualTo(50));
        }

        [Test]
        public void UseCredits_NotEnoughCredits_ReturnsFalse()
        {
            bool result = _member.UseCredits(150);
            Assert.IsFalse(result);
            Assert.That(_member.CreditPoints, Is.EqualTo(100));
        }
    }

    [TestFixture]
    public class MenuTests
    {
        private Menu _menu;
        private Dish _dish;

        [SetUp]
        public void Setup()
        {
            _menu = new Menu("Lunch Specials", "Lunch");
            _dish = new Dish { IdDish = 1, Name = "Burger", Price = 8.99m };
        }

        [Test]
        public void AddDish_ValidDish_AddsSuccessfully()
        {
            _menu.AddDish(_dish);
            Assert.Contains(_dish, _menu.Dishes);
        }

        [Test]
        public void RemoveDish_ValidId_RemovesSuccessfully()
        {
            _menu.AddDish(_dish);
            bool result = _menu.RemoveDish(1);
            Assert.IsTrue(result);
            Assert.IsEmpty(_menu.Dishes);
        }
    }


    [TestFixture]
    public class NonMemberTests
    {
        [Test]
        public void BeMember_ValidConversion_ReturnsMember()
        {
            var nonMember = new NonMember { Id = 1 };
            var member = nonMember.BeMember(2);
            Assert.IsNotNull(member);
            Assert.That(member.IdMember, Is.EqualTo(2));
        }
    }


    [TestFixture]
    public class OrderDishTests
    {
        private Dish _dish;
        private OrderDish _orderDish;

        [SetUp]
        public void Setup()
        {
            _dish = new Dish { IdDish = 1, Name = "Cake", Price = 3.50m };
            _orderDish = new OrderDish(_dish, 2);
        }

        [Test]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            Assert.That(_orderDish.Quantity, Is.EqualTo(2));
            Assert.That(_orderDish.Dish, Is.EqualTo(_dish));
        }

        [Test]
        public void TotalPrice_CalculateBasedOnQuantity_CalculatesCorrectly()
        {
            var totalPrice = _orderDish.TotalPrice;
            Assert.That(totalPrice, Is.EqualTo(7.00m));
        }
    }

    [TestFixture]
    public class PersonTests
    {
        [Test]
        public void Constructor_ValidData_InitializesCorrectly()
        {
            var person = new Person
            {
                IdPerson = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthOfDate = new DateTime(1990, 1, 1),
                PhoneNumber = "123-456-7890"
            };
            Assert.That(person.FirstName, Is.EqualTo("John"));
            Assert.That(person.LastName, Is.EqualTo("Doe"));
            Assert.That(person.PhoneNumber, Is.EqualTo("123-456-7890"));
        }

        [Test]
        public void ValidateBirthDate_FutureDate_ReturnsError()
        {
            var validationContext = new ValidationContext(new Person());
            var birthOfDate = DateTime.Now.AddDays(1);
            var result = Person.ValidateBirthDate(birthOfDate, validationContext);
            Assert.IsNotNull(result);
            Assert.That(result.ErrorMessage, Is.EqualTo("Birth date cannot be in the future."));
        }
    }

    [TestFixture]
    public class ReservationTests
    {
        private Reservation _reservation;
        private Table _table;
        

        [SetUp]
        public void Setup()
        {
            _table = new Table { IdTable = 1, NumberOfChairs = 4, TableType = "Round" };
            _reservation = new Reservation(1, DateTime.Now.AddDays(1));
        }

        [Test]
        public void ReserveTable_ValidTable_ReservesSuccessfully()
        {
            bool result = _reservation.ReserveTable(_table, 2, 6); 
            Assert.IsTrue(result, "The reservation should succeed for valid table and reservation.");
            Assert.That(_reservation.ReservedTable, Is.EqualTo(_table));
        }
        
        [Test]
        public void ReserveTable_PastDate_ReturnsFalse()
        {
            var pastDateReservation = new Reservation { IdReservation = 2, DateOfReservation = DateTime.Now.AddDays(-1) };
            bool result = pastDateReservation.ReserveTable(_table, 4, 1); 
            Assert.IsFalse(result);
        }
    }

    [TestFixture]
    public class RestaurantTests
    {
        private Restaurant _restaurant;
        private Table _table;

        [SetUp]
        public void Setup()
        {
            _restaurant = new Restaurant { Name = "Gourmet Place", MaxCapacity = 50 };
            _table = new Table { IdTable = 1, NumberOfChairs = 4, TableType = "Rectangle" };
        }

        [Test]
        public void AddTable_ValidTable_IncreasesTableCount()
        {
            _restaurant.AddTable(_table);
            Assert.Contains(_table, _restaurant.Tables);
        }

        [Test]
        public void RemoveTable_ExistingTable_RemovesSuccessfully()
        {
            _restaurant.AddTable(_table);
            bool result = _restaurant.RemoveTable(1);
            Assert.IsTrue(result);
            Assert.IsEmpty(_restaurant.Tables);
        }
    }


    [TestFixture]
    public class SousChefTests
    {
        private SousChef _sousChef;

        [SetUp]
        public void Setup()
        {
            _sousChef = new SousChef(1, "French", "Pastry", 3);
        }

        [Test]
        public void PrepareSpecials_CallsMethod_PrintsMessage()
        {
            _sousChef.PrepareSpecials();
        }

        [Test]
        public void AssistHeadChef_ValidChef_AssistsSuccessfully()
        {
            var headChef = new ExecutiveChef(2, "French", 20);
            Assert.DoesNotThrow(() => _sousChef.AssistHeadChef(headChef));
        }
    }


    [TestFixture]
    public class TableTests
    {
        [Test]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            var table = new Table { IdTable = 1, NumberOfChairs = 4, TableType = "Rectangle" };
            Assert.That(table.NumberOfChairs, Is.EqualTo(4));
            Assert.That(table.TableType, Is.EqualTo("Rectangle"));
        }
    }


    [TestFixture]
    public class ValeTests
    {
        private Vale _vale;

        [SetUp]
        public void Setup()
        {
            _vale = new Vale { IdVale = 1, AssignedLocation = "Front Entrance" };
        }

        [Test]
        public void DisplayValeInfo_CorrectlyDisplaysInformation()
        {
            _vale.DisplayValeInfo();
        }
    }


    [TestFixture]
    public class WaiterTests
    {
        private Waiter _waiter;
        private Table _table;

        [SetUp]
        public void Setup()
        {
            _waiter = new Waiter { IdWaiter = 1 };
            _table = new Table { IdTable = 1, NumberOfChairs = 4, TableType = "Square" };
        }

        [Test]
        public void AssignTable_ValidTable_AssignsSuccessfully()
        {
            _waiter.AssignTable(_table);
            Assert.Contains(_table, _waiter.AssignedTables);
        }

        [Test]
        public void UnassignTable_TableAssigned_RemovesSuccessfully()
        {
            _waiter.AssignTable(_table);
            var result = _waiter.UnassignTable(1);
            Assert.IsTrue(result);
            Assert.IsEmpty(_waiter.AssignedTables);
        }

        [Test]
        public void UnassignTable_TableNotAssigned_ReturnsFalse()
        {
            var result = _waiter.UnassignTable(2);
            Assert.IsFalse(result);
        }
    }

    [TestFixture]
    public class SerializableObjectTests
    {
        [Test]
        public void AddInstance_AddsCorrectly()
        {
            var chef = new Chef { IdChef = 1, CuisineType = "Italian" };
            SerializableObject<Chef>.AddInstance(chef);
            Assert.Contains(chef, SerializableObject<Chef>.Instances);
            SerializableObject<Chef>.Instances.Clear();
        }

        [Test]
        public void AddInstance_DuplicateIsNotAdded()
        {
            var chef1 = new Chef { IdChef = 1, CuisineType = "Italian" };
            var chef2 = new Chef { IdChef = 1, CuisineType = "Italian" };
            SerializableObject<Chef>.AddInstance(chef1);
            SerializableObject<Chef>.AddInstance(chef2);
            Assert.That(SerializableObject<Chef>.Instances.Count, Is.EqualTo(1));
            SerializableObject<Chef>.Instances.Clear();
        }

        [Test]
        public void SaveExtent_SavesToXmlFile()
        {
            var chef = new Chef { IdChef = 1, CuisineType = "Italian" };
            SerializableObject<Chef>.AddInstance(chef);
            SerializableObject<Chef>.SaveExtent();
            var filePath = $"{typeof(Chef).Name}_Extent.xml";
            Assert.IsTrue(File.Exists(filePath), "XML file should be saved.");
            File.Delete(filePath);
            SerializableObject<Chef>.Instances.Clear();
        }

        [Test]
        public void LoadExtent_LoadsFromXmlFile()
        {
            var chef = new Chef { IdChef = 1, CuisineType = "Italian" };
            SerializableObject<Chef>.AddInstance(chef);
            SerializableObject<Chef>.SaveExtent();
            SerializableObject<Chef>.Instances.Clear();
            SerializableObject<Chef>.LoadExtent();
            Assert.That(SerializableObject<Chef>.Instances.Count, Is.EqualTo(1));
            Assert.That(SerializableObject<Chef>.Instances.First().CuisineType, Is.EqualTo("Italian"));
            File.Delete($"{typeof(Chef).Name}_Extent.xml");
        }

        [Test]
        public void SaveExtent_IncludesMetadata()
        {
            var chef = new Chef { IdChef = 1, CuisineType = "Italian" };
            SerializableObject<Chef>.AddInstance(chef);
            SerializableObject<Chef>.SaveExtent();
            var filePath = $"{typeof(Chef).Name}_Extent.xml";
            var xmlContent = File.ReadAllText(filePath);
            Assert.IsTrue(xmlContent.Contains("<Metadata>"), "Metadata should be included in the XML file.");
            File.Delete(filePath);
            SerializableObject<Chef>.Instances.Clear();
        }

        [Test]
        public void LoadExtent_HandlesEmptyFileGracefully()
        {
            var filePath = $"{typeof(Chef).Name}_Extent.xml";
            if (File.Exists(filePath)) File.Delete(filePath);
            SerializableObject<Chef>.LoadExtent();
            Assert.AreEqual(0, SerializableObject<Chef>.Instances.Count,
                "Instances should be empty when loading from an empty file.");
        }

        [Test]
        public void AddOrder_SetsReverseConnection_CustomerAndOrderConnected()
        {
            var customer = new Customer { IdCustomer = 1 };
            var order = new Order { IdOrder = 1 };

            customer.AddOrder(order);

            Assert.That(order.Customer, Is.EqualTo(customer));
            Assert.Contains(order, customer.Orders.ToList());
        }

        [Test]
        public void RemoveOrder_RemovesReverseConnection()
        {
            var customer = new Customer { IdCustomer = 1 };
            var order = new Order { IdOrder = 1 };

            customer.AddOrder(order);
            customer.RemoveOrder(order);

            Assert.IsNull(order.Customer);
            Assert.IsFalse(customer.Orders.Contains(order));
        }

        [Test]
        public void AddOrder_NullOrder_ThrowsException()
        {
            var customer = new Customer { IdCustomer = 1 };
            Assert.Throws<ArgumentNullException>(() => customer.AddOrder(null));
        }

        [Test]
        public void AddOrder_DuplicateOrder_DoesNotAddTwice()
        {
            var customer = new Customer { IdCustomer = 1 };
            var order = new Order { IdOrder = 1 };

            customer.AddOrder(order);
            customer.AddOrder(order);
            Assert.That(customer.Orders.Count, Is.EqualTo(1));
        }

        [Test]
        public void DiscountedPrice_WithZeroPrice_ReturnsZero()
        {
            var dish = new Dish { Price = 0.00m };
            Assert.That(dish.DiscountedPrice, Is.EqualTo(0.00m));
        }

        [Test]
        public void PriceAfterTax_CorrectlyCalculatesWithVAT()
        {
            var dish = new Dish { Price = 100.00m, VatPercentage = 0.2m };
            Assert.That(dish.PriceAfterTax, Is.EqualTo(120.00m));
        }

        [Test]
        public void AddInstance_SavesToExtentCorrectly()
        {
            var customer = new Customer { IdCustomer = 1 };
            SerializableObject<Customer>.AddInstance(customer);
            Assert.Contains(customer, SerializableObject<Customer>.Instances);
        }

        [Test]
        public void LoadExtent_ClearsPreviousInstances()
        {
            var customer1 = new Customer { IdCustomer = 1 };
            var customer2 = new Customer { IdCustomer = 2 };
            SerializableObject<Customer>.AddInstance(customer1);
            SerializableObject<Customer>.AddInstance(customer2);
            SerializableObject<Customer>.Instances.Clear();

            SerializableObject<Customer>.LoadExtent();
            Assert.IsEmpty(SerializableObject<Customer>.Instances);
        }

        [Test]
        public void RemoveRestaurant_RemovesAllTables()
        {
            var restaurant = new Restaurant { Name = "Test", MaxCapacity = 50 };
            var table = new Table { IdTable = 1, NumberOfChairs = 4, TableType = "Standard" };

            restaurant.AddTable(table);
            SerializableObject<Restaurant>.AddInstance(restaurant);

            SerializableObject<Restaurant>.Instances.Remove(restaurant);

            Assert.IsEmpty(SerializableObject<Table>.Instances);
        }

        [Test]
        public void AddIngredient_AddsSuccessfully()
        {
            var dish = new Dish { Name = "Pasta" };
            dish.Ingredients.Add("Tomato");
            Assert.Contains("Tomato", dish.Ingredients);
        }

        [Test]
        public void AddIngredient_NullValue_ThrowsException()
        {
            var dish = new Dish { Name = "Pasta" };
            Assert.Throws<ArgumentException>(() => dish.AddIngredient(null));
        }

        
        [Test]
        public void AddTable_ExceedMaxCapacity_ThrowsException()
        {
            var restaurant = new Restaurant { Name = "Test", MaxCapacity = 1 };
            var table = new Table { IdTable = 1, NumberOfChairs = 4, TableType = "Standard" };

            restaurant.AddTable(table);
            var anotherTable = new Table { IdTable = 2, NumberOfChairs = 2, TableType = "Round" };

            Assert.Throws<InvalidOperationException>(() => restaurant.AddTable(anotherTable));
        }

        [TestCase(100.00, 0.2, 120.00)]
        [TestCase(200.00, 0.1, 220.00)]
        public void PriceAfterTax_CalculatesCorrectly(decimal price, decimal vat, decimal expected)
        {
            var dish = new Dish { Price = price, VatPercentage = vat };
            Assert.That(dish.PriceAfterTax, Is.EqualTo(expected));
        }

        [TestFixture]
        public class DiscountTests
        {
            [TestCase(100, 0.1, 90)]
            [TestCase(50, 0.2, 40)]
            public void DiscountedPrice_CalculatesCorrectly(decimal price, decimal vatPercentage, decimal expected)
            {
                var dish = new Dish { Price = price, VatPercentage = vatPercentage };
                Assert.That(dish.DiscountedPrice, Is.EqualTo(expected));
            }
        }
    }
}