using System;
namespace HB.SharedObject.ExchangeViewModel
{
	public class ExchangeMappingResponseViewModel
	{
        public ExchangeMappingDetail Gold { get; set; }
        public ExchangeMappingDetail Silver { get; set; }
    }

    public class ExchangeMappingDetail
	{
        public string Selling { get; set; }
        public string Buying { get; set; }
        public string Changing { get; set; }
        public string ChangingRate { get; set; }
        public string ChangeingRoute { get; set; }
    }
}

