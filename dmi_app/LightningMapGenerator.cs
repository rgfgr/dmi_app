using dmi_app.GeoJson;
using Newtonsoft.Json;
using Microsoft.Maui.Controls.Maps;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace dmi_app
{
    public class LightningMapGenerator
    {
        public static IEnumerable<LightningPin> GeneratePinData(string geoJson) => GeneratePinData(JsonConvert.DeserializeObject<FeatureCollection>(geoJson));

        public static IEnumerable<LightningPin> GeneratePinData(FeatureCollection observationDataCollection)
        {
            return observationDataCollection.Features.Select(observationData =>
            {
                var coordinates = observationData.Geometry.Coordinates;
                var props = observationData.Properties;

                return new LightningPin()
                {
                    longitude = coordinates[0],
                    latitude = coordinates[1],
                    amp = props.Amp,
                    observed = props.Observed,
                    strokes = (int)props.Strokes,
                    type = (int)props.Type
                };
            });
        }
    }

    public struct LightningPin
    {
        public double longitude;
        public double latitude;
        public double amp;
        public DateTime observed;
        public int strokes;
        public int type;

        public readonly Pin GetPin()
        {
            StringBuilder sb = new();
            sb.Append("Amps: ");
            sb.Append(amp);
            sb.Append(", Observed: ");
            sb.Append(observed);
            sb.Append(", Strokes: ");
            sb.Append(strokes);
            sb.Append(", Type: ");
            sb.Append(type);
            return new()
            {
                Location = new(latitude, longitude),
                Label = sb.ToString(),
                Address = $"Lat: {latitude}, Lng: {longitude}",
            };
        }
    }
}
