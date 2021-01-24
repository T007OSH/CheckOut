using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOut.Com.PaymentServices.Dtos
{
    public class CardBalance
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
