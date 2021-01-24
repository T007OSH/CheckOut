using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOut.Com.PaymentServices.Dtos
{
    public class Transaction
    {
        public string CardNumber { get; set; }
        public DateTime Expiry { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Cvv { get; set; }
        public Status PaymentStatus { get; set; }
        public DateTime TimeOfTransaction { get; set; }
    }

    public enum Status
    {
        Authorised, // when bank accepts transaction
        Paid, // when money is paid to PG or merchant 
        Pending,
        Declined,
        Refunded
    }
}
