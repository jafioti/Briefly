using System;
using Android.Content;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech.Tts;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Plugin.GoogleClient;
using Google.Apis;
using Google.Apis.Calendar.v3;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Android;
using Xamarin.Auth;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Android.Media;

namespace Briefing.Droid
{
    [Activity(Name = "com.companyname.Briefing.MainActivity", Label = "Briefing", Icon = "@drawable/clipboard", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, TextToSpeech.IOnInitListener, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        public static MediaPlayer player;
        public static TextToSpeech speaker;
        public static int currentUtterance = 0;
        public static int prevUtterance = 0;
        public static int currentLength = 0;
        public static GoogleApiClient client;
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        public static string exceptionString = "";
        public static Account currentAccount;
        public static Account todoistAccount;
        public static OAuth2Authenticator authenticator = new OAuth2Authenticator("868276222444-rk9uq2o9n199nfkvp6viams3pqgt6sf6.apps.googleusercontent.com", null, "https://www.googleapis.com/auth/calendar.events https://www.googleapis.com/auth/gmail.readonly https://www.googleapis.com/auth/calendar.readonly", new Uri("https://accounts.google.com/o/oauth2/auth"), new Uri("com.googleusercontent.apps.868276222444-rk9uq2o9n199nfkvp6viams3pqgt6sf6:/oauth2redirect"), new Uri("https://accounts.google.com/o/oauth2/token"), null, true);
        public static OAuth2Authenticator todoistAuthenticator = new OAuth2Authenticator("745c8a61f81949ad897aa0d4c1b218da", "7e97e98c6b06492d85e880336873b542", "data:read", new Uri("https://todoist.com/oauth/authorize"), new Uri("briefing://oauth.redirect/"), new Uri("https://todoist.com/oauth/access_token"), null, true);
        public static string errorString = "";

        internal static MainActivity Instance { get; private set; } 
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //var uri = new Uri(Intent.Data.ToString());
            global::Xamarin.Forms.Forms.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            LoadApplication(new App());
            CheckAppPermissions();
            SetupSpeaker();
            authenticator.Completed += Authenticator_Completed;
            authenticator.Error += Authenticator_Error;
            todoistAuthenticator.Completed += TodoistAuthenticator_Completed;
            todoistAuthenticator.Error += TodoistAuthenticator_Error;
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            TryLoadAccounts();
        }

        public static void InitializeMediaManager()
        {
            
        }

        private void TodoistAuthenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            errorString = e.Message;
            Toast.MakeText(this, "Error", ToastLength.Long).Show();
        }

        private async void TodoistAuthenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                AccountStore.Create(Android.App.Application.Context, "briefingPassword").Save(e.Account, "com.companyname.Briefing.todoist");
                todoistAccount = e.Account;
            }
            else
            {
                errorString = "error";
            }
        }

        void TryLoadAccounts()
        {
            Account account =  AccountStore.Create(Android.App.Application.Context, "briefingPassword").FindAccountsForService("com.companyname.Briefing").FirstOrDefault();
            Account tempTodoistAccount =  AccountStore.Create(Android.App.Application.Context, "briefingPassword").FindAccountsForService("com.companyname.Briefing.todoist").FirstOrDefault();
            if(account != null)
            {
                exceptionString = account.Properties["access_token"];
                currentAccount = account;
            }
            else
            {
                exceptionString = "FAILED";
            }
            if(tempTodoistAccount != null)
            {
                todoistAccount = tempTodoistAccount;
            }
        }

        public static void LogoutGoogle()
        {
            MainActivity.currentAccount = null;
        }

        public static void LogoutTodoist()
        {
            MainActivity.todoistAccount = null;
        }

        public static void CallAuth()
        {
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        public static void CallTodoistAuth()
        {
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(todoistAuthenticator);
        }

        private void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            exceptionString = "ERROR";
        }

        private async void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                AccountStore.Create(Android.App.Application.Context, "briefingPassword").Save(e.Account, "com.companyname.Briefing");
                exceptionString = e.Account.Properties["refresh_token"];
                currentAccount = e.Account;
                var request = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    string userJson = response.GetResponseText();
                    //exceptionString = userJson;
                }
                else
                {
                    exceptionString = "";
                }
            }
            else
            {
                exceptionString = "NOT AUTH";
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void SetupGoogleSignIn()
        {
            try
            {
                UserCredential credential;
                var assembly = typeof(MainActivity).GetTypeInfo().Assembly;
                using (var stream = assembly.GetManifestResourceStream("Briefing.Droid.credentials.json"))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "token.json");
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }
            }catch (AggregateException e)
            {
                foreach(var inner in e.InnerExceptions)
                {
                    exceptionString += "EXCEPTION: " + inner.ToString();
                }
                Toast.MakeText(this, exceptionString, ToastLength.Long).Show();
            }
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //GoogleClientManager.OnAuthCompleted(requestCode, resultCode, data);
            if(requestCode == 9001)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount accountDetails = result.SignInAccount;
                    Toast.MakeText(this, accountDetails.IdToken, ToastLength.Long).Show();
                }
            }
        }

        #region speech
        public void SetupSpeaker()
        {
            speaker = new TextToSpeech(this, this);
            speaker.SetOnUtteranceProgressListener(new utteranceProgressListener(this));
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {

        }

        public void SpeechDone(string utteranceID)
        {
            currentUtterance = Convert.ToInt32(utteranceID) + 1;
            if(currentUtterance >= currentLength)
            {
                currentUtterance = 0;
                prevUtterance = 0;
            }
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            //throw new NotImplementedException();
        }

        public void OnConnected(Bundle connectionHint)
        {
            //throw new NotImplementedException();
        }

        public void OnConnectionSuspended(int cause)
        {
            //throw new NotImplementedException();
        }
        #endregion

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    || PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted
                    || PackageManager.CheckPermission(Manifest.Permission.AccessCoarseLocation, PackageName) != Permission.Granted
                    || PackageManager.CheckPermission(Manifest.Permission.AccessFineLocation, PackageName) != Permission.Granted
                    || PackageManager.CheckPermission(Manifest.Permission.RecordAudio, PackageName) != Permission.Granted
                    || PackageManager.CheckPermission(Manifest.Permission.Vibrate, PackageName) != Permission.Granted
                    || PackageManager.CheckPermission(Manifest.Permission.WakeLock, PackageName) != Permission.Granted
                    || PackageManager.CheckPermission(Manifest.Permission.MediaContentControl, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.MediaContentControl, Manifest.Permission.WakeLock, Manifest.Permission.Vibrate, Manifest.Permission.RecordAudio, Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage, Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation, Manifest.Permission.PersistentActivity};
                    RequestPermissions(permissions, 1);
                }
            }
        }

    public class utteranceProgressListener : UtteranceProgressListener
    {
        MainActivity _parent;

        public utteranceProgressListener(MainActivity p_parent)
        {
            _parent = p_parent;
        }

        public override void OnStart(String utteranceId)
        {
            Console.WriteLine("OnStart called");
        }

        public override void OnError(String utteranceId)
        {
            Console.WriteLine("OnError called");
        }

        public override void OnDone(String utteranceId)
        {
            Console.WriteLine("UTTERANCE: " + utteranceId);
            _parent.SpeechDone(utteranceId);
        }


    }
}
}

