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
    public class TicketsRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<Tickets> _ticketsCollection;
        private const int DefaultTicketsPerPage = 20;
        private const int DefaultSortOrder = -1;
        private readonly ProjectionDefinition<Tickets> staticExclution = Builders<Tickets>.Projection.Exclude("_id");

        public TicketsRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _ctx = context;
            _ticketsCollection = _ctx.GetCollection<Tickets>("Tickets");
        }

        public async Task<List<Tickets>> GetAllTickets(int ticketsPerPage = DefaultTicketsPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = ticketsPerPage * page;
            var limit = ticketsPerPage;

            var tickets = await _ticketsCollection
                .Find(orders => true)
                .Project<Tickets>(staticExclution)
                .ToListAsync();

            return tickets;
        }
        public async Task<Tickets> GetTicketById(string ticketId)
        {
            return await _ticketsCollection.Find(Builders<Tickets>.Filter.Eq(found => found.Confirmation_Id, ticketId))
                .Project<Tickets>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<Tickets>> GetTicketsByUser(string userId, int ticketsPerPage = DefaultTicketsPerPage,
            int page = 0, int sort = DefaultSortOrder)
        {
            var skip = ticketsPerPage * page;
            var limit = ticketsPerPage;

            var tickets = await _ticketsCollection
                .Find(Builders<Tickets>.Filter.Eq(found => found.User_Id, userId))
                .Project<Tickets>(staticExclution)
                .ToListAsync();

            return tickets;
        }
        public async Task<IReadOnlyList<Tickets>> GetTicketByDate(DateTime date, int ticketsPerPage = DefaultTicketsPerPage,
            int page = 0, int sort = DefaultSortOrder)
        {
            var skip = ticketsPerPage * page;
            var limit = ticketsPerPage;

            var tickets = await _ticketsCollection
                .Find(Builders<Tickets>.Filter.Eq(ticket => ticket.Payment_Date, date.Date))
                .Project<Tickets>(staticExclution)
                .ToListAsync();

            return tickets;
        }
        public async Task<TicketsResponse> AddTicket(string pass_type, Events events, string userId,
            DateTime paymentDate, decimal venueFee, decimal processingFee, decimal totalCost,
            decimal paymentAmount, string paymentMethod)
        {
            string randId = Helpers.MakeRandomID();
            string tranId = Helpers.MakeRandomID();
            if (CheckTicketDb(randId, tranId))
            {
                await AddTicket(pass_type, events, userId, paymentDate,
                    venueFee, processingFee, totalCost, paymentAmount, paymentMethod);
            }
            try
            {
                var newTicket = new Tickets()
                {
                    Confirmation_Id = randId,
                    Pass_Type = pass_type,
                    Event = events,
                    User_Id = userId,
                    Payment_Date = DateTime.Now,
                    Venue_Fee = venueFee,
                    Processing_Fee = processingFee,
                    Total_Cost = totalCost,
                    Payment_Amount = paymentAmount,
                    Payment_Method = paymentMethod,
                    Transaction_Id = tranId
                };
                await _ticketsCollection.InsertOneAsync(newTicket);
                return new TicketsResponse(true, "Order Created", newTicket);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new TicketsResponse(false, "A Order with this ID already exists.")
                    : new TicketsResponse(false, ex.Message);
            }
        }
        public async Task<TicketsResponse> DeleteTicket(string ticketId)
        {
            try
            {
                var deletedTicket = await _ticketsCollection.FindOneAndDeleteAsync(Builders<Tickets>
                    .Filter.Eq(x => x.Confirmation_Id, ticketId));
                if (!_ticketsCollection.AsQueryable<Tickets>().Any(exists => exists.Confirmation_Id == deletedTicket.Confirmation_Id))
                    return new TicketsResponse(true, "Ticket Deleted", deletedTicket);
                return new TicketsResponse(false, "Ticket failed to be deleted", deletedTicket);
            }
            catch (Exception ex)
            {
                return new TicketsResponse(false, ex.ToString());
            }
        }
        public async Task<TicketsResponse> UpdateTicketByEvent(string confirmId, Events eventToUpdate)
        {
            try
            {
                var updatedTicket = await _ticketsCollection.FindOneAndUpdateAsync(
                    Builders<Tickets>.Filter.Where(rec => rec.Confirmation_Id == confirmId),
                    Builders<Tickets>.Update
                    .Set(rec => rec.Event, eventToUpdate),
                    options: new FindOneAndUpdateOptions<Tickets>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new TicketsResponse(true, "Ticket Updated", updatedTicket);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new TicketsResponse(false, "Ticket could not be updated")
                    : new TicketsResponse(false, ex.Message);
            }
        }
        public async Task<TicketsResponse> UpdateTicketByPassType(string confirmId, string passType)
        {
            try
            {
                var updatedTicket = await _ticketsCollection.FindOneAndUpdateAsync(
                    Builders<Tickets>.Filter.Where(rec => rec.Confirmation_Id == confirmId),
                    Builders<Tickets>.Update
                    .Set(rec => rec.Pass_Type, passType),
                    options: new FindOneAndUpdateOptions<Tickets>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new TicketsResponse(true, "Ticket Updated", updatedTicket);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new TicketsResponse(false, "Ticket could not be updated")
                    : new TicketsResponse(false, ex.Message);
            }
        }
        public bool CheckTicketDb(string confirmationId, string transId)
        {
            if(confirmationId != null)
            {
                if(_ticketsCollection.AsQueryable<Tickets>().Any(exists => exists.Confirmation_Id == confirmationId)) { return true; }
            }
            if(transId != null)
            {
                if(_ticketsCollection.AsQueryable<Tickets>().Any(exists => exists.Transaction_Id == transId)) { return true; }
            }
            return false;
        }
    }
}
