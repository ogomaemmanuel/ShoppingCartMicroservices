using BasketService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Services
{
   public interface IRepository<T> where T:class
    {
        bool Add(T t,string customerId);
        bool ClearBasketByCustomerId(string id);
        List<T> GetByCustomerId(string customerId);
        bool RemoveSingleBasketItem(Guid itemId, string customerId);
        bool UpdateBasketItem(T t, string customerId);
        int Count(string id);
    }
}
