using System.Text.Json.Serialization;
using WebApi.Entities;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
        public bool IsAdmin { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            UserName = user.Usuario;
            Domain = user.Dominio;
            IsAdmin = user.IsAdmin;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}