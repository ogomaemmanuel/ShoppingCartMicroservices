using ManufacturerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManufacturerService.Services
{
  
    public class ManufacturerManager : IRepository<Manufacturer>
    {
        private ShoppingCartDbContext _shoppingCartDbContext;
        public ManufacturerManager(ShoppingCartDbContext shoppingCartDbContext)
        {
            _shoppingCartDbContext = shoppingCartDbContext;
        }
        public bool Add(Manufacturer t)
        {
            try
            {
                this._shoppingCartDbContext.Manufacturers.Add(t);
                _shoppingCartDbContext.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public IEnumerable<Manufacturer> GetAll()
        {
          var manufacturers=  _shoppingCartDbContext.Manufacturers.ToList();
            return manufacturers;
        }

        public Manufacturer GetById(Guid id)
        {
            var manufacturer = _shoppingCartDbContext.Manufacturers.Find(id);
            return manufacturer;
        }

        public bool Remove(Guid id)
        {
            try
            {
                _shoppingCartDbContext.Manufacturers.Remove(GetById(id));
                _shoppingCartDbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
