using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class AdAuthenticateRequest
    {
        public string Id { get; set; }

        public string AccessToken { get; set; }
    }
}
