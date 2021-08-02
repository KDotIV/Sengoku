using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Sengoku.API.Models;
using Sengoku.API.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Sengoku.API;

namespace Migrator
{
    class Program
    {
        private static IMongoCollection<User> _userCollection;
        private static IMongoCollection<Events> _eventsCollection;
        private static IMongoCollection<Products> _productsCollection;
        private static IMongoCollection<Orders> _ordersCollection;
        private static IMongoCollection<Tickets> _ticketsCollection;

        static string mongoConnectionString = "mongodb+srv://shogun:821XVvZqg@province.rk4mm.mongodb.net/Sengoku?retryWrites=true&w=majority";

        public static async Task Main(string [] args)
        {
            Setup();
            Console.WriteLine("Starting the data migration.");
            //await BulkInsertEvents(10);
            //await BulkInsertUsers(50);
            //await BulkInsertProducts(20);
            await BulkInsertOrders(25);
            //await BulkInsertTickets(28);
        }
        public static async Task BulkInsertUsers(int nUsers)
        {
            var listWrites = new List<WriteModel<User>>();
            var names = new List<string>();
            var userIds = new List<string>();

            for (int i = 0; i < nUsers; i++)
            {
                var name = Helpers.MakeUniqueUserName(names);
                names.Add(name);
                var newId = Helpers.MakeRandomID(userIds);
                userIds.Add(newId);

                var newUser = new User
                {
                    userId = newId,
                    Name = name,
                    Email = Helpers.MakeUserEmail(name),
                    CreatedAt = DateTime.Now,
                    IsBlocked = false,
                    LastUpdated = DateTime.Now
                };

                listWrites.Add(new InsertOneModel<User>(newUser));
            }

            var resultWrites = await _userCollection.BulkWriteAsync(listWrites);
            Console.WriteLine($"OK?: {resultWrites.IsAcknowledged} - Inserted User Count: {resultWrites.InsertedCount}");
        }
        public static async Task BulkInsertEvents(int nEvents)
        {
            var listWrites = new List<WriteModel<Events>>();
            var eventNames = new List<string>();
            var addresses = new List<Address>();
            var eventIDs = new List<string>();

            for(int i = 0; i < nEvents; i++)
            {
                var eventName = Helpers.MakeUniqueEventName(eventNames);
                eventNames.Add(eventName);

                var eventAddress = Helpers.MakeUniqueAddress(addresses);
                addresses.Add(eventAddress);

                var eventId = Helpers.MakeRandomID(eventIDs);
                eventIDs.Add(eventId);

                var newEvent = new Events
                {
                    Name = eventName,
                    Address = eventAddress,
                    Status = "Not Started",
                    Date = Helpers.GetRandomOrderPlaced(),
                    Event_Id = eventId,
                    City = Helpers.GetRandomCity(),
                    Game = Helpers.GetRandomGame(),
                    LastUpdated = Helpers.GetRandomOrderPlaced()
                };
                listWrites.Add(new InsertOneModel<Events>(newEvent));
            }
            var resultWrites = await _eventsCollection.BulkWriteAsync(listWrites);
            Console.WriteLine($"OK?: {resultWrites.IsAcknowledged} - Inserted Events Count: {resultWrites.InsertedCount}");
        }
        public static async Task BulkInsertProducts(int nProducts)
        {
            var listWrites = new List<WriteModel<Products>>();

            for (int i = 0; i < nProducts; i++)
            {
                var newId = Helpers.MakeRandomID();
                var newName = Helpers.MakeUniqueEventName();
                decimal newPrice = Helpers.rand.Next((10), 500);
                var newStock = Helpers.rand.Next(1, 50);

                Products newProduct = new Products
                {
                    Product_Id = newId,
                    Product_Name = newName,
                    Price = newPrice,
                    Stock = newStock,
                    Supplier = Helpers.MakeUniqueEventName()
                };
                listWrites.Add(new InsertOneModel<Products>(newProduct));
            }
            var resultWrites = await _productsCollection.BulkWriteAsync(listWrites);
            Console.WriteLine($"OK?: {resultWrites.IsAcknowledged} - Inserted Products Count: {resultWrites.InsertedCount}");
        }
        public static async Task BulkInsertOrders(int nOrders)
        {
            var listWrites = new List<WriteModel<Orders>>();
            List<User> userList = GetUsers();
            List<Products> prodList = GetAllProducts();

            for (int i = 0; i < nOrders; i++)
            {
                string newOrderId = Helpers.MakeRandomID();
                if (_ordersCollection.AsQueryable<Orders>().Any(exists => exists.Order_Id == newOrderId))
                {
                    newOrderId = Helpers.MakeRandomID();
                }
                var index = Helpers.rand.Next(0, userList.Count);
                var orderAddress = Helpers.MakeUniqueAddress();
                var prodIndex = Helpers.rand.Next(0, prodList.Count);
                int randomIndex = Helpers.rand.Next(1, 5);
                Products[] products = new Products[randomIndex];
                decimal totalCost = 0.00M;

                for (int j = 0; j < randomIndex; j++)
                {
                    products[j] = prodList[prodIndex];
                    totalCost += products[j].Price;
                }

                var newOrder = new Orders
                {
                    Order_Id = newOrderId,
                    User = userList[index],
                    Shipping_Address = orderAddress,
                    Billing_Address = orderAddress,
                    OrderDate = Helpers.GetRandomOrderPlaced(),
                    Products = products,
                    Processing_Fee = 0.03M,
                    Total_Cost = totalCost,
                    Payment_Amount = totalCost,
                    Payment_Method = "paypal",
                    Transaction_Id = Helpers.MakeRandomID()
                };
                listWrites.Add(new InsertOneModel<Orders>(newOrder));
            }
            var resultWrites = await _ordersCollection.BulkWriteAsync(listWrites);
            Console.WriteLine($"OK?: {resultWrites.IsAcknowledged} - Inserted Orders Count: {resultWrites.InsertedCount}");
        }
        public static async Task BulkInsertTickets(int nTickets)
        {
            var listWrites = new List<WriteModel<Tickets>>();
            List<User> userList = GetUsers();
            List<Events> eventList = GetAllEvents();

            for (int i = 0; i < nTickets; i++)
            {
                string newTicketId = Helpers.MakeRandomID();
                if (_ticketsCollection.AsQueryable().Any(exists => exists.Confirmation_Id == newTicketId))
                {
                    newTicketId = Helpers.MakeRandomID();
                }
                var index = Helpers.rand.Next(0, userList.Count);
                var orderAddress = Helpers.MakeUniqueAddress();
                var eventIndex = Helpers.rand.Next(0, eventList.Count);
                int randomIndex = Helpers.rand.Next(1, 5);

                var newTicket = new Tickets
                {
                    Confirmation_Id = newTicketId,
                    Pass_Type = "Competitor",
                    Event = eventList[eventIndex],
                    User_Id = userList[index].Name,
                    Payment_Date = Helpers.GetRandomOrderPlaced(),
                    Venue_Fee = 20.00M,
                    Processing_Fee = 0.03M,
                    Total_Cost = 20.00M + 0.03M,
                    Payment_Amount = 20.03M,
                    Payment_Method = "paypal",
                    Transaction_Id = Helpers.MakeRandomID()
                };
                listWrites.Add(new InsertOneModel<Tickets>(newTicket));
            }
            var resultWrites = await _ticketsCollection.BulkWriteAsync(listWrites);
            Console.WriteLine($"OK?: {resultWrites.IsAcknowledged} - Inserted Tickets Count: {resultWrites.InsertedCount}");
        }
        public static List<User> GetUsers()
        {
            var users = _userCollection
                .Find(user => true)
                .Project<User>(Builders<User>.Projection.Exclude("password").Exclude("_id"))
                .ToList();

            return users;
        }
        public static List<Products> GetAllProducts()
        {
            var products = _productsCollection
                .Find(products => true)
                .Project<Products>(Builders<Products>.Projection.Exclude("_id"))
                .ToList();

            return products;
        }
        public static List<Events> GetAllEvents()
        {
            var events = _eventsCollection
                .Find(events => true)
                .Project<Events>(Builders<Events>.Projection.Exclude("_id"))
                .ToList();

            return events;
        }
        static void Setup()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            var mongoUri = mongoConnectionString;
            var _client = new MongoClient(mongoUri);
            var sengokuDatabase = _client.GetDatabase("Sengoku");
            _userCollection = sengokuDatabase.GetCollection<User>("Users");
            _eventsCollection = sengokuDatabase.GetCollection<Events>("Events");
            _productsCollection = sengokuDatabase.GetCollection<Products>("Products");
            _ordersCollection = sengokuDatabase.GetCollection<Orders>("Orders");
            _ticketsCollection = sengokuDatabase.GetCollection<Tickets>("Tickets");
        }
    }
}
