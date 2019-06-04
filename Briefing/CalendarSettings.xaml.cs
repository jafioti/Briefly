using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace Briefing
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarSettings : ContentPage
	{
        List<string> savedCalendars = new List<string>();
        Label exampleLabel;
        bool finishedLoading = false;

        public CalendarSettings ()
		{
            finishedLoading = false;
			InitializeComponent ();
            if (Application.Current.Properties["appTheme"].ToString() == "dark")
            {
                Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
                Resources["refreshStyle"] = Resources["darkThemeRefreshStyle"];
            }
            else
            {
                Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                Resources["refreshStyle"] = Resources["lightThemeRefreshStyle"];
                BackgroundColor = Color.White;
            }

            LoadCalendarSettings();
		}
        
        async void LoadCalendarSettings()
        {
            MainCalendarSettingsStack.Children.Clear();
            //short or full briefing
            StackLayout calendarBriefingStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 30};
            Switch briefingTypeSwitch = new Switch { HorizontalOptions = LayoutOptions.Start, IsToggled = false };
            briefingTypeSwitch.Toggled += BriefingTypeSwitch_Toggled;
            calendarBriefingStack.Children.Add(briefingTypeSwitch);
            Label briefingLabel = new Label { Text = "Use Full Briefing", HorizontalOptions = LayoutOptions.Start, FontSize = 20};
            briefingLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            calendarBriefingStack.Children.Add(briefingLabel);
            MainCalendarSettingsStack.Children.Add(calendarBriefingStack);
            exampleLabel = new Label { Text = "\"You have 8 events today and the first one starts in 30 minutes\"", HorizontalOptions = LayoutOptions.Start, FontSize = 15, TextColor = Color.Gray, FontAttributes = FontAttributes.Italic};
            MainCalendarSettingsStack.Children.Add(exampleLabel);
            if (Application.Current.Properties["fullCalendarBriefing"].ToString() == "t")
            {
                briefingTypeSwitch.IsToggled = true;
                exampleLabel.Text = "\"You have 8 events today. At 8 AM you have school. At 11 AM...\"";
            }
            //calendars
            StackLayout sidewaysTitleStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 30 };
            Label titleLabel = new Label { Text = "Calendars", FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Start };
            titleLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            sidewaysTitleStack.Children.Add(titleLabel);
            Xamarin.Forms.ImageButton refreshButton = new Xamarin.Forms.ImageButton {HorizontalOptions = LayoutOptions.EndAndExpand};
            refreshButton.SetDynamicResource(VisualElement.StyleProperty, "refreshStyle");
            refreshButton.Clicked += RefreshButton_Clicked;
            sidewaysTitleStack.Children.Add(refreshButton);
            MainCalendarSettingsStack.Children.Add(sidewaysTitleStack);
            MainCalendarSettingsStack.Children.Add(new Image { Source = "line.png", HorizontalOptions = LayoutOptions.CenterAndExpand});

            //Get user's calendars
            Task<string[]> calendarsTask = DependencyService.Get<ITextToSpeech>().GetCalendars();
            string[] calendars = await calendarsTask;
            //Get calendars saved
            savedCalendars = Application.Current.Properties["userCalendars"].ToString().Split(',').ToList<string>();

            //Display calendars
            for(int i = 0; i < calendars.Length; i++)
            {
                StackLayout calendarStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0};
                XLabs.Forms.Controls.CheckBox checkBox = new XLabs.Forms.Controls.CheckBox {DefaultText = "", HorizontalOptions = LayoutOptions.Start, Checked = false, ClassId = calendars[i]};
                checkBox.CheckedChanged += CheckBox_CheckedChanged;
                calendarStack.Children.Add(checkBox);
                Label calendarLabel = new Label { Text = calendars[i], FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                calendarLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
                calendarStack.Children.Add(calendarLabel);
                //check if calendar is already selected
                for(int x = 0; x < savedCalendars.Count; x++)
                {
                    if(savedCalendars[x] == calendars[i])
                    {
                        checkBox.Checked = true;
                    }
                }
                MainCalendarSettingsStack.Children.Add(calendarStack);
                if(i < calendars.Length - 1)
                {
                    MainCalendarSettingsStack.Children.Add(new Image { Source = "line.png", HorizontalOptions = LayoutOptions.CenterAndExpand });
                }
            }
            finishedLoading = true;
        }

        private void BriefingTypeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["fullCalendarBriefing"] = "t";
                exampleLabel.Text = "\"You have 8 events today. At 8 AM you have school. At 11 AM...\"";
            }
            else
            {
                Application.Current.Properties["fullCalendarBriefing"] = "f";
                exampleLabel.Text = "\"You have 8 events today and the first one starts in 30 minutes\"";
            }
        }

        private void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            string calendarName = ((XLabs.Forms.Controls.CheckBox)sender).ClassId;
            if (e.Value)
            {
                if(savedCalendars.IndexOf(calendarName) == -1)
                savedCalendars.Add(calendarName);
            }
            else
            {
                savedCalendars.RemoveAll(item => item == calendarName);
            }
            SaveCalendars();
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            LoadCalendarSettings();
        }

        void SaveCalendars()
        {
            string temp = "";
            for(int i = 0; i < savedCalendars.Count; i++)
            {
                temp += savedCalendars[i];
                if(i < savedCalendars.Count - 1 && i > 0)
                {
                    temp += ",";
                }
            }
            Application.Current.Properties["userCalendars"] = temp;
        }
    }
}