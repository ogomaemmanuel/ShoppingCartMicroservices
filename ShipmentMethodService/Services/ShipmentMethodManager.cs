using ShipmentMethodService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipmentMethodService.Services
{
    public class ShipmentMethodManager : IRepository<ShipmentMethod>
    {
        private readonly ShoppingCartDbContext _dbContext;
        public ShipmentMethodManager(ShoppingCartDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public bool Add(ShipmentMethod shipmentMethod)
        {
            try
            {
                this._dbContext.ShipmentMethods.Add(shipmentMethod);
                this._dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<ShipmentMethod> GetAll()
        {
            var shipmentmethods = this._dbContext.ShipmentMethods.ToList();
            return shipmentmethods;
        }

        public ShipmentMethod GetById(Guid id)
        {
            var shipmentMethod = this._dbContext.ShipmentMethods.Find(id);
            return shipmentMethod;
        }

        public bool Remove(Guid id)
        {
            try
            {
                var shipmentMethod = GetById(id);
                this._dbContext.ShipmentMethods.Remove(shipmentMethod);
                this._dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
