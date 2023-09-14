using dmi_app.GeoJson;

namespace dmi_app.HttpStuff
{
    public class Lightning_Repo
    {
        private static readonly string api_url = "v2/lightningdata/collections";
        private static readonly string _apiKey = Class1.Api_key;

        public async Task<FeatureCollection> GetObservationsAsync(Dictionary<string, object> parameters = null)
        {
            return await DMI_Caller.GetFeatureCollectionAsync($"{api_url}/observation", _apiKey, parameters);
        }

        public async Task<FeatureCollection> GetObservationsWithBuiltUrlAsync(string apiUrl)
        {
            return await DMI_Caller.GetFeatureCollectionWithBuiltUrlAsync(apiUrl);
        }

        public async Task<string> GetObservationsJsonAsync(Dictionary<string, object> parameters = null)
        {
            return await DMI_Caller.GetJsonAsync($"{api_url}/observation", _apiKey, parameters);
        }

        public async Task<string> GetObservationsJsonWithBuiltUrlAsync(string apiUrl)
        {
            return await DMI_Caller.GetJsonWithBuiltUrlAsync(apiUrl);
        }
    }
}
