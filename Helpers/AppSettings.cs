namespace WebApi.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int JwtTokenExpiresInMinutes { get; set; }
        public int RefreshTokenExpiresInDays { get; set; }
        public string Domain { get; set; }
    }
}