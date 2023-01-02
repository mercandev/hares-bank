using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.Domain.Model
{
	public class Address : BaseModel
	{
        public int CustomerId { get; set; }
		public string? AddressExplanation { get; set; }

        //MAPPING
        [ForeignKey("CustomerId")]
        public virtual Customers? Customers { get; set; }

    }
}

