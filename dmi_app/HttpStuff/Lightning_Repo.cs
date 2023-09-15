using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using dmi_app.GeoJson;

namespace dmi_app.HttpStuff
{
    public static class Lightning_Repo
    {
        private static readonly SecretClient client = new(new(Environment.GetEnvironmentVariable("VaultUri")), new DefaultAzureCredential());

        public static async Task<FeatureCollection> GetObservationsAsync(Dictionary<string, object> parameters = null)
        {
            var (apiKey, apiUrl) = await GetKeyAndUrlAsync();
            return await DMI_Caller.GetFeatureCollectionAsync($"{apiUrl}/observation", apiKey, parameters);
        }

        public static async Task<FeatureCollection> GetObservationsWithBuiltUrlAsync(string apiUrl)
        {
            return await DMI_Caller.GetFeatureCollectionWithBuiltUrlAsync(apiUrl);
        }

        public static async Task<string> GetObservationsJsonAsync(Dictionary<string, object> parameters = null)
        {
            var (apiKey, apiUrl) = await GetKeyAndUrlAsync();
            return await DMI_Caller.GetJsonAsync($"{apiUrl}/observation", apiKey, parameters);
        }

        public static async Task<string> GetObservationsJsonWithBuiltUrlAsync(string apiUrl)
        {
            return await DMI_Caller.GetJsonWithBuiltUrlAsync(apiUrl);
        }

        private static async Task<(string, string)> GetKeyAndUrlAsync()
        {
            var apiKey = await client.GetSecretAsync("Lightning-key");
            var apiUrl = await client.GetSecretAsync("Lightning-url");
            return (apiKey.Value.Value, apiUrl.Value.Value);
        }
    }
}
