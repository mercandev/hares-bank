using System;
namespace HB.Domain.Model
{
	public class BaseModel
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public string? CreatedBy { get; set; }
		public DateTime UpdatedDate { get; set; } = DateTime.Now;
		public string? UpdatedBy { get; set; }
		public bool IsActive { get; set; } = true;
	}
}

