using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace Briefing
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StocksSettings : ContentPage
	{
        CheckBox stockChangePercentCheckBox;
        bool finishedLoading = false;
        public StocksSettings ()
		{
            finishedLoading = false;
			InitializeComponent ();

            if (Application.Current.Properties["appTheme"].ToString() == "dark")
            {
                Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
                Resources["buttonStyle"] = Resources["darkThemeButtonStyle"];
            }
            else
            {
                Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                Resources["buttonStyle"] = Resources["lightThemeButtonStyle"];
                BackgroundColor = Color.White;
            }

            StackLayout tempStack = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 30, Padding = 5 };
            CheckBox stockChangeCheckBox = new CheckBox { Checked = false, HorizontalOptions = LayoutOptions.Start };
            if (Application.Current.Properties["stocksChange"].ToString() == "t")
            {
                stockChangeCheckBox.Checked = true;
            }
            stockChangeCheckBox.CheckedChanged += StockChangeCheckBox_CheckedChanged;
            tempStack.Children.Add(stockChangeCheckBox);
            Label stockChangeLabel = new Label { Text = "Show stock changes", HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand, FontAttributes = FontAttributes.Bold };
            stockChangeLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            tempStack.Children.Add(stockChangeLabel);
            FullStockSettingsStack.Children.Add(tempStack);
            StackLayout tempPercentStack = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 30, Padding = 5 };
            stockChangePercentCheckBox = new CheckBox { Checked = false, HorizontalOptions = LayoutOptions.Start, IsEnabled = false};
            if (Application.Current.Properties["stocksChange"].ToString() == "t")
            {
                stockChangePercentCheckBox.IsEnabled = true;
            }
            if (Application.Current.Properties["stocksPercentChange"].ToString() == "t")
            {
                stockChangePercentCheckBox.Checked = true;
            }
            stockChangePercentCheckBox.CheckedChanged += StockChangePercentCheckBox_CheckedChanged;
            tempPercentStack.Children.Add(stockChangePercentCheckBox);
            Label stockChangePercentLabel = new Label { Text = "Show stock changes in percents", HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand, FontAttributes = FontAttributes.Bold };
            stockChangePercentLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            tempPercentStack.Children.Add(stockChangePercentLabel);
            FullStockSettingsStack.Children.Add(tempPercentStack);
            LoadStocks();
		}

        private void StockChangePercentCheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["stocksPercentChange"] = "t";
            }
            else
            {
                Application.Current.Properties["stocksPercentChange"] = "f";
            }
        }

        void LoadStocks()
        {
            MainStockSettingsStack.Children.Clear();
            string[] stocks = Application.Current.Properties["stocks"].ToString().Split(',');
            if(Application.Current.Properties["stocks"].ToString().Replace(" ", "") == "")
            {
                stocks = new string[0];
            }
            for(int i = 0; i < stocks.Length; i++)
            {
                StackLayout stockStack = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand};
                Label stockLabel = new Label { Text = stocks[i].ToUpper(), FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontAttributes = FontAttributes.Bold};
                stockLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
                stockStack.Children.Add(stockLabel);
                Xamarin.Forms.ImageButton removeButton = new Xamarin.Forms.ImageButton { Source = "trash.png", HorizontalOptions = LayoutOptions.EndAndExpand, ClassId = stocks[i], HeightRequest = 30};
                removeButton.SetDynamicResource(VisualElement.StyleProperty, "buttonStyle");
                removeButton.Clicked += RemoveButton_Clicked;
                stockStack.Children.Add(removeButton);
                MainStockSettingsStack.Children.Add(stockStack);

                if(i < stocks.Length - 1)
                {
                    MainStockSettingsStack.Children.Add(new Image { Source = "/drawable/line.png" });
                }
            }
            finishedLoading = true;
        }

        private void StockChangeCheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["stocksChange"] = "t";
                stockChangePercentCheckBox.IsEnabled = true;
            }
            else
            {
                Application.Current.Properties["stocksChange"] = "f";
                stockChangePercentCheckBox.IsEnabled = false;
            }
        }

        //remove stock
        private void RemoveButton_Clicked(object sender, EventArgs e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            List<string> stocks = Application.Current.Properties["stocks"].ToString().Split(',').ToList();
            stocks.Remove(((Xamarin.Forms.ImageButton)sender).ClassId.ToString().ToLower());
            string temp = "";
            for(int i = 0; i < stocks.Count; i++)
            {
                temp += stocks[i];
                if(i < stocks.Count - 1)
                {
                    temp += ",";
                }
            }
            Application.Current.Properties["stocks"] = temp;
            LoadStocks();
        }

        public async void AddStock(object sender, EventArgs args)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            //check if stock exists
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://cloud.iexapis.com/stable/stock/" + AddStocksEditor.Text + "/quote?token=pk_f51846cce15f403fa11f3b676aaf111b");
            string result = await response.Content.ReadAsStringAsync();
            if (!result.Contains("Unknown symbol") && !AddStocksEditor.Text.Contains("/"))
            {
                if (("," + Application.Current.Properties["stocks"].ToString() + ",").Contains("," + AddStocksEditor.Text + ","))
                {
                    DisplayAlert("Already Added", AddStocksEditor.Text.ToUpper() + " has already been added.", "OK");
                    return;
                }
                if (Application.Current.Properties["stocks"].ToString().Replace(" ", "") == "")
                {
                    Application.Current.Properties["stocks"] = AddStocksEditor.Text.ToLower();
                }
                else
                {
                    Application.Current.Properties["stocks"] += "," + AddStocksEditor.Text;
                }
            }
            else
            {
                DisplayAlert("Not Found", AddStocksEditor.Text.ToUpper() + " has not been found.", "OK");
                return;
            }
            AddStocksEditor.Text = "";
            LoadStocks();
        }
	}
}