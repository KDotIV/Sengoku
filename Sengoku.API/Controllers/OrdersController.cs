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
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly OrdersRepository _ordersRepository;

        public OrdersController(OrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        [HttpGet]
        [Route("GetAllOrders")]
        public async Task<ActionResult> GetAllOrdersAsync(int limit = 20, int page = 0,
            int sort = -1)
        {
            var orders = await _ordersRepository.GetAllOrders(limit, page, sort);
            return Ok(orders);
        }
        [HttpGet]
        [Route("GetOrderId/{orderId}", Name = "GetOrderId")]
        public async Task<ActionResult> GetOrderByIdAsync(string orderId)
        {
            var result = await _ordersRepository.GetOrderById(orderId);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetOrdersUser/{userId}", Name = "GetOrdersUser")]
        public async Task<ActionResult> GetOrdersByUserAsync(string userId, int limit = 20, int page = 0,
            int sort = -1)
        {
            var orders = await _ordersRepository.GetOrderByUser(userId, limit, page, sort);
            if (orders == null) return BadRequest(new ErrorResponse("No Orders under that ID"));
            return Ok(orders);
        }
        [HttpGet]
        [Route("GetOrdersDate/{date}")]
        public async Task<ActionResult> GetOrdersByDateAsync(DateTime date, int limit = 20, int page = 0,
            int sort = -1)
        {
            var orders = await _ordersRepository.GetOrderByDate(date, limit, page, sort);
            if (orders == null) return BadRequest(new ErrorResponse("No Orders under that Date"));
            return Ok(orders);
        }
        [HttpGet]
        [Route("GetOrderTran/{tranId}", Name = "GetOrderTran")]
        public async Task<ActionResult> GetOrderByTransAsync(string tranId)
        {
            var result = await _ordersRepository.GetOrderById(tranId);
            return Ok(result);
        }
        [HttpDelete("DeleteOrder/{orderId}")]
        public async Task<ActionResult> DeleteOrderAsync(string orderId)
        {
            var result = await _ordersRepository.DeleteOrder(orderId);
            return Ok(result);
        }
        [HttpPut("UpdateCompletedDate/{orderId}")]
        public async Task<ActionResult> UpdateCompletedDate([FromBody] Orders dateRequest, string orderId)
        {
            var orderToUpdate = await _ordersRepository.GetOrderById(orderId);
            var dateResult = await _ordersRepository.UpdateOrderCompleted(orderToUpdate.Order_Id, dateRequest.CompletedDate);

            return Ok(dateResult);
        }
    }
}
