using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HB.Domain.Model
{
	public class Documents : BaseModel
	{
        public string ImageName { get; set; }
        public string Size { get; set; }
        public string FullPath { get; set; }
        public FileType FileType { get; set; }

        //MAPPING
        [ForeignKey("CustomersId")]
        public virtual Customers? Customers { get; set; }
    }

    public enum FileType
    {
        ProfileImg = 1,
        Document = 2
    }
}

