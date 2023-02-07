using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HB.SharedObject.TransactionViewModel
{
	public class TransactionsResponseViewModel
	{
		public TransactionsResponseViewModel()
		{
            CardNumber = CardId == default(Guid) ? "0" : CardId.ToString();
		}

		public Guid Id { get; set; }
		public string Explanation { get; set; }
		public int AccountId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid CardId { get; set; }

        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
		public string TransactionsType { get; set; }
		public string ProccessType { get; set; }
		public decimal AvailableBalance { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}

