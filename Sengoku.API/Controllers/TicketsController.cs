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
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : Controller
    {
        private readonly TicketsRepository _ticketsRepository;
        public TicketsController(TicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        [HttpGet]
        [Route("GetTickets")]
        public async Task<ActionResult> GetTicketAllTicketsAsync(int limit = 20, int page = 0,
            int sort = -1)
        {
            var tickets = await _ticketsRepository.GetAllTickets(limit, page, sort);
            return Ok(tickets);
        }
        [HttpGet]
        [Route("GetTicketById/{ticketId}")]
        public async Task<ActionResult> GetTicketByIdAsync(string ticketId)
        {
            var result = await _ticketsRepository.GetTicketById(ticketId);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetTicketsByUser/{userId}")]
        public async Task<ActionResult> GetTicketsByUserAsync(string userId)
        {
            var tickets = await _ticketsRepository.GetTicketsByUser(userId);
            return Ok(tickets);
        }
        [HttpGet]
        [Route("GetTicketsByDate/{date}")]
        public async Task<ActionResult> GetTicketsByDateAsync(DateTime date)
        {
            var tickets = await _ticketsRepository.GetTicketByDate(date);
            return Ok(tickets);
        }
        [HttpPost]
        [Route("CreateTicket")]
        public async Task<ActionResult> AddTicketAsync([FromBody] Tickets ticket)
        {
            var response = await _ticketsRepository.AddTicket(ticket.Pass_Type, ticket.Event, ticket.User_Id,
                ticket.Payment_Date, ticket.Venue_Fee, ticket.Processing_Fee, ticket.Total_Cost, ticket.Payment_Amount, ticket.Payment_Method);
            if(!response.Success) { return BadRequest(new { error = response.ErrorMessage });  }
            return Ok(response);
        }
        [HttpDelete]
        [Route("DeleteOrder/{confirmId}")]
        public async Task<ActionResult> DeleteTicketAsync(string confirmId)
        {
            var result = await _ticketsRepository.DeleteTicket(confirmId);
            return Ok(result);
        }
        [HttpPut("UpdateTicketEvent/{confirmId}")]
        public async Task<ActionResult> UpdateTicketEventAsync([FromBody] Tickets eventRequest, string confirmId)
        {
            var eventResult = await _ticketsRepository.UpdateTicketByEvent(confirmId, eventRequest.Event);

            return Ok(eventResult);
        }
        [HttpPut("UpdateTicketPass/{confirmId}")]
        public async Task<ActionResult> UpdateTicketPassAsync([FromBody] Tickets passRequest, string confirmId)
        {
            var ticketToUpdate = await _ticketsRepository.GetTicketById(confirmId);
            var passResult = await _ticketsRepository.UpdateTicketByPassType(ticketToUpdate.Confirmation_Id, passRequest.Pass_Type);

            return Ok(passResult);
        }
    }
}
