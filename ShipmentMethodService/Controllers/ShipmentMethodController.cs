using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipmentMethodService.Models;
using ShipmentMethodService.Services;

namespace ShipmentMethodService.Controllers
{
   // [Authorize]
    [Produces("application/json")]
    [Route("api/ShipmentMethods")]
    public class ShipmentMethodsController : Controller
    {
        private IRepository<ShipmentMethod> _shipmentMethodManager;
        public ShipmentMethodsController(IRepository<ShipmentMethod> shipmentMethodManager)
        {
            this._shipmentMethodManager = shipmentMethodManager;
        }
        // GET: api/ShipmentMethod
        [HttpGet]       
        [ProducesResponseType(typeof(ShipmentMethod), 200)]
        public IActionResult Get()
        {
            var shipmentMethods = this._shipmentMethodManager.GetAll();
            return new OkObjectResult(shipmentMethods);
        }

        // GET: api/ShipmentMethod/5
        [HttpGet("{id}", Name = "GetShipmentMethod")]
        public IActionResult Get(int id)
        {
            return null;
        }

        // POST: api/ShipmentMethod
        [HttpPost]
        public IActionResult Post([FromBody]ShipmentMethod shipmentMethod)
        {
            if (ModelState.IsValid)
            {
                var result = this._shipmentMethodManager.Add(shipmentMethod);
                if (result)
                {
                    return StatusCode(201);
                }
                return StatusCode(500, "Object could not be created");
            }
            return BadRequest(this.ModelState);

        }

        // PUT: api/ShipmentMethod/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ShipmentMethod value)
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