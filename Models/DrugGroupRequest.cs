using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class DrugGroupRequest
    {
        [Required]
        public string Description { get; set; }
        public virtual ICollection<int> DrugCodes { get; set; }
    }
}