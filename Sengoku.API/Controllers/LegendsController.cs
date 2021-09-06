using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;
using Sengoku.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Controllers
{
    [Route("api/legends")]
    [ApiController]
    public class LegendsController : Controller
    {
        private readonly LegendRepository _legendRespository;
        public LegendsController(LegendRepository legendRepository)
        {
            _legendRespository = legendRepository;
        }

        [HttpGet]
        [Route("GetAllLegends")]
        public async Task<ActionResult> GetAllLegends(int limit = 20, int page = 0,
            int sort = -1)
        {
            var legends = await _legendRespository.GetAllLegend(limit, page, sort);
            return Ok(legends);
        }
        [HttpGet]
        [Route("GetLegend/id/{legendId}")]
        public async Task<ActionResult> GetLegendByIDAsync(string legendId)
        {
            var result = await _legendRespository.GetLegendById(legendId);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetLegend/subject/{subject}")]
        public async Task<ActionResult> GetLegendsBySubjectAsync(string subject)
        {
            var legends = await _legendRespository.GetLegendBySubject(subject);
            return Ok(legends);
        }
        [HttpGet]
        [Route("GetLegend/game/{gameName}")]
        public async Task<ActionResult> GetLegendsByGameAsync(string gameName)
        {
            var legends = await _legendRespository.GetLegendByGame(gameName);
            return Ok(legends);
        }
        [HttpGet]
        [Route("GetLegendPlot/{legendId}")]
        public async Task<ActionResult> GetPlotByLegendsAsync(string legendId)
        {
            var plot = await _legendRespository.GetPlotsByLegend(legendId);
            return Ok(plot);
        }
        [HttpPost]
        [Route("CreateLegend")]
        public async Task<ActionResult> CreateLegend([FromBody] Legend newLegend)
        {
            var response = await _legendRespository.AddLegend(newLegend.subject, newLegend.summary, newLegend.game,
                newLegend.plotPoints);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
        [HttpPut]
        [Route("AddPlot/{legendId}")]
        public async Task<ActionResult> CreatePlotAsync(string legendId, [FromBody] Plot newPlot)
        {
            var response = await _legendRespository.AddPlot(legendId, newPlot.text, newPlot.image, newPlot.clipRef);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
        [HttpDelete]
        [Route("DeleteLegend/{legendId}")]
        public async Task<ActionResult> DeleteLegendAsync(string legendId)
        {
            var response = await _legendRespository.DeleteLegend(legendId);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
        [HttpPut]
        [Route("UpdateLegend/subject/{legendId}")]
        public async Task<ActionResult> UpdateLegendSubjectAsync(string legendId, [FromBody] Legend updatedLegend)
        {
            var response = await _legendRespository.UpdateLegendBySubject(legendId, updatedLegend.subject);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
        [HttpPut]
        [Route("UpdateLegend/summary/{legendId}")]
        public async Task<ActionResult> UpdateLegendSummaryAsync(string legendId, [FromBody] Legend updatedLegend)
        {
            var response = await _legendRespository.UpdateLegendBySummary(legendId, updatedLegend.summary);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
        [HttpPut]
        [Route("UpdateLegend/game/{legendId}")]
        public async Task<ActionResult> UpdateLegendGameAsync(string legendId, [FromBody] Legend updatedLegend)
        {
            var response = await _legendRespository.UpdateLegendByGame(legendId, updatedLegend.game);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
    }
}
