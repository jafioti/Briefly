using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Plugin.GoogleClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace Briefing
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
        string[] order;
        bool finishedLoading = false;

        public Settings ()
		{
            finishedLoading = false;
			InitializeComponent ();
            order = Application.Current.Properties["infoOrder"].ToString().Split('-');
            UserNameEditor.Text = (string)Application.Current.Properties["userName"];
            if (Application.Current.Properties["appTheme"].ToString() == "light")
            {
                DarkModeSwitch.IsToggled = false;
                BackgroundColor = Color.FromHex("#FFFFFF");
                Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                Resources["frameStyle"] = Resources["lightThemeFrameStyle"];
                Resources["settingsStyle"] = Resources["lightThemeSettingsStyle"];
                Resources["upArrowStyle"] = Resources["lightThemeUpArrowStyle"];
                Resources["downArrowStyle"] = Resources["lightThemeDownArrowStyle"];
                Resources["editorStyle"] = Resources["lightThemeEditorStyle"];
            }
            else
            {
                DarkModeSwitch.IsToggled = true;
                BackgroundColor = Color.FromHex("#3a3a3a");
                Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
                Resources["frameStyle"] = Resources["darkThemeFrameStyle"];
                Resources["settingsStyle"] = Resources["darkThemeSettingsStyle"];
                Resources["upArrowStyle"] = Resources["darkThemeUpArrowStyle"];
                Resources["downArrowStyle"] = Resources["darkThemeDownArrowStyle"];
                Resources["editorStyle"] = Resources["darkThemeEditorStyle"];
            }

            //fill in sunset theme stack
            CheckBox sunsetTheme = new CheckBox { Checked = false, HorizontalOptions = LayoutOptions.Start };

            if(Application.Current.Properties["timeAppTheme"].ToString() == "t")
            {
                sunsetTheme.Checked = true;
                DarkModeSwitch.IsEnabled = false;
            }
            sunsetTheme.CheckedChanged += SunsetTheme_CheckedChanged;
            SunsetThemeStack.Children.Add(sunsetTheme);
            Label sunsetThemeLabel = new Label { Text = "Use Sunset to Determine Theme", FontSize = 18, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand};
            sunsetThemeLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            SunsetThemeStack.Children.Add(sunsetThemeLabel);
            BuildInformationStack();

            if(Application.Current.Properties["googleLogin"].ToString() == "t")
            {
                GoogleLoginLabel.Text = "Logged In to Google";
                GoogleLoginButton.Text = "Logout";
            }
		}

        async void SunsetTheme_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (e.Value)
            {
                Application.Current.Properties["timeAppTheme"] = "t";
                DarkModeSwitch.IsEnabled = false;
                HttpClient client = new HttpClient();
                string lat = "";
                string lon = "";
                if (Application.Current.Properties["useLocationWeather"].ToString() == "t" || Application.Current.Properties["customWeatherLocationCoords"].ToString() == "0,0")
                {
                    var locator = CrossGeolocator.Current;
                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                    lat = position.Latitude.ToString();
                    lon = position.Longitude.ToString();
                }
                else
                {
                    string[] temp = Application.Current.Properties["customWeatherLocationCoords"].ToString().Split(',');
                    lat = temp[0];
                    lon = temp[1];
                }
                HttpResponseMessage response = await client.GetAsync("https://api.openweathermap.org/data/2.5/weather?" + "lat=" + lat + "&lon=" + lon + "&mode=json&units=imperial&appid=f65268dddf50ca94e95bedb00e196013");
                string result = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                DateTime[] times = await GetSunRiseSetTimes(lat, lon, json);
                Application.Current.Properties["sunsetTime"] = times[1].ToString("yyyy-MM-dd HH:mm tt");
                if (DateTime.Now > times[1])
                {
                    Application.Current.Properties["appTheme"] = "dark";
                    DarkModeSwitch.IsToggled = true;
                    BackgroundColor = Color.FromHex("#3a3a3a");
                    Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
                    Resources["frameStyle"] = Resources["darkThemeFrameStyle"];
                    Resources["settingsStyle"] = Resources["darkThemeSettingsStyle"];
                    Resources["upArrowStyle"] = Resources["darkThemeUpArrowStyle"];
                    Resources["downArrowStyle"] = Resources["darkThemeDownArrowStyle"];
                    Resources["editorStyle"] = Resources["darkThemeEditorStyle"];
                }
                else
                {
                    Application.Current.Properties["appTheme"] = "light";
                    DarkModeSwitch.IsToggled = false;
                    BackgroundColor = Color.FromHex("#FFFFFF");
                    Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                    Resources["frameStyle"] = Resources["lightThemeFrameStyle"];
                    Resources["settingsStyle"] = Resources["lightThemeSettingsStyle"];
                    Resources["upArrowStyle"] = Resources["lightThemeUpArrowStyle"];
                    Resources["downArrowStyle"] = Resources["lightThemeDownArrowStyle"];
                    Resources["editorStyle"] = Resources["lightThemeEditorStyle"];
                }
            }
            else
            {
                Application.Current.Properties["timeAppTheme"] = "f";
                DarkModeSwitch.IsEnabled = true;
            }
        }

        void BuildInformationStack()
        {
            MainInfoStack.Children.Clear();
            for (int i = 0; i < order.Length; i++)
            {
                string title = char.ToUpper(order[i][0]) + order[i].Substring(1);
                string metaTitle = order[i];
                if (title == "Calendar") { title = "Schedule"; }
                if (title == "Todo") { title = "To Do List"; }
                Frame infoFrame = new Frame { CornerRadius = 20, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 45 };
                infoFrame.SetDynamicResource(VisualElement.StyleProperty, "frameStyle");
                StackLayout horizontalStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 40 };
                infoFrame.Content = horizontalStack;
                CheckBox sectionBox = new CheckBox { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, Checked = false, ClassId = metaTitle};
                sectionBox.CheckedChanged += SectionBox_CheckedChanged;
                if (Application.Current.Properties[metaTitle + "Section"].ToString() == "t")
                {
                    sectionBox.Checked = true;
                }
                horizontalStack.Children.Add(sectionBox);
                Label titleLabel = new Label { Text = title, FontSize = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand};
                titleLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
                horizontalStack.Children.Add(titleLabel);
                Xamarin.Forms.ImageButton settingsButton = new Xamarin.Forms.ImageButton {};
                settingsButton.SetDynamicResource(VisualElement.StyleProperty, "settingsStyle");
                settingsButton.Clicked += Settings_Tapped;
                horizontalStack.Children.Add(settingsButton);
                StackLayout verticalStack = new StackLayout { Orientation = StackOrientation.Vertical, VerticalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 40, HorizontalOptions = LayoutOptions.End, Padding = 0};
                Xamarin.Forms.ImageButton upButton = new Xamarin.Forms.ImageButton {};
                upButton.SetDynamicResource(VisualElement.StyleProperty, "upArrowStyle");
                upButton.Clicked += UpButtonTapped;
                verticalStack.Children.Add(upButton);
                Xamarin.Forms.ImageButton downButton = new Xamarin.Forms.ImageButton {};
                downButton.SetDynamicResource(VisualElement.StyleProperty, "downArrowStyle");
                downButton.Clicked += DownButtonTapped;
                verticalStack.Children.Add(downButton);
                horizontalStack.Children.Add(verticalStack);
                MainInfoStack.Children.Add(infoFrame);
            }
            finishedLoading = true;
        }

        private void SectionBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading && Application.Current.Properties[((CheckBox)sender).ClassId + "Section"].ToString() != e.Value.ToString()[0].ToString().ToLower()) { Application.Current.Properties["settingsChanged"] = "t"; }
            Application.Current.Properties[((CheckBox)sender).ClassId + "Section"] = e.Value.ToString()[0].ToString().ToLower();
            if (e.Value)
            {
                Application.Current.Properties["enabledSections"] = (Convert.ToInt32(Application.Current.Properties["enabledSections"].ToString()) + 1).ToString();
            }
            else
            {
                Application.Current.Properties["enabledSections"] = (Convert.ToInt32(Application.Current.Properties["enabledSections"].ToString()) - 1).ToString();
            }
        }

        void Settings_Tapped(object sender, EventArgs e)
        {
            Xamarin.Forms.ImageButton button = (Xamarin.Forms.ImageButton)sender;
            StackLayout horizontalStack = (StackLayout)button.Parent;
            Label settingsLabel = (Label)horizontalStack.Children[1];
            if(settingsLabel.Text == "Weather")
            {
                this.Navigation.PushAsync(new WeatherSettings());
            }
            else if(settingsLabel.Text == "Email")
            {
                this.Navigation.PushAsync(new EmailSettings());
            }
            else if(settingsLabel.Text == "Schedule")
            {
                this.Navigation.PushAsync(new CalendarSettings());
            }
            else if(settingsLabel.Text == "Stocks")
            {
                this.Navigation.PushAsync(new StocksSettings());
            }
            else if(settingsLabel.Text == "Travel")
            {
                this.Navigation.PushAsync(new Travel());
            }
            else if(settingsLabel.Text == "To Do List")
            {
                this.Navigation.PushAsync(new TodoSettings());
            }
            else
            {
                this.Navigation.PushAsync(new NewsSettings());
            }
        }

        void UpButtonTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.ImageButton button = (Xamarin.Forms.ImageButton)sender;
            StackLayout verticalStack = (StackLayout)button.Parent;
            StackLayout horizontalStack = (StackLayout)verticalStack.Parent;
            Label settingsLabel = (Label)horizontalStack.Children[1];
            string temp = settingsLabel.Text.ToLower();
            if(temp == "schedule") { temp = "calendar"; }
            if (temp == "to do list") { temp = "todo"; }
            int position = Array.IndexOf(order, temp);
            if(position > 0)
            {
                string swap = order[position - 1];
                order[position - 1] = temp;
                order[position] = swap;
                BuildInformationStack();
                SaveOrder();
            }
        }

        void DownButtonTapped(object sender, EventArgs e)
        {
            Xamarin.Forms.ImageButton button = (Xamarin.Forms.ImageButton)sender;
            StackLayout verticalStack = (StackLayout)button.Parent;
            StackLayout horizontalStack = (StackLayout)verticalStack.Parent;
            Label settingsLabel = (Label)horizontalStack.Children[1];
            string temp = settingsLabel.Text.ToLower();
            if (temp == "schedule") { temp = "calendar"; }
            if(temp == "to do list") { temp = "todo"; }
            int position = Array.IndexOf(order, temp);
            if (position < order.Length - 1)
            {
                string swap = order[position + 1];
                order[position + 1] = temp;
                order[position] = swap;
                MainInfoStack.Children.Clear();
                BuildInformationStack();
                SaveOrder();
            }
        }

        void SaveOrder()
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            string temp = "";
            for(int i = 0; i < order.Length; i++)
            {
                temp += order[i];
                if(i < order.Length - 1)
                {
                    temp += "-";
                }
            }
            Application.Current.Properties["infoOrder"] = temp;
        }

        public void OnWeatherTextChanged(object sender, TextChangedEventArgs args)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            Application.Current.Properties["weatherTown"] = args.NewTextValue;
        }

        public void OnUserNameTextChanged(object sender, TextChangedEventArgs args)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            Application.Current.Properties["userName"] = args.NewTextValue;
        }

        public void ToggleTheme(object sender, ToggledEventArgs args)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (args.Value)
            {
                //dark
                Application.Current.Properties["appTheme"] = "dark";
                BackgroundColor = Color.FromHex("#3a3a3a");
                Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
                Resources["frameStyle"] = Resources["darkThemeFrameStyle"];
                Resources["settingsStyle"] = Resources["darkThemeSettingsStyle"];
                Resources["upArrowStyle"] = Resources["darkThemeUpArrowStyle"];
                Resources["downArrowStyle"] = Resources["darkThemeDownArrowStyle"];
                Resources["editorStyle"] = Resources["darkThemeEditorStyle"];
                BuildInformationStack();
            }
            else
            {
                //light
                Application.Current.Properties["appTheme"] = "light";
                BackgroundColor = Color.FromHex("#FFFFFF");
                Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                Resources["frameStyle"] = Resources["lightThemeFrameStyle"];
                Resources["settingsStyle"] = Resources["lightThemeSettingsStyle"];
                Resources["upArrowStyle"] = Resources["lightThemeUpArrowStyle"];
                Resources["downArrowStyle"] = Resources["lightThemeDownArrowStyle"];
                Resources["editorStyle"] = Resources["lightThemeEditorStyle"];
                BuildInformationStack();
            }
        }

        async Task<DateTime[]> GetSunRiseSetTimes(string lat, string lon, JObject json)
        {
            HttpClient client = new HttpClient();
            DateTime sunsetTime = new DateTime();
            DateTime sunriseTime = new DateTime();

            //get sunrise and sunset times
            sunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0, System.DateTimeKind.Utc);
            sunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0, System.DateTimeKind.Utc);

            //adjust for timezone
            int minutesOffset = 0;
            if (Application.Current.Properties["useLocationWeather"].ToString() == "t" || Application.Current.Properties["customWeatherLocation"].ToString() == "")
            {
                if (Application.Current.Properties["localWeatherCoords"].ToString() == "")
                {
                    //get local coords
                    HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/timezone/json?location=" + lat + "," + lon + "&key=AIzaSyBWpXqLEaG3lmFYTHqJaM-f1eglchCPXVA&timestamp=0&sensor=true");
                    string result = await response.Content.ReadAsStringAsync();
                    JObject offsetJson = JObject.Parse(result);
                    minutesOffset = (Convert.ToInt32(offsetJson["rawOffset"].ToString()) / 60);
                    Application.Current.Properties["localWeatherLocationTimeOffset"] = minutesOffset.ToString();
                    Application.Current.Properties["localWeatherCoords"] = lat + "," + lon;
                }
                else
                {
                    double localLat = Convert.ToDouble(Application.Current.Properties["localWeatherCoords"].ToString().Split(',')[0]);
                    double localLon = Convert.ToDouble(Application.Current.Properties["localWeatherCoords"].ToString().Split(',')[1]);
                    //check if they are within 2 coord points of last location
                    if (localLat + 2 < Convert.ToDouble(lat) || localLat - 2 > Convert.ToDouble(lat) || localLon + 2 < Convert.ToDouble(lon) || localLon - 2 > Convert.ToDouble(lon))
                    {
                        //get local coords
                        HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/timezone/json?location=" + lat + "," + lon + "&key=AIzaSyBWpXqLEaG3lmFYTHqJaM-f1eglchCPXVA&timestamp=0&sensor=true");
                        string result = await response.Content.ReadAsStringAsync();
                        JObject offsetJson = JObject.Parse(result);
                        minutesOffset = (Convert.ToInt32(offsetJson["rawOffset"].ToString()) / 60);
                        Application.Current.Properties["localWeatherLocationTimeOffset"] = minutesOffset.ToString();
                        Application.Current.Properties["localWeatherCoords"] = lat + "," + lon;
                    }
                    else
                    {
                        minutesOffset = Convert.ToInt32(Application.Current.Properties["localWeatherLocationTimeOffset"].ToString());
                    }
                }
            }
            else
            {
                minutesOffset = Convert.ToInt32(Application.Current.Properties["customWeatherLocationTimeOffset"].ToString());
                if (minutesOffset < 0)
                {
                    minutesOffset -= 60;
                }
                else
                {
                    minutesOffset += 60;
                }
            }

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
            {
                if (minutesOffset < 0)
                {
                    minutesOffset += 60;
                }
                else
                {
                    minutesOffset -= 60;
                }
            }

            sunsetTime = sunsetTime.AddSeconds(Convert.ToDouble(json["sys"]["sunset"].ToString()) + minutesOffset * 60);
            sunriseTime = sunriseTime.AddSeconds(Convert.ToDouble(json["sys"]["sunrise"].ToString()) + minutesOffset * 60);
            sunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunsetTime.Hour, sunriseTime.Minute, sunsetTime.Second);
            return (new DateTime[] { sunriseTime, sunsetTime });
        }

        public void LoginToGoogle(object sender, EventArgs args)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (Application.Current.Properties["googleLogin"].ToString() == "f")
            {
                DependencyService.Get<ITextToSpeech>().Login();
                GoogleLoginButton.Text = "Logout";
                GoogleLoginLabel.Text = "Logged In to Google";
                Application.Current.Properties["googleLogin"] = "t";
            }
            else
            {
                GoogleLoginLabel.Text = "Google Sign In";
                GoogleLoginButton.Text = "Login To Google";
                Application.Current.Properties["googleLogin"] = "f";
            }
        }
    }
}