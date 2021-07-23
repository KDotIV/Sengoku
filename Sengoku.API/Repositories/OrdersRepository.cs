using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;

namespace Sengoku.API.Repositories
{
    public class OrdersRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<Orders> _ordersCollection;
        private const int DefaultOrderPerPage = 20;
        private const int DefaultSortOrder = -1;
        private readonly ProjectionDefinition<Orders> staticExclution = Builders<Orders>.Projection.Exclude("_id");

        public OrdersRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _ctx = context;
            _ordersCollection = _ctx.GetCollection<Orders>("Orders");
        }

        public async Task<List<Orders>> GetAllOrders(int ordersPerPage = DefaultOrderPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = ordersPerPage * page;
            var limit = ordersPerPage;

            var orders = await _ordersCollection
                .Find(orders => true)
                .Project<Orders>(staticExclution)
                .ToListAsync();

            return orders;
        }
        public async Task<Orders> GetOrderById(string orderId)
        {
            return await _ordersCollection.Find(Builders<Orders>.Filter.Eq(x => x.Order_Id, orderId))
                .Project<Orders>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<Orders>> GetOrderByUser(string userId, int ordersPerPage = DefaultOrderPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = ordersPerPage * page;
            var limit = ordersPerPage;

            var orders = await _ordersCollection
                .Find(Builders<Orders>.Filter.Eq(order => order.User.userId, userId ))
                .Project<Orders>(staticExclution)
                .ToListAsync();

            return orders;
        }
        public async Task<IReadOnlyList<Orders>> GetOrderByDate(DateTime date, int ordersPerPage = DefaultOrderPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = ordersPerPage * page;
            var limit = ordersPerPage;

            var orders = await _ordersCollection
                .Find(Builders<Orders>.Filter.Eq(order => order.OrderDate.Date, date.Date))
                .Project<Orders>(staticExclution)
                .ToListAsync();

            return orders;
        }
        public async Task<Orders> GetOrderByTransId(string transId)
        {
            var result = await _ordersCollection
                .Find(Builders<Orders>.Filter.Eq(order => order.Transaction_Id, transId))
                .Project<Orders>(Builders<Orders>.Projection.Include("transaction_id")).FirstOrDefaultAsync();

            return result;
        }
        public async Task<OrdersResponse> AddOrderAsync(decimal totalCost, decimal paymentAmount, string paymentMethod, decimal processingFee = 0.30M,
            Products[] productList = null,User user = null, Address shippingAddress = null, Address billingAddress = null)
        {
            string randId = Helpers.MakeRandomID();
            string tranId = Helpers.MakeRandomID();
            if (CheckOrderDb(randId, tranId))
            {
                await AddOrderAsync(totalCost, paymentAmount, paymentMethod, processingFee,
                    productList, user, shippingAddress, billingAddress);
            }
            try
            {
                var newOrder = new Orders()
                {
                    Order_Id = randId,
                    User = user,
                    Shipping_Address = shippingAddress,
                    Billing_Address = billingAddress,
                    OrderDate = DateTime.Now,
                    Products = productList,
                    Processing_Fee = processingFee,
                    Total_Cost = totalCost,
                    Payment_Amount = paymentAmount,
                    Payment_Method = paymentMethod,
                    Transaction_Id = tranId
                };
                var createdOrder = await GetOrderById(newOrder.Order_Id);
                await _ordersCollection.InsertOneAsync(newOrder);
                return new OrdersResponse(true, "Order Created", createdOrder);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new OrdersResponse(false, "A Order with this ID already exists.")
                    : new OrdersResponse(false, ex.Message);
            }
        }
        public async Task<OrdersResponse> DeleteOrder(string orderId)
        {
            try
            {
                var deletedOrder = await _ordersCollection.FindOneAndDeleteAsync(Builders<Orders>
                    .Filter.Eq(x => x.Order_Id, orderId));
                if (!_ordersCollection.AsQueryable<Orders>().Any(exists => exists.Order_Id == deletedOrder.Order_Id))
                    return new OrdersResponse(true, "Order Deleted", deletedOrder);
                return new OrdersResponse(false, "Order Deletion was unsuccessful", deletedOrder);
            }
            catch (Exception ex)
            {
                return new OrdersResponse(false, ex.ToString());
            }
        }
        public async Task<OrdersResponse> UpdateOrderCompleted(string orderId, DateTime updatedDate)
        {
            try
            {
                var updatedOrder = await _ordersCollection.FindOneAndUpdateAsync(
                    Builders<Orders>.Filter.Where(rec => rec.Order_Id == orderId),
                    Builders<Orders>.Update
                    .Set(rec => rec.CompletedDate, updatedDate),
                    options: new FindOneAndUpdateOptions<Orders>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new OrdersResponse(true, "Order Updated", updatedOrder);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new OrdersResponse(false, "Order could not be updated.")
                    : new OrdersResponse(false, ex.Message);
            }
        }
        private bool CheckOrderDb(string orderId, string transId)
        {
            if (orderId != null)
            {
                if (_ordersCollection.AsQueryable<Orders>().Any(exists => exists.Order_Id == orderId)) { return true; }
            }
            if (transId != null)
            {
                if (_ordersCollection.AsQueryable<Orders>().Any(exists => exists.Transaction_Id == transId)) { return true; }
            }

            return false;
        }
    }
}
