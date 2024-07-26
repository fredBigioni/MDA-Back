using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Dominio { get; set; }
        public bool IsAdmin { get; set; }


        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}