using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Briefing
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Travel : ContentPage
	{
        bool finishedLoading = false;
        List<DateTime> timesList = new List<DateTime>();

        public Travel ()
		{
            
            finishedLoading = false;
			InitializeComponent ();

            //theme
            if (Application.Current.Properties["appTheme"].ToString() == "light")
            {
                BackgroundColor = Color.FromHex("#FFFFFF");
                Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                Resources["frameStyle"] = Resources["lightThemeFrameStyle"];
                Resources["editorStyle"] = Resources["lightThemeEditorStyle"];
                Resources["deleteButtonStyle"] = Resources["lightThemeDeleteButtonStyle"];
                Resources["editButtonStyle"] = Resources["lightThemeEditButtonStyle"];
                Resources["timePickerStyle"] = Resources["lightThemeTimePickerStyle"];
            }
            else
            {
                BackgroundColor = Color.FromHex("#3a3a3a");
                Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
                Resources["frameStyle"] = Resources["darkThemeFrameStyle"];
                Resources["editorStyle"] = Resources["darkThemeEditorStyle"];
                Resources["deleteButtonStyle"] = Resources["darkThemeDeleteButtonStyle"];
                Resources["editButtonStyle"] = Resources["darkThemeEditButtonStyle"];
                Resources["timePickerStyle"] = Resources["darkThemeTimePickerStyle"];
            }
            
            LoadDailyCommute();

		}

        void LoadDailyCommute()
        {
            MainTravelStack.Children.Clear();
            Label titleLabel = new Label { Text = "Daily Commute", FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand };
            titleLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            MainTravelStack.Children.Add(titleLabel);
            string[] destinations = Application.Current.Properties["commuteDestinations"].ToString().Split(',');
            if(destinations[0] == "")
            {
                destinations = new string[0];
            }
            string[] names = Application.Current.Properties["commuteNames"].ToString().Split(',');
            if(names[0] == "")
            {
                names = new string[0];
            }
            string[] timeStrings = Application.Current.Properties["commuteTimes"].ToString().Split(',');
            if(timeStrings[0] == "")
            {
                timeStrings = new string[0];
            }
            DateTime[] times = new DateTime[timeStrings.Length];
            for(int i = 0; i < times.Length; i++)
            {
                times[i] = DateTime.ParseExact(timeStrings[i], "yyyy-MM-dd HH:mm tt", CultureInfo.InvariantCulture);
                timesList.Add(DateTime.ParseExact(timeStrings[i], "yyyy-MM-dd HH:mm tt", CultureInfo.InvariantCulture));
            }
            for(int i = 0; i < destinations.Length; i++)
            {
                //create commute bubble
                Frame commuteFrame = new Frame { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, CornerRadius = 20 };
                commuteFrame.SetDynamicResource(VisualElement.StyleProperty, "frameStyle");
                StackLayout mainStack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                commuteFrame.Content = mainStack;
                StackLayout firstHorizontalLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 50, HorizontalOptions = LayoutOptions.FillAndExpand };
                Editor nameEditor = new Editor { Text = names[i], HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 20, ClassId = i.ToString() };
                nameEditor.TextChanged += NameEditor_TextChanged;
                nameEditor.SetDynamicResource(VisualElement.StyleProperty, "editorStyle");
                firstHorizontalLayout.Children.Add(nameEditor);
                ImageButton deleteButton = new ImageButton { Source = "trash.png", HeightRequest = 30, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.CenterAndExpand, ClassId = i.ToString() };
                deleteButton.SetDynamicResource(VisualElement.StyleProperty, "deleteButtonStyle");
                deleteButton.Pressed += DeleteButton_Pressed;
                firstHorizontalLayout.Children.Add(deleteButton);
                mainStack.Children.Add(firstHorizontalLayout);
                Label tempLabel = new Label { Text = "Destination", FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20 };
                tempLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
                mainStack.Children.Add(tempLabel);
                StackLayout secondHorizontalLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand };
                Label destinationLabel = new Label { Text = destinations[i], HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 20 };
                destinationLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
                secondHorizontalLayout.Children.Add(destinationLabel);
                ImageButton editDestinationButton = new ImageButton { HeightRequest = 30, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, ClassId = i.ToString() };
                editDestinationButton.SetDynamicResource(VisualElement.StyleProperty, "editButtonStyle");
                editDestinationButton.Clicked += EditDestinationButton_Clicked;
                secondHorizontalLayout.Children.Add(editDestinationButton);
                mainStack.Children.Add(secondHorizontalLayout);
                Label tempLabel2 = new Label { Text = "Approximate Arrival Time", HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, FontAttributes = FontAttributes.Bold };
                tempLabel2.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
                mainStack.Children.Add(tempLabel2);
                Label subLabel = new Label { Text = "What time do you expect to be at your destination?", FontAttributes = FontAttributes.Italic, FontSize = 15, TextColor = Color.Gray, HorizontalOptions = LayoutOptions.StartAndExpand };
                subLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
                mainStack.Children.Add(subLabel);
                TimePicker timePicker = new TimePicker {Time = new TimeSpan(times[i].Hour, times[i].Minute, times[i].Second), HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, ClassId = i.ToString()};
                timePicker.PropertyChanged += TimePicker_PropertyChanged;
                timePicker.SetDynamicResource(VisualElement.StyleProperty, "timePickerStyle");
                mainStack.Children.Add(timePicker);
                MainTravelStack.Children.Add(commuteFrame);
            }

            if (AddButtonTravelStack.Children.Count == 0)
            {
                //add button
                Button addButton = new Button { Text = "Add Commute", FontSize = 20, TextColor = Color.White, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 50, BackgroundColor = Color.FromHex("006EFF"), CornerRadius = 30 };
                addButton.Clicked += AddButton_Clicked;
                AddButtonTravelStack.Children.Add(addButton);
            }

            finishedLoading = true;
        }

        private void NameEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != e.OldTextValue)
            {
                string[] names = Application.Current.Properties["commuteNames"].ToString().Split(',');
                names[Convert.ToInt32(((Editor)sender).ClassId)] = e.NewTextValue;
            }
        }

        private void TimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Convert.ToInt32(((TimePicker)sender).ClassId) < timesList.Count)
            {
                DateTime currentTime = new DateTime(1, 1, 1, ((TimePicker)sender).Time.Hours, ((TimePicker)sender).Time.Minutes, ((TimePicker)sender).Time.Seconds);
                if (currentTime != timesList[Convert.ToInt32(((TimePicker)sender).ClassId)])
                {
                    //save times
                    timesList[Convert.ToInt32(((TimePicker)sender).ClassId)] = currentTime;
                    string temp = "";
                    for (int i = 0; i < timesList.Count; i++)
                    {
                        temp += timesList[i].ToString("yyyy-MM-dd HH:mm tt");
                        if (i < timesList.Count - 1)
                        {
                            temp += ",";
                        }
                    }
                    Application.Current.Properties["commuteTimes"] = temp;
                }
            }
        }

        private void EditDestinationButton_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteButton_Pressed(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(((Editor)sender).ClassId);
            List<string> destinations = Application.Current.Properties["commuteDestinations"].ToString().Split(',').ToList();
            List<string> names = Application.Current.Properties["commuteNames"].ToString().Split(',').ToList();
            List<string> times = Application.Current.Properties["commuteTimes"].ToString().Split(',').ToList();
            destinations.RemoveAt(index);
            names.RemoveAt(index);
            times.RemoveAt(index);
            string destinationsString = "";
            string namesString = "";
            string timesString = "";
            for(int i = 0; i < destinations.Count; i++)
            {
                destinationsString += destinations[i];
                namesString += names[i];
                timesString += times[i];
                if(i < destinations.Count - 1)
                {
                    destinationsString += ",";
                    namesString += ",";
                    timesString += ",";
                }
            }
            Application.Current.Properties["commuteDestinations"] = destinationsString;
            Application.Current.Properties["commuteNames"] = namesString;
            Application.Current.Properties["commuteTimes"] = timesString;
            DisplayAlert((index + 1).ToString() + ", " + MainTravelStack.Children.Count.ToString(), "", "OK");
            //MainTravelStack.Children.RemoveAt(index + 1);
            LoadDailyCommute();
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            //add new empty commute
            List<string> destinations = Application.Current.Properties["commuteDestinations"].ToString().Split(',').ToList();
            List<string> names = Application.Current.Properties["commuteNames"].ToString().Split(',').ToList();
            List<string> timeStrings = Application.Current.Properties["commuteTimes"].ToString().Split(',').ToList();
            if (Application.Current.Properties["commuteDestinations"].ToString().Replace(" ", "") == "")
            {
                destinations = new List<string>();
                names = new List<string>();
                timeStrings = new List<string>();
            }
            destinations.Add("No Destination");
            names.Add("New Commute");
            timeStrings.Add(new DateTime(1, 1, 1, 12, 0, 0).ToString("yyyy-MM-dd HH:mm tt"));
            timesList.Add(new DateTime(1, 1, 1, 12, 0, 0));
            string destinationsString = "";
            string namesString = "";
            string timeStringsString = "";
            for(int i = 0; i < destinations.Count; i++)
            {
                if(timeStrings[i].Replace(" ", "") == "")
                {
                    continue;
                }
                destinationsString += destinations[i];
                namesString += names[i];
                timeStringsString += timeStrings[i];
                if(i < destinations.Count - 1)
                {
                    destinationsString += ",";
                    namesString += ",";
                    timeStringsString += ",";
                }
            }
            DisplayAlert(destinationsString, "", "OK");
            Application.Current.Properties["commuteDestinations"] = destinationsString;
            Application.Current.Properties["commuteNames"] = namesString;
            Application.Current.Properties["commuteTimes"] = timeStringsString;
            timesList.Add(new DateTime(1, 1, 1, 0, 0, 0));
            Frame commuteFrame = new Frame { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, CornerRadius = 20 };
            commuteFrame.SetDynamicResource(VisualElement.StyleProperty, "frameStyle");
            StackLayout mainStack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand};
            commuteFrame.Content = mainStack;
            StackLayout firstHorizontalLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 50, HorizontalOptions = LayoutOptions.FillAndExpand };
            Editor nameEditor = new Editor { Text = "New Commute", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 20 };
            nameEditor.TextChanged += DeleteButton_Pressed;
            nameEditor.SetDynamicResource(VisualElement.StyleProperty, "editorStyle");
            firstHorizontalLayout.Children.Add(nameEditor);
            ImageButton deleteButton = new ImageButton { HeightRequest = 30, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.CenterAndExpand, ClassId = (destinations.Count - 1).ToString() };
            deleteButton.SetDynamicResource(VisualElement.StyleProperty, "deleteButtonStyle");
            deleteButton.Pressed += DeleteButton_Pressed;
            firstHorizontalLayout.Children.Add(deleteButton);
            mainStack.Children.Add(firstHorizontalLayout);
            Label tempLabel = new Label { Text = "Destination", FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20 };
            tempLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            mainStack.Children.Add(tempLabel);
            StackLayout secondHorizontalLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand };
            Label destinationLabel = new Label { Text = "No Destination", HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 20 };
            destinationLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            secondHorizontalLayout.Children.Add(destinationLabel);
            ImageButton editDestinationButton = new ImageButton { HeightRequest = 30, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, ClassId = (destinations.Count - 1).ToString() };
            //editDestinationButton.SetDynamicResource(VisualElement.StyleProperty, "editButtonStyle");
            editDestinationButton.Clicked += EditDestinationButton_Clicked;
            secondHorizontalLayout.Children.Add(editDestinationButton);
            mainStack.Children.Add(secondHorizontalLayout);
            Label tempLabel2 = new Label { Text = "Approximate Arrival Time", HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, FontAttributes = FontAttributes.Bold};
            tempLabel2.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            mainStack.Children.Add(tempLabel2);
            Label subLabel = new Label { Text = "What time do you expect to be at your destination?", FontAttributes = FontAttributes.Italic, FontSize = 15, TextColor = Color.Gray, HorizontalOptions = LayoutOptions.StartAndExpand };
            subLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            mainStack.Children.Add(subLabel);
            TimePicker timePicker = new TimePicker { HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, Time = new TimeSpan(12, 0, 0), ClassId = (destinations.Count - 1).ToString()};
            timePicker.PropertyChanged += TimePicker_PropertyChanged;
            timePicker.SetDynamicResource(VisualElement.StyleProperty, "timePickerStyle");
            mainStack.Children.Add(timePicker);
            MainTravelStack.Children.Add(commuteFrame);
        }
    }
}