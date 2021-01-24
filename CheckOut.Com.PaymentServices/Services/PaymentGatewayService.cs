using CheckOut.Com.PaymentServices.Dtos;
using CheckOut.Com.PaymentServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOut.Com.PaymentServices.Services
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly ILogger<PaymentGatewayService> _logger;
        private readonly IBank _bank;
        private static Dictionary<string, Transaction> _transactions = new Dictionary<string, Transaction>();
        public PaymentGatewayService(ILogger<PaymentGatewayService> logger, IBank bank)
        {
            _logger = logger;
            _bank = bank;
        }

        /// <summary>
        /// Validates card, figures out bank, Processes bank payment 
        /// </summary>
        /// <param name="cardDetails"></param>
        /// <returns>Guid which is a form of unique Id</returns>
        public async Task<Guid?> MakeCardPayment(Card cardDetails)
        {
            if (IsLegitCard())
            {
                _logger.LogInformation("Contacting bank ...");
                var cardIssuer = WhichBank(cardDetails);
                _logger.LogInformation($"Bank contacted. Bank is {cardIssuer.Name}");
                var trans = new Transaction()
                {
                    Amount = cardDetails.Amount,
                    CardNumber = cardDetails.CardNumber,
                    Currency = cardDetails.Currency,
                    Expiry = cardDetails.Expiry,
                    TimeOfTransaction = DateTime.Now,
                    Cvv = cardDetails.Cvv,
                    PaymentStatus = Status.Pending,

                };
                _logger.LogInformation($"Transaction set to pending. Authorising by issuer ...");
                var id = await cardIssuer.AuthorisePayment(trans);

                if (id.HasValue)
                {
                    trans.PaymentStatus = Status.Authorised;
                    _logger.LogInformation($"Authorised. with payment ID {id.Value}.");
                    _transactions.Add(id.Value.ToString("D"), trans);
                }
                return id;
            }
            return null;
        }

        /// <summary>
        /// Get money from card issuer
        /// </summary>
        /// <returns></returns>
        Task<Guid?> IPaymentGatewayService.RetrieveMoneyFromBank()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Works out the card issuer based on card details. For now we use MockBank in all cases
        /// </summary>
        /// <param name="cardDetails"></param>
        /// <returns></returns>
        public IBank WhichBank(Card cardDetails)
        {
            return _bank;
        }

        /// <summary>
        /// Gets the payment details based on unigue Id presented by merchant
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>Details of transaction</returns>
        public Task<Transaction> GetPaymentDetails(string paymentId)
        {
            if (_transactions.TryGetValue(paymentId, out var transaction))
            {
                _logger.LogInformation("Masking in process ...");
                transaction.CardNumber = ScrambleCardNumber(transaction.CardNumber);
                _logger.LogInformation("Masking done.");
                return Task.FromResult(transaction);
            }
            return null;
        }
        /// <summary>
        /// Scrambles card number apart from last 4 digits
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        private string ScrambleCardNumber(string cardNumber)
        {
            var length = cardNumber.Length - 4;
            var toScramble = cardNumber.Substring(0, length);
            var scrambled = string.Empty;

            foreach(char c in toScramble.ToCharArray())
            {
                scrambled += "X";
            }
            cardNumber = cardNumber.Replace(toScramble, scrambled);
            return cardNumber;
        }

        private bool IsLegitCard()
        {
            _logger.LogInformation("Card is legit");
            return true;
        }

    }
}
