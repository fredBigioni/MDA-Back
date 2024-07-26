using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class ProductPresentationGroupRequest
    {
        [Required]
        public string Description { get; set; }
        public bool ExpandGroup { get; set; }
        public virtual ICollection<int> ProductPresentationCodes { get; set; }
    }
}