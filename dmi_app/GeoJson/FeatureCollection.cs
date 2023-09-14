using Newtonsoft.Json; 
using System.Collections.Generic; 
using System; 
namespace dmi_app.GeoJson{ 

    public class FeatureCollection
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }

        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("numberReturned")]
        public double NumberReturned { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }
    }

}