using System;
using HB.Domain.Model;

namespace HB.SharedObject
{
	public class CardGeneratorViewModel
	{
		public string? CardNumber { get; set; }
		public string? LastDate { get; set; }
		public string? LastYear { get; set; }
		public string? Cvv { get; set; }
		public CardPaymentType CardPaymentType { get; set; }
	}
}

