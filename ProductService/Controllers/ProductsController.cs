using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Produces("application/json")]
    [Route("api/products")]
    //[Authorize]
    public class ProductsController : Controller
    {


        private IRepository<Product> _productsManager;
        public ProductsController(IRepository<Product> productsManager)
        {
            _productsManager = productsManager;//  productsManager;
        }

        [Route("all")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public IActionResult GetAllProduct()
        {
            var products = this._productsManager.GetAll();
            return new OkObjectResult(products);
        }

        [Route("paged")]
        [HttpGet(Name ="GetPaged")]
        [ProducesResponseType(typeof(PagedResult<Product>), 200)]
        public IActionResult GetAllProductPaged([FromQuery]PagingParams pagingParams)
        {
            PagedResult<Product> pagedProducts = this._productsManager.GetPaged(pagingParams);
            return new OkObjectResult(pagedProducts);
        }




        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), 200)]
        public IActionResult GetProductById(Guid id)
        {
            var product = this._productsManager.GetById(id);
            if (product != null)
            {
                return new OkObjectResult(product);
            };
            return NotFound();
        }
        [HttpPost]
        public IActionResult CreateProduct([FromBody]Product product)
        {
            try
            {
                var result = this._productsManager.Add(product);

                return new OkResult();

            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException);
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            var result = this._productsManager.Remove(id);
            if (result)
            {
                return new NoContentResult();
            }
            else return NotFound();
        }

    }
}