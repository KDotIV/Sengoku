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
    public class PlotRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<Plot> _plotCollection;

        private const int DefaultPlotsPerPage = 20;
        private const int DefaultSortOrder = -1;
        private readonly ProjectionDefinition<Plot> staticExclution = Builders<Plot>.Projection.Exclude("_id");

        public PlotRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _ctx = context;
            _plotCollection = _ctx.GetCollection<Plot>("Plot");
        }

        public async Task<IReadOnlyList<Plot>> GetAllPlots(int plotsPerPage = DefaultPlotsPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var plots = await _plotCollection
                .Find(plots => true)
                .Project<Plot>(staticExclution)
                .ToListAsync();

            return plots;
        }
        public async Task<Plot> GetPlotById(string plotId)
        {
            return await _plotCollection.Find(Builders<Plot>.Filter.Eq(rec => rec.plotId, plotId))
                .Project<Plot>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<PlotResponse> AddPlot(string text, string image = null, string clipRef = null)
        {
            string randId = Helpers.MakeRandomID();
            if(CheckPlotDb(randId))
            {
                await AddPlot(text, image, clipRef);
            }
            try
            {
                var newPlot = new Plot()
                {
                    plotId = randId,
                    text = text,
                    image = image,
                    clipRef = clipRef
                };
                await _plotCollection.InsertOneAsync(newPlot);
                return new PlotResponse(true, "Plot created", newPlot);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new PlotResponse(false, "a Plot with this ID already exists.")
                    : new PlotResponse(false, ex.Message);
            }
        }
        public async Task<PlotResponse> DeletePlot(string plotId)
        {
            try
            {
                var deletedPlot = await _plotCollection.FindOneAndDeleteAsync(Builders<Plot>
                    .Filter.Eq(result => result.plotId, plotId));
                if (!_plotCollection.AsQueryable<Plot>().Any(exists => exists.plotId == deletedPlot.plotId))
                    return new PlotResponse(true, "Plot Deleted", deletedPlot);
                return new PlotResponse(false, "Plot deletion was unsuccessful", deletedPlot);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new PlotResponse(false, "a Plot with this ID already exists.")
                    : new PlotResponse(false, ex.Message);
            }
        }
        public async Task<PlotResponse> UpdatePlotText(string plotId, string updatedText)
        {
            try
            {
                var updatedPlot = await _plotCollection.FindOneAndUpdateAsync(
                    Builders<Plot>.Filter.Where(rec => rec.plotId == plotId),
                    Builders<Plot>.Update
                    .Set(rec => rec.text, updatedText),
                    options: new FindOneAndUpdateOptions<Plot>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new PlotResponse(true, "Plot Updated", updatedPlot);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new PlotResponse(false, "a Plot with this ID already exists.")
                    : new PlotResponse(false, ex.Message);
            }
        }
        private bool CheckPlotDb(string plotId)
        {
            if (plotId != null)
            {
                if (_plotCollection.AsQueryable<Plot>().Any(exists => exists.plotId == plotId)) { return true; }
            }
            else return false;
            return false;
        }
    }
}
