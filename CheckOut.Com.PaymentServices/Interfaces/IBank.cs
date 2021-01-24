using CheckOut.Com.PaymentServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOut.Com.PaymentServices.Interfaces
{
    public interface IBank
    {
        public string Name { get; set; }
        Task<bool> ValidateCard(Card cardDetails);
        Task<Guid?> AuthorisePayment(Transaction transaction);

        Task PersistTransactionData(Guid id, Transaction transaction);
        Task SendPaymentToMerchant();

        Task SendTransactionDetailsToThirdParties();
    }
}
