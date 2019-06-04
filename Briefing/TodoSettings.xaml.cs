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
	public partial class TodoSettings : ContentPage
	{
        bool finishedLoading = false;
        Button loginButton;
		public TodoSettings ()
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

            //Create sign in button
            loginButton = new Button { Text = "Log In to Todoist", HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 18, BackgroundColor = Color.FromHex("FF0000"), TextColor = Color.White, HeightRequest = 60, CornerRadius = 20};
            loginButton.Clicked += LoginButton_Clicked;
            if(Application.Current.Properties["todoistLogin"].ToString() == "t")
            {
                loginButton.Text = "Logout of Todoist";
            }
            MainTodoSettingsStack.Children.Add(loginButton);
            StackLayout horizontalStack = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 40 };
            CheckBox check = new CheckBox { HorizontalOptions = LayoutOptions.Start, Checked = false, VerticalOptions = LayoutOptions.CenterAndExpand};
            check.CheckedChanged += Check_CheckedChanged;
            if(Application.Current.Properties["showFullTodo"].ToString() == "t"){
                check.Checked = true;
            }
            horizontalStack.Children.Add(check);
            Label tempLabel = new Label { Text = "Show individual tasks", HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand};
            tempLabel.SetDynamicResource(VisualElement.StyleProperty, "labelStyle");
            horizontalStack.Children.Add(tempLabel);
            MainTodoSettingsStack.Children.Add(horizontalStack);
            
            finishedLoading = true;
        }

        private void Check_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (e.Value)
            {
                Application.Current.Properties["showFullTodo"] = "t";
            }
            else
            {
                Application.Current.Properties["showFullTodo"] = "f";
            }
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (finishedLoading) { Application.Current.Properties["settingsChanged"] = "t"; }
            if (Application.Current.Properties["todoistLogin"].ToString() == "f")
            {
                DependencyService.Get<ITextToSpeech>().TodoistLogin();
                loginButton.Text = "Logout of Todoist";
                Application.Current.Properties["todoistLogin"] = "t";
            }
            else
            {
                loginButton.Text = "Login to Todoist";
                Application.Current.Properties["todoistLogin"] = "f";
            }
        }
    }
}