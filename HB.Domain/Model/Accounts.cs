using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.Domain.Model
{
	public class Accounts : BaseModel
	{
		public string? Name { get; set; }
		public decimal Amount { get; set; }
		public int BranchOfficesId { get; set; }
		public int CurrencyId { get; set; }

		//MAPPING
        public Customers? Customers { get; set; }

        [ForeignKey("BranchOfficesId")]
        public virtual BranchOffices? BranchOffices { get; set; }
	}
}

	