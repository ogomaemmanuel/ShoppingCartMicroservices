using ProductService.Models;
using ProductService.SystemIntegration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public class ProductsManager : IRepository<Product>
    {
        private readonly ShoppingCartDbContext _dbContext;
        private readonly IProductUpdatedEventPublisher _productUpdatedEventPublisher;
        public ProductsManager(ShoppingCartDbContext dbContext,
            IProductUpdatedEventPublisher productUpdatedEventPublisher)
        {
            _productUpdatedEventPublisher = productUpdatedEventPublisher;
            _dbContext = dbContext;
        }

        public IEnumerable<Product> GetAll()
        {
            return this._dbContext.Products.ToList();
        }

        public Product GetById(Guid id)
        {
            return this._dbContext.Products.Find(id);
        }

        public bool Remove(Guid id)
        {
            var product = this.GetById(id);
            this._dbContext.Products.Remove(product);
            try
            {
                this._dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Add(Product product)
        {
            this._dbContext.Add(product);
            try
            {
                _dbContext.SaveChanges();
                _productUpdatedEventPublisher.Publish(product);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public PagedResult<Product> GetPaged(PagingParams pagingParams)
        {
            PagedResult<Product> pagedProducts=
                this._dbContext.Products.GetPaged<Product>(pagingParams.PageNumber, pagingParams.PageSize);
           
            return pagedProducts;
        }

        public bool Update(Product product)
        {
           // PagedResult <Product> pagedProducts
              return false;
        }
    }
}
