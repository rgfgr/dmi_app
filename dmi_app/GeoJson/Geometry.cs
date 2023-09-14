using Newtonsoft.Json;
namespace dmi_app.GeoJson
{

    public class Geometry
    {
        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

}