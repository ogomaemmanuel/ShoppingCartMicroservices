using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Models
{
    
        public class CustomerOrder
        {
            public Guid? OrderId { get; set; }
            public string CustomerId { get; set; }
        }
    
}
