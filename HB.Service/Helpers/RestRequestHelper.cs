using System;
using Newtonsoft.Json;
using System.Text;

namespace HB.Service.Helpers
{
    public static class RestRequestHelper<T> where T : class
    {
        public static async Task<T?> GetService(string url)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(url);

            string result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(result) || !response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                return null;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}

