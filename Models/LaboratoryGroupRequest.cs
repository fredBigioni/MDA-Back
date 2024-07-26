using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class LaboratoryGroupRequest
    {
        [Required]
        public string Description { get; set; }
        public string Class { get; set; }
        public virtual ICollection<int> LaboratoryCodes { get; set; }
    }
}