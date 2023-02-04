using System;
using HB.Infrastructure.Const;
using HB.Infrastructure.Exceptions;
using System.Text.RegularExpressions;

namespace HB.Service.Helpers
{
	public static class PaymentHelper
	{
        public static void IbanValidation(string iban)
        {
            var result = new Regex(IbanConst.IBAN_REGEX_INT).Match(iban);

            if (!result.Success)
            {
                throw new HbBusinessException("Iban is not valid!");
            }
        }

    }
}

