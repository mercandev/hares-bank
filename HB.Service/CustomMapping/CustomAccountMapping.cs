using System;
using HB.Domain.Model;
using HB.Service.Helpers;

namespace HB.Service.CustomMapping
{
	public static class CustomAccountMapping
	{
        public static Accounts CreateAccountMapping(Customers customerInformation, int currencyId)
        {
            return new()
            {
                BranchOfficesId = customerInformation.BranchOfficesId,
                CurrencyId = currencyId,
                Iban = GeneratorHelper.IbanGenerator(),
                CustomersId = customerInformation.Id,
                Name = $"{HbHelpers.GetEnumDescriptionIntToEnum((Currency)currencyId)} hesabım",
                CreatedBy = customerInformation.Name
            };
        }

    }
}

