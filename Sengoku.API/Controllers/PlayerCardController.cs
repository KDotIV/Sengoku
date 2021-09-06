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
    [Route("api/playercards")]
    [ApiController]
    public class PlayerCardController : Controller
    {
        private readonly PlayerRepository _playerRepository;
        public PlayerCardController(PlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpGet]
        [Route("GetAllPlayers")]
        public async Task<ActionResult> GetAllPlayerCardsAsync(int limit = 20, int page = 0,
            int sort = -1)
        {
            var players = await _playerRepository.GetAllPlayers(limit, page, sort);
            return Ok(players);
        }
        [HttpGet]
        [Route("GetPlayer/id/{playerId}")]
        public async Task<ActionResult> GetPlayerByIDAsync(string playerId)
        {
            var result = await _playerRepository.GetPlayerById(playerId);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetPlayer/name/{playerName}")]
        public async Task<ActionResult> GetPlayerByNameAsync(string playerName)
        {
            var result = await _playerRepository.GetPlayerByName(playerName);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetPlayer/events/{playerId}")]
        public async Task<ActionResult> GetPlayersEventsAsync(string playerId)
        {
            var events = await _playerRepository.GetPlayersEvents(playerId);
            return Ok(events);
        }
        [HttpGet]
        [Route("GetPlayer/stats/{playerId}")]
        public async Task<ActionResult> GetPlayerStatsAsync(string playerId)
        {
            var stats = await _playerRepository.GetStatsByPlayer(playerId);
            return Ok(stats);
        }
        [HttpPost]
        [Route("CreatePlayer")]
        public async Task<ActionResult> AddPlayerCardAsync([FromBody] PlayerCards playerCard)
        {
            var response = await _playerRepository.AddPlayerCard(playerCard.Name, playerCard.image,
                playerCard.social, playerCard.stats, playerCard.events);
            if(!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
        [HttpPut]
        [Route("AddEvent/{playerId}")]
        public async Task<ActionResult> AddEventToPLayerAsync(string playerId, [FromBody] Events events)
        {
            var response = await _playerRepository.AddEventToPlayerCard(playerId, events.Name, events.Date, events.City, events.Game);
            if(!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
        [HttpDelete]
        [Route("DeletePlayerCard/{playerId}")]
        public async Task<ActionResult> DeletePlayerCard(string playerId)
        {
            var result = await _playerRepository.DeletePlayerCard(playerId);
            return Ok(result);
        }
        [HttpPut]
        [Route("UpdatePlayerCard/name/{playerId}")]
        public async Task<ActionResult> UpdatePlayerCardNameAsync(string playerId, [FromBody] PlayerCards updatedPlayerCard)
        {
            var response = await _playerRepository.UpdatePlayerCardByName(playerId, updatedPlayerCard.Name);
            if(!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
        [HttpPut]
        [Route("UpdatePlayerCard/event_name/{playerId}")]
        public async Task<ActionResult> UpdatePlayerCardEventName(string playerId, [FromBody] Events updatedEvent)
        {
            var response = await _playerRepository.UpdatePlayerCardByEventName(playerId, updatedEvent.Event_Id, updatedEvent.Name);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
    }
}
