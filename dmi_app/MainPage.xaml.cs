using dmi_app.GeoJson;
using dmi_app.HttpStuff;
using Microsoft.Maui.Controls.Maps;
using System.Globalization;
using System.Text;

namespace dmi_app
{
    public partial class MainPage : ContentPage
    {
        private const double lon1 = 7.0;
        private const double lat1 = 54.0;
        private const double lon2 = 16.0;
        private const double lat2 = 58.0;
        public MainPage()
        {
            InitializeComponent();
            var date = DateTime.Today.AddDays(-1);
            fromDate.MaximumDate = date;
            toDate.MaximumDate = date;
            Reset_Map();
        }

        private async void Get_data_Button_Clicked(object sender, EventArgs e)
        {
            get_map_data.IsEnabled = false;
            try
            {
                await MapStuff(fromDate.Date, toDate.Date, (int)observations_slider.Value, (int)iterations_slider.Value, yr_ol.IsChecked);
            }
            catch (Exception ex)
            {
                error.Text = ex.Message;
            }
            get_map_data.IsEnabled = true;
        }

        private async Task MapStuff(DateTime? fromDate, DateTime? toDate, int limit, int iterations, bool yr_ol)
        {
            error.Text = "";
            if (fromDate > toDate)
            {
                throw new Exception("From date can't be more than To date");
            }
            var ci = CultureInfo.CreateSpecificCulture("en-US");
            Dictionary<string, object> _params = new()
            {
                {"limit",  limit},
                {"datetime", DateToString(fromDate, toDate, yr_ol)},
                {"bbox", $"{lon1.ToString(ci)},{lat1.ToString(ci)},{lon2.ToString(ci)},{lat2.ToString(ci)}"},
                {"offset", "1000" },
            };
            var curr_observations_task = Lightning_Repo.GetObservationsAsync(_params);
            error.Text = "Loading...";
            Reset_Map();
            var curr_observations = await curr_observations_task;
            foreach (var i in Enumerable.Range(1, iterations))
            {
                Task<FeatureCollection> next_observations = null;
                var last = i == iterations;
                if (!last)
                {
                    next_observations = Lightning_Repo.GetObservationsWithBuiltUrlAsync(curr_observations.Links[1].Href);
                }
                foreach (var pin in LightningMapGenerator.GeneratePinData(curr_observations))
                {
                    myMap.Pins.Add(pin.GetPin());
                }
                if (last)
                {
                    break;
                }
                curr_observations = await next_observations;
            }
            error.Text = "Done";
        }

        private static string DateToString(DateTime? fromDate, DateTime? toDate, bool yr_ol)
        {
            DateTime? dt;
            if (yr_ol)
            {
                int? year = fromDate?.Year;
                dt = year.HasValue ? new(year.Value, 1, 1) : null;
            }
            else
                dt = fromDate?.Date;

            StringBuilder sb = new();
            sb.Append(dt?.ToString("yyyy-MM-dd'T''00:00:00''Z'") ?? "..");
            sb.Append('/');
            if (yr_ol)
            {
                int? year = toDate?.Year;
                dt = year.HasValue ? new(year.Value, 12, 31) : null;
            }
            else
                dt = toDate?.Date;
            sb.Append(dt?.ToString("yyyy-MM-dd'T''23:59:59''Z'") ?? "..");
            return sb.ToString();
        }

        readonly Pin sw_pin = new()
        {
            Location = new(lat1, lon1),
            Address = "South west",
            Label = "South west point",
        };

        readonly Pin ne_pin = new()
        {
            Location = new(lat2, lon2),
            Address = "North east",
            Label = "North east point",
        };
        private void Reset_Map()
        {
            myMap.Pins.Clear();
            myMap.Pins.Add(sw_pin);
            myMap.Pins.Add(ne_pin);
        }

        private void Observations_slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Change_editor(ref observations_editor, observations_slider);
        }

        private void Observations_editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Change_slider(ref observations_slider, observations_editor);
        }

        private void Iterations_slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Change_editor(ref iterations_editor, iterations_slider);
        }

        private void Iterations_editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Change_slider(ref iterations_slider, iterations_editor);
        }

        private static void Change_slider(ref Slider slider, Editor editor)
        {
            if (double.TryParse(editor.Text, out double val))
            {
                if (val > slider.Maximum)
                {
                    val = slider.Maximum;
                    editor.Text = val.ToString();
                }
                slider.Value = val;
            }
        }

        private static void Change_editor(ref Editor editor, Slider slider)
        {
            editor.Text = ((int)slider.Value).ToString();
        }
    }
}