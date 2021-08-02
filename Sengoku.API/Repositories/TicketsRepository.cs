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
    }
}
