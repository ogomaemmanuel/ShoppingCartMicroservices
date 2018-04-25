using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Models
{
    public class LipaNaMpesaResponse
    {
       public int ResultCode { get; set; }
       public String ResultDesc { get; set; }
       public String ThirdPartyTransID { get; set; }
    }
}
