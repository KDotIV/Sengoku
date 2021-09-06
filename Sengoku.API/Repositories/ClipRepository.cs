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
    public class ClipRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<Clip> _clipCollection;

        private const int DefaultClipsPerPage = 20;
        private const int DefaultSortOrder = -1;
        private readonly ProjectionDefinition<Clip> staticExclution = Builders<Clip>.Projection.Exclude("_id");

        public ClipRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _ctx = context;
            _clipCollection = _ctx.GetCollection<Clip>("Clip");
        }
        public async Task<IReadOnlyList<Clip>> GetAllClips(int clipsPerPage = DefaultClipsPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = clipsPerPage * page;
            var limit = clipsPerPage;

            var clips = await _clipCollection
                .Find(clips => true)
                .Project<Clip>(staticExclution)
                .ToListAsync();

            return clips;
        }
        public async Task<Clip> GetClipById(string clipId)
        {
            return await _clipCollection.Find(Builders<Clip>.Filter.Eq(rec => rec.clipId, clipId))
                .Project<Clip>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<Clip> GetClipByName(string clipName)
        {
            return await _clipCollection.Find(Builders<Clip>.Filter.Eq(rec => rec.name, clipName))
                .Project<Clip>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<Clip>> GetClipByGame(string gameName, int clipsPerPage = DefaultClipsPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = clipsPerPage * page;
            var limit = clipsPerPage;

            var clips = await _clipCollection
                .Find(Builders<Clip>.Filter.Eq(rec => rec.game, gameName))
                .Limit(limit)
                .Skip(skip)
                .ToListAsync();

            return clips;
        }
        public async Task<ClipResponse> AddClip(string url, string name, string game)
        {
            string randId = Helpers.MakeRandomID();
            if(CheckClipDb(randId))
            {
                await AddClip(url, name, game);
            }
            try
            {
                Clip newClip = new Clip()
                {
                    clipId = randId,
                    url = url,
                    name = name,
                    game = game
                };
                await _clipCollection.InsertOneAsync(newClip);
                return new ClipResponse(true, "Clip created", newClip);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E11 duplicate key error")
                    ? new ClipResponse(false, "Clip could not be created.")
                    : new ClipResponse(false, ex.Message);
            }
        }
        public async Task<ClipResponse> DeleteClip(string clipId)
        {
            try
            {
                var deletedClip = await _clipCollection.FindOneAndDeleteAsync(Builders<Clip>
                    .Filter.Eq(result => result.clipId, clipId));
                if (!_clipCollection.AsQueryable<Clip>().Any(exists => exists.clipId == deletedClip.clipId))
                    return new ClipResponse(true, "Clip Deleted", deletedClip);
                return new ClipResponse(false, "Clip deletion was unsuccessful", deletedClip);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E11 duplicate key error")
                    ? new ClipResponse(false, "Clip could not be created.")
                    : new ClipResponse(false, ex.Message);
            }
        }
        public async Task<ClipResponse> UpdateClipUrl(string clipId, string updatedURL)
        {
            try
            {
                var updatedClip = await _clipCollection.FindOneAndUpdateAsync(
                    Builders<Clip>.Filter.Where(rec => rec.clipId == clipId),
                    Builders<Clip>.Update
                    .Set(rec => rec.url, updatedURL),
                    options: new FindOneAndUpdateOptions<Clip>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new ClipResponse(true, "Clip Updated", updatedClip);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E11 duplicate key error")
                    ? new ClipResponse(false, "Clip could not be created.")
                    : new ClipResponse(false, ex.Message);
            }
        }
        private bool CheckClipDb(string clipId)
        {
            if (clipId != null)
            {
                if (_clipCollection.AsQueryable<Clip>().Any(exists => exists.clipId == clipId)) return true;
            }
            else return false;
            return false;
        }
    }
}
