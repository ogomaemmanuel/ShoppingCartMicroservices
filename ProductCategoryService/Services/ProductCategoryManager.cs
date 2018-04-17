using ProductCategoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCategoryService.Services
{
    public class ProductCategoryManager : IRepository<ProductCategory>
    {
        private readonly ShoppingCartDbContext _shoppingCartDbContext;
        public ProductCategoryManager(ShoppingCartDbContext shoppingCartDbContext)
        {
            _shoppingCartDbContext = shoppingCartDbContext;
        }

        public bool Add(ProductCategory t)
        {
            _shoppingCartDbContext.Add(t);
            _shoppingCartDbContext.SaveChanges();
            return true;
        }

        public IEnumerable<ProductCategory> GetAll()
        {
          var productCategories=  _shoppingCartDbContext.ProductCategories.ToList();
            return productCategories;
        }

        public ProductCategory GetById(Guid id)
        {
            var productCategory = _shoppingCartDbContext.ProductCategories.Find(id);
            return productCategory;
        }

        public bool Remove(Guid id)
        {
          var productcategory = _shoppingCartDbContext.ProductCategories.Find(id);
            _shoppingCartDbContext.ProductCategories.Remove(productcategory);
            _shoppingCartDbContext.SaveChanges();
            return true;
        }
    }
}
