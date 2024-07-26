using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class LineGroupRequest
    {
        [Required]
        public string Description { get; set; }
    }
}