using System;
using HB.Domain.Model;
using HB.Service.Const;
using HB.SharedObject;
using HB.SharedObject.CardViewModel;

namespace HB.Service.Helpers
{
	public static class GeneratorHelper
	{

		public static string? IbanGenerator() //MARK: this method not truth.
		{
			var controlDigit = DigitGenerator(2);
			var lastSixteenDigit = DigitGenerator(16);

			return $"{GeneratorConst.COUNTRY_CODE}{controlDigit}{GeneratorConst.BANK_CODE}{GeneratorConst.REZERV_CODE}{lastSixteenDigit}";
		}

		public static CardGeneratorViewModel CardGenerator()
		{
			var randomPaymentType = Enum.GetValues<CardPaymentType>();

            return new CardGeneratorViewModel {
				CardNumber = DigitGenerator(16),
				LastDate = new Random().Next(10, 25).ToString(),
                LastYear = new Random().Next(25, 35).ToString(),
				Cvv = DigitGenerator(3),
				CardPaymentType = (CardPaymentType)randomPaymentType.GetValue(new Random().Next(randomPaymentType.Length))
			};
		}

		private static string DigitGenerator(int digit)
		{
            var result = new char[digit];
            for (var j = 0; j < digit; j++) result[j] = (char)(new Random().NextDouble() * 10 + 48);
			return new string(result);
        }
	}
}

