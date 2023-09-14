using dmi_app.GeoJson;
using Newtonsoft.Json;
using System.Text;

namespace dmi_app.HttpStuff
{
    public class DMI_Caller
    {
        private static readonly string base_url = "https://dmigw.govcloud.dk/";
        private static readonly HttpClient _httpClient = new();

        public static async Task<FeatureCollection> GetFeatureCollectionAsync(string url,
                                                                              string apiKey,
                                                                              Dictionary<string, object> parameters)
        {
            string json = await GetJsonAsync(url, apiKey, parameters);
            FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(json);
            return featureCollection;
        }

        public static async Task<string> GetJsonAsync(string url,
                                                       string api_key,
                                                       Dictionary<string, object> parameters = null)
        {
            string apiUrl = BuildApiUrl(url, api_key, parameters);
            return await GetJsonWithBuiltUrlAsync(apiUrl);
        }

        public static async Task<FeatureCollection> GetFeatureCollectionWithBuiltUrlAsync(string apiUrl)
        {
            string json = await GetJsonWithBuiltUrlAsync(apiUrl);
            FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(json);
            return featureCollection;
        }

        public static async Task<string> GetJsonWithBuiltUrlAsync(string apiUrl)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
            }

            return await response.Content.ReadAsStringAsync();
        }

        private static string BuildApiUrl(string url, string api_key, Dictionary<string, object> parameters)
        {
            StringBuilder urlBuilder = new(base_url);

            urlBuilder.Append($"{url}/items?");

            if (parameters != null && parameters.Count > 0)
            {
                var parameterStrings = parameters
                    .Select(param => $"{param.Key}={Uri.EscapeDataString(param.Value.ToString())}")
                    .ToArray();

                urlBuilder.Append(string.Join('&', parameterStrings));
                urlBuilder.Append('&');
            }

            urlBuilder.Append("api-key=");
            urlBuilder.Append(api_key);

            return urlBuilder.ToString();
        }
    }
}
