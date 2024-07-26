using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class BusinessUnitRequest
    {
        [Required]
        public string Description { get; set; }
    }
}