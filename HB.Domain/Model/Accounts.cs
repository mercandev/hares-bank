using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.Domain.Model
{
	public class Accounts : BaseModel
	{
		public Accounts()
		{
			Name = string.IsNullOrWhiteSpace(Name) ? Name = "Main" : Name;
			CurrencyId = CurrencyId == 0 ? CurrencyId = (int)Currency.TRY : CurrencyId;
		}

		public string? Name { get; set; }
		public decimal Amount { get; set; }
		public int BranchOfficesId { get; set; }
		public int CurrencyId { get; set; }
        public int CustomersId { get; set; }
		public string? Iban { get; set; }

		//MAPPING
		[ForeignKey("CustomersId")]
        public virtual Customers? Customers { get; set; }

        [ForeignKey("BranchOfficesId")]
        public virtual BranchOffices? BranchOffices { get; set; }
	}

	public enum Currency
	{
		TRY = 1,
		USD = 2,
		EURO = 3
	}
}

	