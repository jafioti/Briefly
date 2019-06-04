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
	public partial class NewsSettings : ContentPage
	{
        bool finishedLoading = false;
        string[] newsSources = { "Axios", "BBC_News", "BBC_Sport", "Bleacher_Report", "Bloomberg", "Breitbart_News", "Business_Insider", "Business_Insider_UK", "Buzzfeed", "CBC_News", "CNBC", "CNN", "Crypto_Coins_News", "Daily_Mail",
            "Engadget", "Entertainment_Weekly", "ESPN", "Financial_Post", "Financial_Times", "Fortune", "Fox_News", "Google_News", "Hacker_News", "IGN", "Independent", "Mashable", "Medical_News_Today", "Metro", "Mirror",
            "MSNBC", "MTV_News", "MTV_News_UK", "National_Geographic", "National_Review", "NBC_News", "New_Scientist", "Newsweek", "New_York_Magazine", "Next_Big_Future", "NFL_News", "NHL_News", "Politico", "Polygon", "Recode", "TalkSport",
            "TechCrunch", "TechRadar", "The_American_Conservative", "The_Economist", "The_Globe_And_Mail", "The_Hill", "The_Huffington_Post", "The_Irish_Times", "The_Jerusalem_Post", "The_Lad_Bible", "The_New_York_Times", "The_Next_Web",
            "The_Sport_Bible", "The_Telegraph", "The_Verge", "The_Wall_Street_Journal", "The_Washington_Post", "The_Washington_Times", "Time", "USA_Today", "Vice_News", "Wired"};

        public NewsSettings ()
		{
            finishedLoading = false;
			InitializeComponent ();

            if (Application.Current.Properties["appTheme"].ToString() == "dark") {
                Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
            }
            else
            {
                Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                BackgroundColor = Color.White;
            }
            Label title = new Label { Text = "Sources", VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = 20, FontAttributes = FontAttributes.Bold };
            title.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            MainNewsSettingsStack.Children.Add(title);
            CreateNewsSourceControls();
		}

        void CreateNewsSourceControls()
        {
            //loop through all news sources and add them to the list
            for (int i = 0; i < newsSources.Length; i++)
            {
                StackLayout stackLayout = new StackLayout { Spacing = 10, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                CheckBox onSwitch = new CheckBox { WidthRequest = 40, ClassId = i.ToString(), HorizontalOptions = LayoutOptions.Start, Checked = Convert.ToBoolean((string)Application.Current.Properties[newsSources[i]]) };
                onSwitch.CheckedChanged += OnSwitch_CheckedChanged;
                Label newsLabel = new Label { Text = newsSources[i].Replace("_", " "), VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20 };
                stackLayout.Children.Add(onSwitch);
                stackLayout.Children.Add(newsLabel);
                MainNewsSettingsStack.Children.Add(stackLayout);
                newsLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            }
            finishedLoading = true;
        }

        private void OnSwitch_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            CheckBox onSwitch = (CheckBox)sender;
            Application.Current.Properties[newsSources[Convert.ToInt32(onSwitch.ClassId)]] = e.Value.ToString();
        }
    }
}