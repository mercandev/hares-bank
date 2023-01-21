using System;
using HB.Domain.Model;

namespace HB.SharedObject.CustomerViewModel
{
	public class CreateCustomerViewModel
	{
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime DateOfBrith { get; set; }
        public string? PhoneNumber { get; set; }
        public int BranchOfficesId { get; set; }
        public string? AddressExplanation { get; set; }
        public string? AccountName { get; set; }
        public decimal Amount { get; set; }
        public Currency CurrencyId { get; set; }
    }
}

