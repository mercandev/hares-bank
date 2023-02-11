using System;
namespace HB.SharedObject.ExchangeViewModel
{
	public class ExchangeResponseViewModel
	{
		public CurrencyDetail USD { get; set; }
        public CurrencyDetail EUR { get; set; }
        public CurrencyDetail GBP { get; set; }
        public CurrencyDetail GA { get; set; }
        public CurrencyDetail C { get; set; }
        public CurrencyDetail GAG { get; set; }
        public CurrencyDetail BTC { get; set; }
        public CurrencyDetail ETH { get; set; }
        public CurrencyDetail XU100 { get; set; }
    }

    public class CurrencyDetail
    {
		public string satis { get; set; }
		public string alis { get; set; }
		public string degisim { get; set; }
		public string d_oran { get; set; }
		public string d_yon { get; set; }
	}

}

