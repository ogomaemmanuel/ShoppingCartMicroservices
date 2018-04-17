using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BasketService.Models;
using BasketService.Services;
using System.Security.Claims;

namespace BasketService.Controllers
{
    [Produces("application/json")]
    [Route("api/Basket")]
    public class BasketController : Controller
    {
        private IRepository<BasketItem> _basketManager;
        public BasketController(IRepository<BasketItem> basketManager) {
            _basketManager = basketManager;

        } 
        [HttpPost]       
        public IActionResult AddBasketIetm([FromBody]BasketItem customerbasketItem) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            this._basketManager.Add(customerbasketItem, userId);
            return new OkResult();
        }
        [HttpGet]
        [Route("{customerId?}")]
        public IActionResult GetCustomerBasketItems([FromRoute]String customerId) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value?? customerId;
            var customerBasketItems = _basketManager.GetByCustomerId(userId);
            return new OkObjectResult(customerBasketItems);
        }
    }
}