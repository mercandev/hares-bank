using System;
using HB.Domain.Model;

namespace HB.SharedObject.CardViewModel
{
	public class EmptyCardsViewModel
	{
        public string CardNumber { get; set; }
        public int LastUseMount { get; set; }
        public int LastUseYear { get; set; }
        public int Cvv { get; set; }
        public string CardPaymentType { get; set; }
    }
}

