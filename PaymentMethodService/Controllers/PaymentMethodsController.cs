using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentMethodService.Models;
using PaymentMethodService.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PaymentMethodService.Controllers
{

    [Produces("application/json")]
    [Route("api/PaymentMethods")]
    //[Authorize]
    public class PaymentMethodsController : Controller
    {
        // GET: api/PaymentMethods
        private IRepository<PaymentMethod> _paymentMethodsManager;
        public PaymentMethodsController(IRepository<PaymentMethod> paymentMethodsManager)
        {
            _paymentMethodsManager = paymentMethodsManager;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentMethod>), 200)]
        public IActionResult Get()
        {
            var paymentMethods = this._paymentMethodsManager.GetAll();
            return new OkObjectResult(paymentMethods);
        }

        // GET: api/PaymentMethods/5
        [HttpGet("{id}", Name = "GetPaymentMethod")]
        public IActionResult Get(int id)
        {
            return null;
        }

        // POST: api/PaymentMethods
        [HttpPost]
        public IActionResult Post([FromBody]PaymentMethod paymentMethod)
        {
            var result = this._paymentMethodsManager.Add(paymentMethod);

            if (result)
            {
                return new StatusCodeResult(201);
            }
            else return new StatusCodeResult(500);
        }

        // PUT: api/PaymentMethods/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return null;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return null;
        }
    }
}
