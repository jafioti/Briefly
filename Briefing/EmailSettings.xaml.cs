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
	public partial class EmailSettings : ContentPage
	{
        bool finishedLoading = false;
		public EmailSettings ()
		{
            finishedLoading = false;
			InitializeComponent ();

            if (Application.Current.Properties["appTheme"].ToString() == "dark")
            {
                Resources["labelStyle"] = Resources["darkThemeLabelStyle"];
            }
            else
            {
                Resources["labelStyle"] = Resources["lightThemeLabelStyle"];
                BackgroundColor = Color.White;
            }

            MakeControls();
		}
        CheckBox useImportantEmailsCheckBox;
        CheckBox readImportantSenders;
        CheckBox readAllSenders;

        void MakeControls()
        {
            StackLayout stack1 = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, HeightRequest = 40, Orientation = StackOrientation.Horizontal};
            useImportantEmailsCheckBox = new CheckBox {DefaultText = "", TextColor = Color.White, FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand };
            useImportantEmailsCheckBox.CheckedChanged += UseImportantEmailsCheckBox_CheckedChanged;
            stack1.Children.Add(useImportantEmailsCheckBox);
            Label label1 = new Label { Text = "Use only important emails", FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand };
            label1.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            stack1.Children.Add(label1);
            StackLayout stack2 = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, HeightRequest = 40, Orientation = StackOrientation.Horizontal };
            readImportantSenders = new CheckBox { DefaultText = "", TextColor = Color.White, FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand };
            readImportantSenders.CheckedChanged += ReadImportantSenders_CheckedChanged;
            stack2.Children.Add(readImportantSenders);
            Label label2 = new Label { Text = "Read out important senders", FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand };
            label2.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            stack2.Children.Add(label2);
            StackLayout stack3 = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, HeightRequest = 40, Orientation = StackOrientation.Horizontal };
            readAllSenders = new CheckBox { DefaultText = "", TextColor = Color.White, FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand };
            readAllSenders.CheckedChanged += ReadAllSenders_CheckedChanged;
            stack3.Children.Add(readAllSenders);
            Label label3 = new Label { Text = "Read out all senders", FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand };
            label3.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            stack3.Children.Add(label3);

            //fil in proper values
            if (Application.Current.Properties["readAllSenders"].ToString() == "t")
            {
                readAllSenders.Checked = true;
            }
            if(Application.Current.Properties["readImportantSenders"].ToString() == "t")
            {
                readImportantSenders.Checked = true;
            }
            if (Application.Current.Properties["useImportantEmails"].ToString() == "t")
            {
                useImportantEmailsCheckBox.Checked = true;
                readAllSenders.IsEnabled = false;
            }
            else{
                readImportantSenders.IsEnabled = false;
            }
            MainEmailSettingsStack.Children.Add(stack1);
            MainEmailSettingsStack.Children.Add(stack2);
            MainEmailSettingsStack.Children.Add(stack3);
            finishedLoading = true;
        }

        private void ReadAllSenders_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["readAllSenders"] = "t";
            }
            else
            {
                Application.Current.Properties["readAllSenders"] = "f";
            }
        }

        private void ReadImportantSenders_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["readImportantSenders"] = "t";
            }
            else
            {
                Application.Current.Properties["readImportantSenders"] = "f";
            }
        }

        private void UseImportantEmailsCheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["useImportantEmails"] = "t";
                readAllSenders.IsEnabled = false;
                readImportantSenders.IsEnabled = true;
            }
            else
            {
                Application.Current.Properties["useImportantEmails"] = "f";
                readAllSenders.IsEnabled = true;
                readImportantSenders.IsEnabled = false;
            }
        }
    }
}