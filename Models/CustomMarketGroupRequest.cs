using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CustomMarketGroupRequest
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string GroupCondition { get; set; }
        [Required]
        public int CustomMarketCode { get; set; }
    }
}