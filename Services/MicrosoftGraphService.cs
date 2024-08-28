using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IMicrosoftGraphService
    {
        Task<AdResponseModel> GetUserIdFromGraphAsync(string accessToken);
    }
    public class MicrosoftGraphService : IMicrosoftGraphService
    {
        private readonly HttpClient _httpClient;

        public MicrosoftGraphService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<AdResponseModel> GetUserIdFromGraphAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<AdResponseModel>(jsonResponse);
                return userData;
            }

            throw new Exception("Failed to get user ID from Microsoft Graph");
        }
    }
}
