using CheckOut.Com.PaymentServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOut.Com.PaymentServices.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<Guid?> MakeCardPayment(Card cardDetails);
        Task<Transaction> GetPaymentDetails(string paymentId);

        IBank WhichBank(Card cardDetails);
        Task<Guid?> RetrieveMoneyFromBank();
    }
}
