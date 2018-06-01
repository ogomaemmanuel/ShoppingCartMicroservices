using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerFavouritesServices.Services
{
    public interface IRepository<T> where T: class
    {
        bool Add(T t, string customerId);
        bool ClearCustomerFovouritesByCustomerId(string id);
        List<T> GetByCustomerId(string customerId);
        bool RemoveSingleFovouriteItem(Guid itemId, string customerId);        
        int Count(string id);
    }
}
