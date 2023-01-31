using System;
using HB.Domain.Model;

namespace HB.SharedObject.CardViewModel
{
	public class CardGeneratorViewModel
	{
		public string CardNumber { get; set; }
		public string LastUseMount { get; set; }
        public string LastUseYear { get; set; }
        public string Cvv { get; set; }
        public CardPaymentType CardPaymentType { get; set; }
        public CardType CardType { get; set; }
    }
}

