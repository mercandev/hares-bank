using System;
using System.ComponentModel;

namespace HB.Domain.Model
{
	public class Users : BaseModel
	{
		public string? Username { get; set; }
		public string? Password { get; set; }
		public string? Email { get; set; }
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public DateTime DateOfBrith { get; set; }
		public string? PhoneNumber { get; set; }
		public UserRole UserRole { get; set; }
	}

	public enum UserRole
	{
        [Description("Admin")]
        Admin = 1,
        [Description("Employee")]
        Employee = 2
	}
}

