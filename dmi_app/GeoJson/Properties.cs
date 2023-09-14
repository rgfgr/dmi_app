using Newtonsoft.Json;
namespace dmi_app.GeoJson{ 

    public class Properties
    {
        [JsonProperty("amp")]
        public double Amp { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("observed")]
        public DateTime Observed { get; set; }

        [JsonProperty("sensors")]
        public string Sensors { get; set; }

        [JsonProperty("strokes")]
        public float Strokes { get; set; }

        [JsonProperty("type")]
        public float Type { get; set; }
    }

}