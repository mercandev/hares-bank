using System;
using Marten.Schema;

namespace HB.Domain.Model
{
	public class MartenBaseModel
	{
        [Identity]
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

    }
}

