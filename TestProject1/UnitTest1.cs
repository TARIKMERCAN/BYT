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
            Assert.AreEqual(1, chef.IdChef);
            Assert.AreEqual("Italian", chef.CuisineType);
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
            Assert.AreEqual(1, order.TotalItems);
            Assert.AreEqual(10.00m, order.TotalAmount);
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

            Assert.AreEqual("Spaghetti Carbonara", dish.Name);
            Assert.AreEqual(12.50m, dish.Price);
            Assert.IsFalse(dish.IsVegan);
        }

        [Test]
        public void ChangeDish_ChangeNameAndPrice_UpdatesCorrectly()
        {
            var dish = new Dish { IdDish = 1, Name = "Burger", Price = 8.00m };
            dish.ChangeDish("Vegan Burger", price: 10.00m);
            Assert.AreEqual("Vegan Burger", dish.Name);
            Assert.AreEqual(10.00m, dish.Price);
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
            Assert.AreEqual(1, order.TotalItems);
            Assert.AreEqual(15.00m, order.TotalAmount);
        }

        [Test]
        public void CalculateTotal_MultipleDishes_CalculatesCorrectTotal()
        {
            var order = new Order();
            order.AddItem(new Dish { IdDish = 1, Name = "Pizza", Price = 15.00m },1);
            order.AddItem(new Dish { IdDish = 2, Name = "Salad", Price = 5.00m },1);
            var total = order.CalculateTotal();
            Assert.AreEqual(20.00m, total);
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
            Assert.AreEqual(PaymentStatus.Completed, payment.Status);
        }

        [Test]
        public void RefundPayment_CompletedPayment_RefundsSuccessfully()
        {
            var payment = new Payment
                { IdPayment = 1, Amount = 100.00m, Method = PaymentMethod.Card, Status = PaymentStatus.Completed };
            var result = payment.RefundPayment();
            Assert.IsTrue(result);
            Assert.AreEqual(PaymentStatus.Refunded, payment.Status);
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
            Assert.AreEqual(366, employedTime);
        }

        [Test]
        public void GetEmployedTime_WithoutLeavingDate_UsesCurrentDate()
        {
            var hiringDate = DateTime.Now.AddDays(-100);
            var employee = new Employee { IdEmployee = 1, DateOfHiring = hiringDate };
            var employedTime = employee.GetEmployedTime();
            Assert.AreEqual(100, employedTime, 1);
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
            Assert.AreEqual(101, _member.CreditPoints);
        }

        [Test]
        public void UseCredits_EnoughCredits_ReturnsTrue()
        {
            bool result = _member.UseCredits(50);
            Assert.IsTrue(result);
            Assert.AreEqual(50, _member.CreditPoints);
        }

        [Test]
        public void UseCredits_NotEnoughCredits_ReturnsFalse()
        {
            bool result = _member.UseCredits(150);
            Assert.IsFalse(result);
            Assert.AreEqual(100, _member.CreditPoints);
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
            Assert.AreEqual(2, member.IdMember);
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
            Assert.AreEqual(2, _orderDish.Quantity);
            Assert.AreEqual(_dish, _orderDish.Dish);
        }

        [Test]
        public void TotalPrice_CalculateBasedOnQuantity_CalculatesCorrectly()
        {
            var totalPrice = _orderDish.TotalPrice;
            Assert.AreEqual(7.00m, totalPrice);
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
            Assert.AreEqual("John", person.FirstName);
            Assert.AreEqual("Doe", person.LastName);
            Assert.AreEqual("123-456-7890", person.PhoneNumber);
        }

        [Test]
        public void ValidateBirthDate_FutureDate_ReturnsError()
        {
            var validationContext = new ValidationContext(new Person());
            var birthOfDate = DateTime.Now.AddDays(1);
            var result = Person.ValidateBirthDate(birthOfDate, validationContext);
            Assert.IsNotNull(result);
            Assert.AreEqual("Birth date cannot be in the future.", result.ErrorMessage);
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
            _reservation = new Reservation { IdReservation = 1, DateOfReservation = DateTime.Now.AddDays(1) };
        }

        [Test]
        public void ReserveTable_ValidTable_ReservesSuccessfully()
        {
            bool result = _reservation.ReserveTable(_table);
            Assert.IsTrue(result);
            Assert.AreEqual(_table, _reservation.ReservedTable);
        }

        [Test]
        public void ReserveTable_PastDate_ReturnsFalse()
        {
            var pastDateReservation = new Reservation
                { IdReservation = 2, DateOfReservation = DateTime.Now.AddDays(-1) };
            bool result = pastDateReservation.ReserveTable(_table);
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
            Assert.AreEqual(4, table.NumberOfChairs);
            Assert.AreEqual("Rectangle", table.TableType);
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
            Assert.AreEqual(1, SerializableObject<Chef>.Instances.Count);
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
            Assert.AreEqual(1, SerializableObject<Chef>.Instances.Count);
            Assert.AreEqual("Italian", SerializableObject<Chef>.Instances.First().CuisineType);
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
            Assert.AreEqual(0, SerializableObject<Chef>.Instances.Count, "Instances should be empty when loading from an empty file.");
        }
    }
}