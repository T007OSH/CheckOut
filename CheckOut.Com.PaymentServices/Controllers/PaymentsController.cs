using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckOut.Com.PaymentServices.Dtos;
using CheckOut.Com.PaymentServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CheckOut.Com.PaymentServices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentGatewayService _paymentService;
    
        public PaymentsController(ILogger<PaymentsController> logger, IPaymentGatewayService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody]Card cardDetails)
        {
            try
            {
                _logger.LogInformation("Processing card payment ...");
                var id = await _paymentService.MakeCardPayment(cardDetails);
               
                if (!id.HasValue)
                {
                    _logger.LogError("Issue processing card payment.");
                    return NotOk();
                }
                _logger.LogInformation("Card payment processed.");
                return Ok(id.Value.ToString("D"));
            }
            catch(Exception e)
            {
                _logger.LogError("Issue processing card payment. {e}", e);
                return NotOk();
            }
        }

        [HttpGet]
        [Route("GetPaymentDetails/{paymentId}")]
        public IActionResult GetPaymentDetails(string paymentId)
        {
            try
            {
                _logger.LogInformation($"Retrieving payment detals for  payment ID : {paymentId} ...");
                var details = _paymentService.GetPaymentDetails(paymentId);

                if (details == null)
                {
                    _logger.LogInformation($"No payment details found for  payment ID : {paymentId}.");
                    return NotFound();
                }

                _logger.LogInformation($"Retrieving payment detals for  payment ID : {paymentId} ...");
                return Ok(details);
            }
            catch (Exception e)
            {
                _logger.LogError($"Issue getting payment details for payment Id {paymentId}. {e}");
                return NotOk();
            }
        }

        private IActionResult NotOk()
        {
            return StatusCode(StatusCodes.Status400BadRequest); // different error code ?
        }
    }
}