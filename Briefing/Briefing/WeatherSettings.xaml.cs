using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace Briefing
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WeatherSettings : ContentPage
	{
        bool finishedLoading = false;
		public WeatherSettings ()
		{
            finishedLoading = false;
			InitializeComponent ();
            if (Application.Current.Properties["appTheme"].ToString() == "dark")
            {
                Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
                Resources["searchButtonStyle"] = Resources["darkThemeSearchButtonStyle"];
            }
            else
            {
                Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                Resources["searchButtonStyle"] = Resources["lightThemeSearchButtonStyle"];
                BackgroundColor = Color.White;
            }
            BuildWeatherSettings();
		}

        void BuildWeatherSettings()
        {
            //daily highs lows
            StackLayout dailyHighLowStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 40};
            CheckBox dailyHighLowCheckBox = new CheckBox {HorizontalOptions = LayoutOptions.Start, Checked = false, VerticalOptions = LayoutOptions.CenterAndExpand};
            dailyHighLowStack.Children.Add(dailyHighLowCheckBox);
            dailyHighLowCheckBox.CheckedChanged += DailyHighLowCheckBox_CheckedChanged;
            Label dailyHighLowLabel = new Label { Text = "Daily Highs/Lows", FontSize = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand};
            dailyHighLowLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            dailyHighLowStack.Children.Add(dailyHighLowLabel);
            MainWeatherSettingsStack.Children.Add(dailyHighLowStack);
            //precipitation outline
            StackLayout precipitationOutlineStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 40 };
            CheckBox precipitationOutline = new CheckBox { HorizontalOptions = LayoutOptions.Start, Checked = false, VerticalOptions = LayoutOptions.CenterAndExpand };
            precipitationOutlineStack.Children.Add(precipitationOutline);
            precipitationOutline.CheckedChanged += PrecipitationOutline_CheckedChanged;
            Label precipitationOutlineLabel = new Label { Text = "Precipitation Outline", FontSize = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand };
            precipitationOutlineLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            precipitationOutlineStack.Children.Add(precipitationOutlineLabel);
            MainWeatherSettingsStack.Children.Add(precipitationOutlineStack);
            //cloud cover outline
            StackLayout cloudCoverOutlineStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 40 };
            CheckBox cloudCoverOutline = new CheckBox { HorizontalOptions = LayoutOptions.Start, Checked = false, VerticalOptions = LayoutOptions.CenterAndExpand };
            cloudCoverOutlineStack.Children.Add(cloudCoverOutline);
            cloudCoverOutline.CheckedChanged += CloudCoverOutline_CheckedChanged;
            Label cloudCoverOutlineLabel = new Label { Text = "Cloud Cover Outline", FontSize = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand };
            cloudCoverOutlineLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            cloudCoverOutlineStack.Children.Add(cloudCoverOutlineLabel);
            MainWeatherSettingsStack.Children.Add(cloudCoverOutlineStack);
            //daily humidity
            StackLayout dailyHumidityStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 40 };
            CheckBox dailyHumidity = new CheckBox { HorizontalOptions = LayoutOptions.Start, Checked = false, VerticalOptions = LayoutOptions.CenterAndExpand };
            dailyHumidityStack.Children.Add(dailyHumidity);
            dailyHumidity.CheckedChanged += DailyHumidity_CheckedChanged;
            Label dailyHumidityLabel = new Label { Text = "Daily Humidity Level", FontSize = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand };
            dailyHumidityLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            dailyHumidityStack.Children.Add(dailyHumidityLabel);
            MainWeatherSettingsStack.Children.Add(dailyHumidityStack);
            //sunrise sunset
            StackLayout sunriseSunsetStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 40 };
            CheckBox sunriseSunset = new CheckBox { HorizontalOptions = LayoutOptions.Start, Checked = false, VerticalOptions = LayoutOptions.CenterAndExpand };
            sunriseSunsetStack.Children.Add(sunriseSunset);
            sunriseSunset.CheckedChanged += SunriseSunset_CheckedChanged;
            Label sunriseSunsetLabel = new Label { Text = "Sunrise/Sunset Times", FontSize = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand };
            sunriseSunsetLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            sunriseSunsetStack.Children.Add(sunriseSunsetLabel);
            MainWeatherSettingsStack.Children.Add(sunriseSunsetStack);

            if (Application.Current.Properties["dailyHighLow"].ToString() == "t")
            {
                dailyHighLowCheckBox.Checked = true;
            }
            if (Application.Current.Properties["precipitationOutline"].ToString() == "t")
            {
                precipitationOutline.Checked = true;
            }
            if(Application.Current.Properties["cloudCoverOutline"].ToString() == "t")
            {
                cloudCoverOutline.Checked = true;
            }
            if (Application.Current.Properties["dailyHumidity"].ToString() == "t")
            {
                dailyHumidity.Checked = true;
            }
            if (Application.Current.Properties["sunriseSunset"].ToString() == "t")
            {
                sunriseSunset.Checked = true;
            }

            //divider
            MainWeatherSettingsStack.Children.Add(new Image { Source = "line.png", HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 10 });
            //use location check
            StackLayout useLocationStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 40 };
            CheckBox useLocation = new CheckBox { HorizontalOptions = LayoutOptions.Start, Checked = false, VerticalOptions = LayoutOptions.CenterAndExpand };
            useLocationStack.Children.Add(useLocation);
            useLocation.CheckedChanged += UseLocation_CheckedChanged;
            Label useLocationLabel = new Label { Text = "Use Current Location", FontSize = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand };
            useLocationLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            useLocationStack.Children.Add(useLocationLabel);
            MainWeatherSettingsStack.Children.Add(useLocationStack);
            if (Application.Current.Properties["useLocationWeather"].ToString() == "t")
            {
                useLocation.Checked = true;
                CustomLocationFrame.BackgroundColor = Color.Gray;
                CustomLocationFrame.BorderColor = Color.Gray;
                CustomWeatherLocationTitle.TextColor = Color.Gray;
                CustomWeatherLocation.IsVisible = false;
                CustomLocationSeachButton.BackgroundColor = Color.Gray;
                customLocationEditor.IsEnabled = false;
                CustomLocationSeachButton.IsEnabled = false;
            }
            else
            {
                CustomLocationFrame.BackgroundColor = Color.White;
                CustomLocationFrame.BorderColor = Color.White;
                CustomWeatherLocationTitle.TextColor = Color.White;
                CustomWeatherLocation.IsVisible = true;
                CustomLocationSeachButton.BackgroundColor = Color.White;
                customLocationEditor.IsEnabled = true;
                CustomLocationSeachButton.IsEnabled = true;
            }

            if(Application.Current.Properties["customWeatherLocation"].ToString().Replace(" ", "") != "")
            {
                CustomWeatherLocation.Text = Application.Current.Properties["customWeatherLocation"].ToString();
            }
            finishedLoading = true;
        }

        async void SearchButton_Clicked(object sender, EventArgs e)
        {
            WeatherLocationsStack.Children.Clear();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + customLocationEditor.Text.Trim().Replace(" ", "+") + "&types=geocode&key=AIzaSyBWpXqLEaG3lmFYTHqJaM-f1eglchCPXVA");
            string result = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(result);
            JArray placeList = JArray.Parse(json["predictions"].ToString());
            if (placeList.Count() > 0)
            {
                WeatherLocationsStack.Children.Add(new Image { Source = "line.png", HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 10 });
                for (int i = 0; i < placeList.Count(); i++)
                {
                    Label weatherLocationLabel = new Label { Text = placeList[i]["description"].ToString(), ClassId = placeList[i]["place_id"].ToString(), FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                    weatherLocationLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += LocationSelected;
                    weatherLocationLabel.GestureRecognizers.Add(tapGestureRecognizer);
                    WeatherLocationsStack.Children.Add(weatherLocationLabel);
                    if (i < placeList.Count() - 1)
                    {
                        WeatherLocationsStack.Children.Add(new Image { Source = "line.png", HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 10 });
                    }
                }
            }
            else
            {
                DisplayAlert("Location not found", "No results were found for " + customLocationEditor.Text.Trim() + ".", "OK");
            }
        }

        async void LocationSelected(object sender, EventArgs e)
        {
            try
            {
                Label label = (Label)sender;
                Application.Current.Properties["customWeatherLocation"] = label.Text;
                CustomWeatherLocation.Text = label.Text;
                //get coordinates
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/place/details/json?fields=geometry,utc_offset&placeid=" + label.ClassId + "&key=AIzaSyBWpXqLEaG3lmFYTHqJaM-f1eglchCPXVA");
                string result = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                Application.Current.Properties["customWeatherLocationCoords"] = json["result"]["geometry"]["location"]["lat"].ToString() + "," + json["result"]["geometry"]["location"]["lng"].ToString();
                Application.Current.Properties["customWeatherLocationTimeOffset"] = json["result"]["utc_offset"].ToString();
                WeatherLocationsStack.Children.Clear();
                customLocationEditor.Text = "";
                if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            }
            catch
            {
                DisplayAlert("Error", "There was an error getting the coordinates for that location", "OK");
            }
        }

        private void UseLocation_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            WeatherLocationsStack.Children.Clear();
            if (e.Value)
            {
                Application.Current.Properties["useLocationWeather"] = "t";
                CustomLocationFrame.BackgroundColor = Color.Gray;
                CustomWeatherLocation.IsVisible = false;
                customLocationEditor.IsEnabled = false;
                CustomLocationSeachButton.BackgroundColor = Color.Gray;
                CustomLocationFrame.BorderColor = Color.Gray;
                CustomWeatherLocationTitle.TextColor = Color.Gray;
                CustomLocationSeachButton.IsEnabled = false;
            }
            else
            {
                Application.Current.Properties["useLocationWeather"] = "f";
                CustomLocationFrame.BackgroundColor = Color.White;
                CustomWeatherLocation.IsVisible = true;
                CustomLocationSeachButton.BackgroundColor = Color.White;
                CustomWeatherLocationTitle.TextColor = Color.White;
                CustomLocationFrame.BorderColor = Color.White;
                customLocationEditor.IsEnabled = true;
                CustomLocationSeachButton.IsEnabled = true;
            }
        }

        private void CloudCoverOutline_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["cloudCoverOutline"] = "t";
            }
            else
            {
                Application.Current.Properties["cloudCoverOutline"] = "f";
            }
        }

        private void SunriseSunset_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["sunriseSunset"] = "t";
            }
            else
            {
                Application.Current.Properties["sunriseSunset"] = "f";
            }
        }

        private void DailyHumidity_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["dailyHumidity"] = "t";
            }
            else
            {
                Application.Current.Properties["dailyHumidity"] = "f";
            }
        }

        private void PrecipitationOutline_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["precipitationOutline"] = "t";
            }
            else
            {
                Application.Current.Properties["precipitationOutline"] = "f";
            }
        }

        private void DailyHighLowCheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["dailyHighLow"] = "t";
            }
            else
            {
                Application.Current.Properties["dailyHighLow"] = "f";
            }
        }
    }
}