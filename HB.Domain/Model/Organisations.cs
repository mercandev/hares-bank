using System;
using System.ComponentModel;

namespace HB.Domain.Model
{
	public class Organisations : BaseModel
	{
		public string Name { get; set; }
		public OrganisationType OrganisationType { get; set; }
		public decimal InvoiceAmount { get; set; }
        public string Iban { get; set; }
    }

    public enum OrganisationType
	{
        [Description("Elektrik Faturası")]
        Electric = 1,

        [Description("Su Faturası")]
        Water = 2,

        [Description("İnternet Faturası")]
        Telecom = 3,

        [Description("Telefon Faturası")]
        Mobile = 4,

        [Description("Doğalgaz Faturası")]
        NaturelGas = 5,
    }
	
}