using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCategoryService.Models;
using ProductCategoryService.Services;

namespace ProductCategoryService.Controllers
{
    [Produces("application/json")]
    [Route("api/ProductsCategory")]
   // [Authorize]
    public class ProductsCategoryController : Controller
    {
        private IRepository<ProductCategory> _productCategoryManager;
        public ProductsCategoryController(IRepository<ProductCategory> productCategoryManager)
        {
            _productCategoryManager = productCategoryManager;

        }
        // GET: api/ProductsCategory
        [HttpGet]
        public IActionResult Get()
        {
            return null;
        }

        // GET: api/ProductsCategory/5
        [HttpGet("{id}", Name = "GetProductsCategory")]
        public IActionResult Get(int id)
        {
            return null;
        }

        // POST: api/ProductsCategory
        [HttpPost]
        public IActionResult Post([FromBody]ProductCategory productCategory)
        {
            return null;
        }

        // PUT: api/ProductsCategory/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ProductCategory productCategory)
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