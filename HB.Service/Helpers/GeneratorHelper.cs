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
			var randomPaymentType = System.Enum.GetValues<CardPaymentType>();
			var cardPaymentType = (CardPaymentType)randomPaymentType.GetValue(new Random().Next(randomPaymentType.Length));

            return new CardGeneratorViewModel {
                CardNumber = CardNumberCreate(cardPaymentType),
				LastUseMount = new Random().Next(1, 12).ToString(),
                LastUseYear = new Random().Next(25, 35).ToString(),
				Cvv = DigitGenerator(3),
				CardPaymentType = cardPaymentType
			};
		}

		private static string CardNumberCreate(CardPaymentType cardPayment)
		{
            var firstFourDigit = string.Empty;

            if (cardPayment == CardPaymentType.MasterCard)
            {
                firstFourDigit = CardPaymentTypeConst.MASTERCARD.Split(";").ToList().PickRandom();
            }

            if (cardPayment == CardPaymentType.Visa)
            {
                firstFourDigit = CardPaymentTypeConst.VISA.Split(";").ToList().PickRandom();
            }

            if (cardPayment == CardPaymentType.Troy)
            {
                firstFourDigit = CardPaymentTypeConst.TROY.Split(";").ToList().PickRandom();
            }

            return firstFourDigit + DigitGenerator(14);
        }

		private static string DigitGenerator(int digit)
		{
            var result = new char[digit];
            for (var j = 0; j < digit; j++) result[j] = (char)(new Random().NextDouble() * 10 + 48);
			return new string(result);
        }

        public static T PickRandom<T>(this List<T> enumerable)
        {
            int index = new Random().Next(0, enumerable.Count());
            return enumerable[index];
        }

		
    }
}

