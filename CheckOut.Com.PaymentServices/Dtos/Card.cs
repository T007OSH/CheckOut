using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CheckOut.Com.PaymentServices.Dtos
{
    public class Card
    {
        [Required]
        [StringLength(16, MinimumLength = 15)]
        public string CardNumber { get; set; }

        [Required]
        public DateTime Expiry { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [StringLength(3, MinimumLength =3)]
        public string Currency { get; set; }

        [Required]
        [StringLength(4, MinimumLength =3)]
        public string Cvv { get; set; }
    }
}
