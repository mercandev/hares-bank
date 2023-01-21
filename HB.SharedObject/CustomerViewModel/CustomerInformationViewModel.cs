using System;
using HB.Domain.Model;

namespace HB.SharedObject
{
	public class CustomerInformationViewModel
	{
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public BranchViewModel? Branch { get; set; }
		public AddressViewModel? Address { get; set; }
		public List<AccountsViewModel> Accounts { get; set; }
	}

	public class BranchViewModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
	}

	public class AddressViewModel
	{
		public int Id { get; set; }
		public string? AddressExplanation { get; set; }
	}

	public class AccountsViewModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public decimal Amount { get; set; }
		public int CurrencyId { get; set; }
		public string? Iban { get; set; }
	}
}

