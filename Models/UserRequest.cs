using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserRequest
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Dominio { get; set; }        
        [Required]
        public bool IsAdmin { get; set; }  
    }
}