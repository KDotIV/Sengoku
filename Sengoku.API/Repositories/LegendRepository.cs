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
    public class LegendRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<Legend> _legendsCollection;
        private readonly IMongoCollection<Plot> _plotsCollection;

        private const int DefaultLegendsPerPage = 20;
        private const int DefaultSortOrder = -1;
        private readonly ProjectionDefinition<Legend> staticExclution = Builders<Legend>.Projection.Exclude("_id");

        public LegendRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _ctx = context;
            _legendsCollection = _ctx.GetCollection<Legend>("Legends");
            _plotsCollection = _ctx.GetCollection<Plot>("Plot");
        }

        public async Task<IReadOnlyList<Legend>> GetAllLegend(int legendsPerPage = DefaultLegendsPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = legendsPerPage * page;
            var limit = legendsPerPage;

            var legends = await _legendsCollection
                .Find(legends => true)
                .Project<Legend>(staticExclution)
                .ToListAsync();

            return legends;
        }
        public async Task<Legend> GetLegendById(string legendId)
        {
            return await _legendsCollection.Find(Builders<Legend>.Filter.Eq(rec => rec.legendId, legendId))
                .Project<Legend>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<Legend>> GetLegendBySubject(string subject)
        {
            return await _legendsCollection.Find(Builders<Legend>.Filter.Eq(rec => rec.subject, subject))
                .ToListAsync();
        }
        public async Task<IReadOnlyList<Legend>> GetLegendByGame(string game)
        {
            return await _legendsCollection.Find(Builders<Legend>.Filter.Eq(rec => rec.game, game))
                .ToListAsync();
        }
        public async Task<IReadOnlyList<Legend>> GetPlotsByLegend(string legendId)
        {
            var filter = Builders<Legend>.Filter.Eq(result => result.legendId, legendId);
            var project = Builders<Legend>.Projection.Include(result => result.plotPoints).Exclude("_id");
            var plots = await _legendsCollection.Find(filter).Project<Legend>(project).ToListAsync();

            return plots;
        }
        public async Task<LegendResponse> AddLegend(string subject, string summary, string game, Plot[] plotPoints = null)
        {
            string randId = Helpers.MakeRandomID();
            if(CheckLegendDb(randId))
            {
                await AddLegend(subject, summary, game, plotPoints);
            }
            try
            {
                var newLegend = new Legend()
                {
                    legendId = randId,
                    subject = subject,
                    summary = summary,
                    game = game,
                    plotPoints = plotPoints
                };
                await _legendsCollection.InsertOneAsync(newLegend);
                return new LegendResponse(true, "Legend Created", newLegend);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new LegendResponse(false, "A Playercard with this ID already exists.")
                    : new LegendResponse(false, ex.Message);
            }
        }
        public async Task<LegendResponse> AddPlot(string legendId, string text, string image, string clipRef)
        {
            string randId = Helpers.MakeRandomID();
            if(CheckPlotDb(randId))
            {
                await AddPlot(legendId, text, image, clipRef);
            }
            try
            {
                Plot addedPlot = new Plot()
                {
                    plotId = randId,
                    text = text,
                    image = image,
                    clipRef = clipRef
                };

                var updateArray = Builders<Legend>.Update
                    .Push<Plot>(legend => legend.plotPoints, addedPlot);

                var updatedLegend = await _legendsCollection.FindOneAndUpdateAsync(
                    Builders<Legend>.Filter.Where(rec => rec.legendId == legendId),
                    updateArray, options: new FindOneAndUpdateOptions<Legend>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new LegendResponse(true, "Plot added to Legend", updatedLegend);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new LegendResponse(false, "A Playercard with this ID already exists.")
                    : new LegendResponse(false, ex.Message);
            }
        }
        public async Task<LegendResponse> DeleteLegend(string legendId)
        {
            try
            {
                var deletedLegend = await _legendsCollection.FindOneAndDeleteAsync(Builders<Legend>
                    .Filter.Eq(result => result.legendId, legendId));
                if (!_legendsCollection.AsQueryable<Legend>().Any(exists => exists.legendId == deletedLegend.legendId))
                    return new LegendResponse(false, "Legend Deleted", deletedLegend);
                return new LegendResponse(false, "Legend deletion was unsuccessful", deletedLegend);
            }
            catch (Exception ex)
            {
                return new LegendResponse(false, ex.ToString());
            }
        }
        public async Task<LegendResponse> UpdateLegendBySubject(string legendId, string subjectUpdate)
        {
            try
            {
                var updatedLegend = await _legendsCollection.FindOneAndUpdateAsync(
                    Builders<Legend>.Filter.Where(rec => rec.legendId == legendId),
                    Builders<Legend>.Update
                    .Set(rec => rec.subject, subjectUpdate),
                    options: new FindOneAndUpdateOptions<Legend>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new LegendResponse(true, "Legend Updated", updatedLegend);
            }
            catch (Exception ex)
            {
                return new LegendResponse(false, ex.ToString());
            }
        }
        public async Task<LegendResponse> UpdateLegendBySummary(string legendId, string summaryUpdate)
        {
            try
            {
                var updatedLegend = await _legendsCollection.FindOneAndUpdateAsync(
                    Builders<Legend>.Filter.Where(rec => rec.legendId == legendId),
                    Builders<Legend>.Update
                    .Set(rec => rec.subject, summaryUpdate),
                    options: new FindOneAndUpdateOptions<Legend>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new LegendResponse(true, "Legend Updated", updatedLegend);
            }
            catch (Exception ex)
            {
                return new LegendResponse(false, ex.ToString());
            }
        }
        public async Task<LegendResponse> UpdateLegendByGame(string legendId, string gameUpdate)
        {
            try
            {
                var updatedLegend = await _legendsCollection.FindOneAndUpdateAsync(
                    Builders<Legend>.Filter.Where(rec => rec.legendId == legendId),
                    Builders<Legend>.Update
                    .Set(rec => rec.subject, gameUpdate),
                    options: new FindOneAndUpdateOptions<Legend>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new LegendResponse(true, "Legend Updated", updatedLegend);
            }
            catch (Exception ex)
            {
                return new LegendResponse(false, ex.ToString());
            }
        }
        public bool CheckLegendDb(string legendId)
        {
            if (legendId != null)
            {
                if (_legendsCollection.AsQueryable<Legend>().Any(exists => exists.legendId == legendId)) return true;
            }
            else return false;
            return false;
        }
        public bool CheckPlotDb(string plotId)
        {
            if (plotId != null)
            {
                if (_plotsCollection.AsQueryable<Plot>().Any(exists => exists.plotId == plotId)) return true;
            }
            else return false;
            return false;
        }
    }
}
