using CheckOut.Com.PaymentServices.Dtos;
using CheckOut.Com.PaymentServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckOut.Com.PaymentServices
{
    public class SimulatorBank : IBank
    {
        private static Dictionary<Guid, Transaction> _transactions = new Dictionary<Guid, Transaction>(); // for reconciliation with PG
        private static Dictionary<string, string> _custCard = new Dictionary<string, string>()
        {{"1111222233334444", "1"} };
        private static Dictionary<string, CardBalance> _cardCardBalance = new Dictionary<string, CardBalance>()
        {{"1", new CardBalance{ CardNumber="1111222233334444", Amount= 100m } } };

        public string Name { get; set; } = typeof(SimulatorBank).Name;

        public async Task<Guid?> AuthorisePayment(Transaction transaction)
        {
            if (_custCard.TryGetValue(transaction.CardNumber, out var custId))
            {
                if (_cardCardBalance.TryGetValue(custId, out var balance))
                {
                    if (balance.Amount >= transaction.Amount)
                    {
                        var id = Guid.NewGuid();
                        _cardCardBalance[custId].Amount -= transaction.Amount;
                        await PersistTransactionData(id, transaction);
                        return id;
                    }
                    return null;
                }
                return null;
            }
            return null;
        }

        public Task PersistTransactionData(Guid id, Transaction transaction)
        {
            transaction.PaymentStatus = Status.Authorised;
            _transactions.Add(id, transaction);
            return Task.CompletedTask;
        }

        public Task SendPaymentToMerchant()
        {
            throw new NotImplementedException();
        }

        public Task SendTransactionDetailsToThirdParties()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateCard(Card cardDetails)
        {
            throw new NotImplementedException();
        }
    }
}
