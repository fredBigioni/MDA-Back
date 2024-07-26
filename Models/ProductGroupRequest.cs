using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class ProductGroupRequest
    {
        [Required]
        public string Description { get; set; }
        public virtual ICollection<int> ProductCodes { get; set; }
    }
}