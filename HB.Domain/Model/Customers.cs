using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.Domain.Model
{
	public class Customers : BaseModel
	{
		public string? Name { get; set; }
		public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public DateTime DateOfBrith { get; set; }
		public bool IsBlackList { get; set; }

		public string? PhoneNumber { get; set; }
        public int BranchOfficesId { get; set; }
        
        [ForeignKey("BranchOfficesId")]
        public virtual BranchOffices? BranchOffices { get; set; }

        public ICollection<Accounts>? Accounts { get; set; }
        public ICollection<Documents>? Documents { get; set; }
    }
}
