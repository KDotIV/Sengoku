using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace Sengoku.API.Repositories
{
    public class PlayerRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<PlayerCards> _playerCollections;
        private readonly IMongoCollection<Events> _eventsCollection;

        private const int DefaultPlayersPerPage = 20;
        private const int DefaultSortOrder = -1;
        private readonly ProjectionDefinition<PlayerCards> staticExclution = Builders<PlayerCards>.Projection.Exclude("_id");

        public PlayerRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _ctx = context;
            _playerCollections = _ctx.GetCollection<PlayerCards>("PlayerCards");
            _eventsCollection = _ctx.GetCollection<Events>("Events");
        }

        public async Task<List<PlayerCards>> GetAllPlayers(int playersPerPage = DefaultPlayersPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = playersPerPage * page;
            var limit = playersPerPage;

            var players = await _playerCollections
                .Find(players => true)
                .Project<PlayerCards>(staticExclution)
                .ToListAsync();

            return players;
        }
        public async Task<PlayerCards> GetPlayerById(string playerId)
        {
            return await _playerCollections.Find(Builders<PlayerCards>.Filter.Eq(x => x.playerId, playerId))
                .Project<PlayerCards>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<PlayerCards> GetPlayerByName(string name)
        {
            return await _playerCollections.Find(Builders<PlayerCards>.Filter.Eq(x => x.Name, name))
                .Project<PlayerCards>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<List<PlayerCards>> GetPlayerByEvent(string playerId)
        {
            var filter = Builders<PlayerCards>.Filter.Eq(result => result.playerId, playerId);
            var project = Builders<PlayerCards>.Projection.Include(result => result.events).Exclude("_id");
            var players = await _playerCollections.Find(filter).Project<PlayerCards>(project).ToListAsync();

            return players;
        }
        public async Task<PlayerCards> GetStatsByPlayer(string playerId)
        {
            var filter = Builders<PlayerCards>.Filter.Eq(result => result.playerId, playerId);
            var project = Builders<PlayerCards>.Projection.Include(result => result.stats).Exclude("_id");
            var stats = await _playerCollections.Find(filter).Project<PlayerCards>(project).FirstOrDefaultAsync();

            return stats;
        }
        public async Task<PlayerCardResponse> AddPlayerCard(string name, string image, string[] social = null, Stats stats = null, Events[] events = null)
        {
            string randId = Helpers.MakeRandomID();
            if(CheckPlayerDb(randId))
            {
                await AddPlayerCard(name, image, social, stats, events);
            }
            try
            {
                var newPlayer = new PlayerCards()
                {
                    playerId = randId,
                    Name = name,
                    events = events,
                    image = image,
                    stats = stats,
                    social = social
                };
                await _playerCollections.InsertOneAsync(newPlayer);
                return new PlayerCardResponse(true, "Player Card Created", newPlayer);
            }
            catch(Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new PlayerCardResponse(false, "A Playercard with this ID already exists.")
                    : new PlayerCardResponse(false, ex.Message);
            }
        }
        public async Task<PlayerCardResponse> AddEventToPlayerCard(string playerId, string name = null, string date = null, string city = null, string game = null)
        {
            string randId = Helpers.MakeRandomID();
            if (CheckEventsDb(randId))
            {
                await AddEventToPlayerCard(playerId, name, date, city, game);
            }

            try
            {
                Events addedEvent = new Events()
                {
                    Name = name,
                    Date = date,
                    LastUpdated = DateTime.Now,
                    Event_Id = randId,
                    City = city,
                    Game = game
                };

                var updateArray = Builders<PlayerCards>.Update
                    .Push<Events>(player => player.events, addedEvent);

                var updatedPlayer = await _playerCollections.FindOneAndUpdateAsync(
                    Builders<PlayerCards>.Filter.Where(rec => rec.playerId == playerId),
                    updateArray, options: new FindOneAndUpdateOptions<PlayerCards>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new PlayerCardResponse(true, "Event added to Player Card", updatedPlayer);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E11 duplicate key error")
                    ? new PlayerCardResponse(false, "Player could not be updated.")
                    : new PlayerCardResponse(false, ex.Message);
            }
        }
        public async Task<PlayerCardResponse> DeletePlayerCard(string playerId)
        {
            try
            {
                var deletedPlayer = await _playerCollections.FindOneAndDeleteAsync(Builders<PlayerCards>
                    .Filter.Eq(result => result.playerId, playerId));
                if (!_playerCollections.AsQueryable<PlayerCards>().Any(exists => exists.playerId == deletedPlayer.playerId))
                    return new PlayerCardResponse(true, "Player Deleted", deletedPlayer);
                return new PlayerCardResponse(false, "Player deletion was unsuccessful", deletedPlayer);
            }
            catch(Exception ex)
            {
                return new PlayerCardResponse(false, ex.ToString());
            }
        }
        public async Task<PlayerCardResponse> UpdatePlayerCardByName(string playerId, string nameUpdate)
        {
            try
            {
                var updatedPlayer = await _playerCollections.FindOneAndUpdateAsync(
                    Builders<PlayerCards>.Filter.Where(rec => rec.playerId == playerId),
                    Builders<PlayerCards>.Update
                    .Set(rec => rec.Name, nameUpdate),
                    options: new FindOneAndUpdateOptions<PlayerCards>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new PlayerCardResponse(true, "Player Updated", updatedPlayer);
            }
            catch(Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E11 duplicate key error")
                    ? new PlayerCardResponse(false, "Player could not be updated.")
                    : new PlayerCardResponse(false, ex.Message);
            }
        }
        public async Task<PlayerCardResponse> UpdatePlayerCardByEventName(string playerId, string eventId, string updatedName)
        {
            try
            {
                var updatedPlayer = await _playerCollections.FindOneAndUpdateAsync(
                    Builders<PlayerCards>.Filter.Where(rec => rec.playerId == playerId && rec.events.Any(i => i.Event_Id == eventId)),
                    Builders<PlayerCards>.Update
                    .Set(rec => rec.events[-1].Name, updatedName),
                    options: new FindOneAndUpdateOptions<PlayerCards>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new PlayerCardResponse(true, "Player Updated", updatedPlayer);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E11 duplicate key error")
                    ? new PlayerCardResponse(false, "Player could not be updated.")
                    : new PlayerCardResponse(false, ex.Message);
            }
        }
        private bool CheckPlayerDb(string playerId)
        {
            if(playerId != null)
            {
                if(_playerCollections.AsQueryable<PlayerCards>().Any(exists => exists.playerId == playerId)) { return true;  }
            }
            else { return false;  }
            return false;
        }
        private bool CheckEventsDb(string eventsId)
        {
            if (eventsId != null)
            {
                if (_eventsCollection.AsQueryable<Events>().Any(exists => exists.Event_Id == eventsId)) return true;
            }
            else return false;
            return false;
        }
    }
}
