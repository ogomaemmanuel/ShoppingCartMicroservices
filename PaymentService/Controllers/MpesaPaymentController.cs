﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Services;

namespace PaymentService.Controllers
{
    [Produces("application/json")]
    [Route("api/MpesaPayment")]
    public class MpesaPaymentController : Controller
    {
        private ILipaNaMpesaManager lipaNaMpesaManager;

        public MpesaPaymentController(ILipaNaMpesaManager lipaNaMpesaManager)
        {
            this.lipaNaMpesaManager = lipaNaMpesaManager;
        }

        public IActionResult ReceivePayment(LinaMpesaOnlineTransactionResponse
            linaMpesaOnlineTransactionResponse) {
         var result=   this.lipaNaMpesaManager.Save(linaMpesaOnlineTransactionResponse);
            return new OkObjectResult(result);
        }
    }
}