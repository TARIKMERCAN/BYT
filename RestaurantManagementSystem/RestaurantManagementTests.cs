using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Tests
{
    [TestFixture]
    public class RestaurantManagementTests
    {
        // Test the constructor with valid input
        [Test]
        public void Chef_Constructor_ValidInput_ShouldCreateInstance()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            Assert.AreEqual(1, chef.IdChef);
            Assert.AreEqual("Italian", chef.CuisineType);
            Assert.AreEqual(10, chef.KitchenExperience);
            Assert.AreEqual(new DateTime(2015, 1, 1), chef.DateOfJoining);
        }

        // Test the constructor with an invalid DateOfJoining (future date)
        [Test]
        public void Chef_Constructor_InvalidDateOfJoining_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ExecutiveChef(1, "Italian", 10, DateTime.Now.AddDays(1)));
        }

        // Test calculation of YearsSinceJoining
        [Test]
        public void Chef_YearsSinceJoining_ShouldCalculateCorrectly()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2020, 1, 1));
            var expectedYears = DateTime.Now.Year - 2020;
            Assert.AreEqual(expectedYears, chef.YearsSinceJoining);
        }

        // Test adding a restaurant to the AssociatedRestaurants list
        [Test]
        public void Chef_AddRestaurant_ShouldAddRestaurantToAssociatedList()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            var restaurant = new Restaurant("Italiano", 50);

            chef.AddRestaurant(restaurant);

            Assert.Contains(restaurant, chef.AssociatedRestaurants.ToList());
        }

        // Test adding a null restaurant to the AssociatedRestaurants list
        [Test]
        public void Chef_AddRestaurant_NullRestaurant_ShouldThrowArgumentNullException()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            Assert.Throws<ArgumentNullException>(() => chef.AddRestaurant(null));
        }

        // Test removing a restaurant from the AssociatedRestaurants list
        [Test]
        public void Chef_RemoveRestaurant_ShouldRemoveFromAssociatedList()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            var restaurant = new Restaurant("Italiano", 50);

            chef.AddRestaurant(restaurant);
            chef.RemoveRestaurant(restaurant);

            Assert.IsFalse(chef.AssociatedRestaurants.Contains(restaurant));
        }

        // Test removing a null restaurant from the AssociatedRestaurants list
        [Test]
        public void Chef_RemoveRestaurant_NullRestaurant_ShouldThrowArgumentNullException()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            Assert.Throws<ArgumentNullException>(() => chef.RemoveRestaurant(null));
        }

        // Test adding a prepared dish to the list
        [Test]
        public void Chef_AddPreparedDish_ShouldAddDishToPreparedDishesList()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));

            chef.AddPreparedDish("Pasta Carbonara");

            Assert.Contains("Pasta Carbonara", chef.PreparedDishes.ToList());
        }

        // Test adding an invalid dish name to the prepared dishes list
        [Test]
        public void Chef_AddPreparedDish_InvalidDishName_ShouldThrowArgumentException()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            Assert.Throws<ArgumentException>(() => chef.AddPreparedDish(null));
            Assert.Throws<ArgumentException>(() => chef.AddPreparedDish(string.Empty));
        }

        // Test clearing all prepared dishes from the list
        [Test]
        public void Chef_ClearPreparedDishes_ShouldRemoveAllDishesFromList()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));

            chef.AddPreparedDish("Pasta Carbonara");
            chef.AddPreparedDish("Risotto");

            chef.ClearPreparedDishes();

            Assert.IsEmpty(chef.PreparedDishes);
        }

        // Test logging actions when a dish is added
        [Test]
        public void Chef_LoggingActions_ShouldTriggerEvents()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            string loggedAction = null;

            chef.OnActionLogged += action => loggedAction = action;

            chef.AddPreparedDish("Pizza");

            Assert.AreEqual("Chef 1: Prepared dish 'Pizza' added to the list.", loggedAction);
        }

        // Test the PrepareSpecialDish method for ExecutiveChef
        [Test]
        public void ExecutiveChef_PrepareSpecialDish_ShouldLogSpecialDishPreparation()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            string loggedAction = null;

            chef.OnActionLogged += action => loggedAction = action;

            chef.PrepareSpecialDish();

            Assert.AreEqual("Chef 1: Executive Chef 1 is preparing a signature dish.", loggedAction);
        }

        // Test the TrainSousChef method for ExecutiveChef
        [Test]
        public void ExecutiveChef_TrainSousChef_ShouldTriggerTrainingEvent()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            string trainingEventLog = null;

            chef.OnTrainingEvent += eventLog => trainingEventLog = eventLog;

            chef.TrainSousChef();

            Assert.AreEqual($"Executive Chef 1 is training a Sous Chef on {DateTime.Now}.", trainingEventLog);
        }

        // Test the constructor for SousChef
        [Test]
        public void SousChef_Constructor_ShouldInitializeProperly()
        {
            var chef = new SousChef(2, "French", "Pastry", new DateTime(2020, 1, 1));

            Assert.AreEqual(2, chef.IdChef);
            Assert.AreEqual("French", chef.CuisineType);
            Assert.AreEqual("Pastry", chef.SupervisedSections);
        }

        // Test the PrepareSpecialDish method for SousChef
        [Test]
        public void SousChef_PrepareSpecialDish_ShouldLogSpecialDishPreparation()
        {
            var chef = new SousChef(2, "French", "Pastry", new DateTime(2020, 1, 1));
            string loggedAction = null;

            chef.OnActionLogged += action => loggedAction = action;

            chef.PrepareSpecialDish();

            Assert.AreEqual("Chef 2: Sous Chef 2 is preparing a special pastry.", loggedAction);
        }

        // Test if Chef has IdChef attribute
        [Test]
        public void Chef_HasIdChefAttribute_ShouldValidateAttribute()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));

            Assert.AreEqual(1, chef.IdChef, "Chef ID is not correctly assigned.");
        }

        // Test if Chef has CuisineType attribute
        [Test]
        public void Chef_HasCuisineTypeAttribute_ShouldValidateAttribute()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));

            Assert.AreEqual("Italian", chef.CuisineType, "CuisineType is not correctly assigned.");
        }

        // Test AssignTask method for Chef
        [Test]
        public void Chef_AssignTask_ShouldLogAssignedTask()
        {
            var chef = new ExecutiveChef(1, "Italian", 10, new DateTime(2015, 1, 1));
            string loggedAction = null;

            chef.OnActionLogged += action => loggedAction = action;

            chef.AssignTask("Prepare event menu");

            Assert.AreEqual("Chef 1: Task 'Prepare event menu' assigned.", loggedAction);
        }
    }
    [TestFixture]
public class CustomerTests
{
    // Test the constructor with valid inputs
    [Test]
    public void Customer_Constructor_ValidInput_ShouldCreateInstance()
    {
        var customer = new Customer(1, "customer@example.com");

        Assert.AreEqual(1, customer.IdCustomer);
        Assert.AreEqual("customer@example.com", customer.Email);
    }

    // Test the constructor with a duplicate email
    [Test]
    public void Customer_Constructor_DuplicateEmail_ShouldThrowException()
    {
        var customer1 = new Customer(1, "duplicate@example.com");

        Assert.Throws<InvalidOperationException>(() => new Customer(2, "duplicate@example.com"));
    }

    // Test GetCustomerByEmail with an existing email
    [Test]
    public void Customer_GetCustomerByEmail_ExistingEmail_ShouldReturnCustomer()
    {
        var customer = new Customer(1, "customer@example.com");

        var retrievedCustomer = Customer.GetCustomerByEmail("customer@example.com");

        Assert.AreEqual(customer, retrievedCustomer);
    }

    // Test GetCustomerByEmail with a non-existent email
    [Test]
    public void Customer_GetCustomerByEmail_NonExistentEmail_ShouldReturnNull()
    {
        var result = Customer.GetCustomerByEmail("nonexistent@example.com");

        Assert.IsNull(result);
    }

    // Test AddReservation adds a reservation to the customer's list
    [Test]
    public void Customer_AddReservation_ValidReservation_ShouldAddToList()
    {
        var customer = new Customer(1, "customer@example.com");
        var reservation = new Reservation(1, DateTime.Now.AddDays(1), new Table(1, 4, "Standard"), customer);

        customer.AddReservation(reservation);

        Assert.Contains(reservation, customer.Reservations.ToList());
    }

    // Test AddReservation with a duplicate reservation
    [Test]
    public void Customer_AddReservation_DuplicateReservation_ShouldThrowException()
    {
        var customer = new Customer(1, "customer@example.com");
        var reservation = new Reservation(1, DateTime.Now.AddDays(1), new Table(1, 4, "Standard"), customer);

        customer.AddReservation(reservation);

        Assert.Throws<InvalidOperationException>(() => customer.AddReservation(reservation));
    }

    // Test RemoveReservation removes a reservation from the customer's list
    [Test]
    public void Customer_RemoveReservation_ValidReservation_ShouldRemoveFromList()
    {
        var customer = new Customer(1, "customer@example.com");
        var reservation = new Reservation(1, DateTime.Now.AddDays(1), new Table(1, 4, "Standard"), customer);

        customer.AddReservation(reservation);
        customer.RemoveReservation(reservation);

        Assert.IsFalse(customer.Reservations.Contains(reservation));
    }

    // Test RemoveReservation with a non-existent reservation
    [Test]
    public void Customer_RemoveReservation_NonExistentReservation_ShouldThrowException()
    {
        var customer = new Customer(1, "customer@example.com");
        var reservation = new Reservation(1, DateTime.Now.AddDays(1), new Table(1, 4, "Standard"), customer);

        Assert.Throws<InvalidOperationException>(() => customer.RemoveReservation(reservation));
    }

    // Test PlaceOrder creates a new order with dishes
    [Test]
    public void Customer_PlaceOrder_ValidDishes_ShouldCreateOrder()
    {
        var customer = new Customer(1, "customer@example.com");
        var dishes = new[]
        {
            new Dish(
                "Pasta", 
                "Italian", 
                12.99m, 
                false, 
                new List<string> { "Pasta", "Cream", "Cheese" } 
            ),
            new Dish(
                "Pizza", // Name (string)
                "Italian", // Cuisine (string)
                15.99m, // Price (decimal)
                false, // IsVegetarian (bool)
                new List<string> { "Dough", "Tomato Sauce", "Cheese" }
            )
        };
        
        var order = customer.PlaceOrder(dishes);

        Assert.IsNotNull(order);
        Assert.AreEqual(2, order.OrderDishes.Count);
        Assert.AreEqual(customer, order.Customer);
    }

    // Test MakePayment successfully processes payment
    [Test]
    public void Customer_MakePayment_ValidPayment_ShouldProcessSuccessfully()
    {
        var customer = new Customer(1, "customer@example.com");
        var order = new Order(1, customer);
        var payment = new Payment(1, (double)order.CalculateTotal(), "1234567812345678", "Customer Name");


        bool result = customer.MakePayment(order, payment);

        Assert.IsTrue(result);
        Assert.IsTrue(order.IsPaid);
        Assert.Contains(payment, customer.Payments.ToList());
    }

    // Test MakePayment for an already paid order
    [Test]
    public void Customer_MakePayment_AlreadyPaidOrder_ShouldThrowException()
    {
        var customer = new Customer(1, "customer@example.com");
        var order = new Order(1, customer);
        order.MarkAsPaid();

        var payment = new Payment(1, (double)order.CalculateTotal(), "1234567812345678", "Customer Name");


        Assert.Throws<InvalidOperationException>(() => customer.MakePayment(order, payment));
    }

    // Test ClearReservations removes all reservations
    [Test]
    public void Customer_ClearReservations_ShouldRemoveAllReservations()
    {
        var customer = new Customer(1, "customer@example.com");
        var reservation1 = new Reservation(1, DateTime.Now.AddDays(1), new Table(1, 4, "Standard"), customer);
        var reservation2 = new Reservation(2, DateTime.Now.AddDays(2), new Table(2, 4, "Standard"), customer);

        customer.AddReservation(reservation1);
        customer.AddReservation(reservation2);

        customer.ClearReservations();

        Assert.IsEmpty(customer.Reservations);
    }

    // Test ClearOrders removes all orders
    [Test]
    public void Customer_ClearOrders_ShouldRemoveAllOrders()
    {
        var customer = new Customer(1, "customer@example.com");
        var order1 = new Order(1, customer);
        var order2 = new Order(2, customer);

        customer.AddOrder(order1);
        customer.AddOrder(order2);

        customer.ClearOrders();

        Assert.IsEmpty(customer.Orders);
    }

    // Test ToString provides correct summary
    [Test]
    public void Customer_ToString_ShouldReturnCorrectSummary()
    {
        var customer = new Customer(1, "customer@example.com");

        string result = customer.ToString();

        Assert.AreEqual("Customer [ID: 1, Email: customer@example.com, Orders Count: 0, Payments Count: 0]", result);
    }
}

}
