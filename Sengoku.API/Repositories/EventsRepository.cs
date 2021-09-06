using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Sengoku.API.Models;

namespace Sengoku.API.Repositories
{
    public class EventsRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<Events> _eventsCollection;

        private const int DefaultEventsPerPage = 20;
        private const int DefaultSortOrder = -1;

        public EventsRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _ctx = context;
            _eventsCollection = _ctx.GetCollection<Events>("Events");
        }

        public async Task<IReadOnlyList<Events>> GetEvents(int eventsPerPage = DefaultEventsPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = eventsPerPage * page;
            var limit = eventsPerPage;

            var events = await _eventsCollection
                .Find(Builders<Events>.Filter.Empty)
                .Limit(limit)
                .Skip(skip)
                .ToListAsync();

            return events;
        }
        public async Task<Events> GetEventById(string eventId)
        {
            return await _eventsCollection.Find(Builders<Events>.Filter.Eq(x => x.Event_Id, eventId)).FirstOrDefaultAsync();
        }
        public async Task<Events> GetEventByName(string eventname)
        {
            return await _eventsCollection.Find(Builders<Events>.Filter.Eq(x => x.Name, eventname)).FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<Events>> GetEventsbyGame(string gameEvent, int eventsPerPage = DefaultEventsPerPage, 
            int page = 0, int sort = DefaultSortOrder)
        {
            var skip = eventsPerPage * page;
            var limit = eventsPerPage;

            var events = await _eventsCollection
                .Find(Builders<Events>.Filter.Eq(x => x.Game, gameEvent))
                .Limit(limit)
                .Skip(skip)
                .ToListAsync();

            return events;
        }
        public bool CheckEventsDb(string eventId)
        {
            if (eventId != null)
            {
                if (_eventsCollection.AsQueryable<Events>().Any(exists => exists.Event_Id == eventId)) return true;
            }
            else return false;
            return false;
        }
    }
}
