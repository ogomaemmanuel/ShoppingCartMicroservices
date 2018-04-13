using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    //[Authorize]
    public class OrdersController : Controller
    {

        private IRepository<Order> _ordersManager;

        public OrdersController(IRepository<Order> ordersManager)
        {
            _ordersManager = ordersManager;
        }
        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
        public IActionResult Get()
        {
            var orders = this._ordersManager.GetAll();
            return new OkObjectResult(orders);
        }
        /// <summary>
        /// Gets a single order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(Guid id)
        {
            var orders = this._ordersManager.GetById(id);
            return new OkObjectResult(orders);
        }
        /// <summary>
        /// Gets orders by a customer, customer id is supplied
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("customer/{id}", Name = "GetCustomerOrders")]
        public IActionResult GetCustomerOrders([FromRoute]String id)
        {
            var result = ((OrdersManager)this._ordersManager).getCustomerOrders(id);
            return new OkObjectResult(result);
        }
        /// <summary>
        /// Gets order items for a particular order, order id is supplied
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("order-items/{id}", Name = "GetOrderItems")]
        public IActionResult GetOrderItems([FromRoute]Guid id)
        {
            var result = ((OrdersManager)this._ordersManager).getOrdersItems(id);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Saves Customer new Order
        /// </summary>
        /// <param name="customerOrderModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]CustomerOrder customerOrderModel)
        {
            var result = ((OrdersManager)this._ordersManager).CreateNewOrder(customerOrderModel, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (result)
            {
                return new OkObjectResult("your order has been recived");
            }
            return StatusCode(500, "could not create order");

        }

        /// <summary>
        /// Deletes an Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = this._ordersManager.Remove(id);
            if (result)
            {
                return new OkResult();
            }
            return StatusCode(500, "could not remove order");
        }
        /// <summary>
        /// Updating an order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Order order)
        {
            return null;
        }

        // DELETE: api/ApiWithActions/5
    }
}