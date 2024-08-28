namespace WebApi.Models
{
    public class AzureAdOptionsModel
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string ClientSecret { get; set; }

        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_domain { get; set; }
    }
}
