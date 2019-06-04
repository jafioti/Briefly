using System;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using Android.Support.V4;
using Xamarin.Forms;
using Android.Provider;
using Android.Speech.Tts;
using Xamarin.Auth;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Android.Media;
using Java.IO;
using Android.App;
using Android.Content;
using System.IO;
using Android.OS;
using System.Globalization;

[assembly: Dependency(typeof(Briefing.Droid.AndroidDependencyService))]
namespace Briefing.Droid
{
    public class AndroidDependencyService : ITextToSpeech
    {
        
        public void MakeToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        bool isPaused = false;

        public void SpeakText(string[] message)
        {
            if (MainActivity.speaker != null)
            {
                MainActivity.currentLength = message.Length - 1;
                for (int i = 0; i < message.Length; i++)
                {
                    MainActivity.speaker.Speak(message[i], QueueMode.Add, null, i.ToString());
                }
            }
        }

        void CreateNotification()
        {
            NotificationManager manager = Android.App.Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            var builder = new Notification.Builder(Android.App.Application.Context);
            builder.SetContentTitle("Test Notification");
            builder.SetContentText("This is the body");
            builder.SetAutoCancel(true);
            builder.SetSmallIcon(Resource.Drawable.clipboard);
            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelId = "com.companyname.briefing.general";
                var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);

                manager.CreateNotificationChannel(channel);

                builder.SetChannelId(channelId);
            }
            var resultIntent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(Android.App.Application.Context.PackageName);
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Android.App.Application.Context);
            stackBuilder.AddNextIntent(resultIntent);
            var resultPendingIntent =
                stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
            builder.SetContentIntent(resultPendingIntent);

            manager.Notify(0, builder.Build());
        }
        
        private void Player_Completion(object sender, EventArgs e)
        {
            isPaused = false;
        }

        public void SynthText(string message)
        {
            if (MainActivity.speaker != null)
            {
                //Check if folder exists
                if (!Directory.Exists("/storage/emulated/0/briefing"))
                {
                    Directory.CreateDirectory("/storage/emulated/0/briefing");
                }
                MainActivity.speaker.SynthesizeToFile(message, new Dictionary<string, string>(), "/storage/emulated/0/briefing/briefing.mp3");
            }
            MainActivity.InitializeMediaManager();
        }

        public int StopSpeaking()
        {
            MainActivity.speaker.Stop();
            int returnNum = MainActivity.currentUtterance + MainActivity.prevUtterance;
            MainActivity.prevUtterance += MainActivity.currentUtterance;
            MainActivity.currentUtterance = 0;
            return (returnNum);
        }

        public void ResetCurrentUtterance()
        {
            MainActivity.currentUtterance = 0;
        }

        public void Login()
        {
            if (MainActivity.currentAccount != null)
            {
                if (MainActivity.currentAccount.Username.Replace(" ", "") == "")
                {
                    MainActivity.CallAuth();
                }
            }
            else
            {
                MainActivity.CallAuth();
            }
        }

        public void TodoistLogin()
        {
            if (MainActivity.todoistAccount != null)
            {
                if (MainActivity.todoistAccount.Username.Replace(" ", "") == "")
                {
                    MainActivity.CallTodoistAuth();
                }
            }
            else
            {
                MainActivity.CallTodoistAuth();
            }
        }

        public async Task<string[]> GetCalendars()
        {
            try
            {
                if (MainActivity.currentAccount != null)
                {
                    //Get user's first calendar
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainActivity.currentAccount.Properties["access_token"]);
                    HttpResponseMessage response = await client.GetAsync("https://www.googleapis.com/calendar/v3/users/me/calendarList");
                    string body = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(body);
                    JArray jsonArray = JArray.Parse(json["items"].ToString());
                    List<string> calendarId = new List<string>();
                    if (json["items"].Count() > 0)
                    {
                        for (int i = 0; i < jsonArray.Count(); i++)
                        {
                            //we only want actual calendars, not ones that google adds in for holidays and contacts, # seemes to be a good identifier
                            if (!jsonArray[i]["id"].ToString().Contains("#") && jsonArray[i]["id"].ToString().Length > 0)
                            {
                                calendarId.Add(jsonArray[i]["id"].ToString());
                            }
                        }
                        return (calendarId.ToArray());
                    }
                    else
                    {
                        return (new string[0]);
                    }
                }
                else
                {
                    //Not loggged in
                    return (new string[0]);
                }
            }
            catch(Exception e)
            {
                //failed
                return (new string[0]);
            }
        }

        public async Task<string[][]> GetCalendarEvents(string[] calendars)
        {
            try
            {
                if (MainActivity.currentAccount != null)
                {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainActivity.currentAccount.Properties["access_token"]);
                        //get calendar events
                        HttpResponseMessage response = await client.GetAsync("https://www.googleapis.com/calendar/v3/calendars/" + calendars[0] + "/events?maxResults=30&orderBy=startTime&singleEvents=true&timeMax=" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "T23:59:59-07:00&timeMin=" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "T01:00:00-07:00&timeZone=EST");
                        string body = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(body);
                        JArray jsonArray = JArray.Parse(json["items"].ToString());
                        string[][] events = new string[jsonArray.Count()][];
                        for (int i = 0; i < jsonArray.Count(); i++)
                        {
                            events[i] = new string[3];
                            events[i][0] = jsonArray[i]["summary"].ToString();
                            try
                            {
                                events[i][1] = jsonArray[i]["start"]["dateTime"].ToString();
                            }
                            catch
                            {
                                events[i][1] = "";
                            }
                            events[i][2] = jsonArray[i]["htmlLink"].ToString();
                        }
                        return (events);

                }
                else
                {
                    return (new string[][] { new string[] {"No account" } });
                }
            }
            catch (Exception e)
            {
                return(new string[][] { new string[] { e.Message } });
            }
        }

        public bool GetAccount()
        {
            if (MainActivity.currentAccount != null)
            {
                return (true);
            }
            return (false);
        }

        public async Task<string[]> GetUnreadEmails(string query)
        {
            try
            {
                if (MainActivity.currentAccount != null)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainActivity.currentAccount.Properties["access_token"]);
                    //get calendar events
                    HttpResponseMessage response;
                    if (query.Contains("all"))
                    {
                        response = await client.GetAsync("https://www.googleapis.com/gmail/v1/users/me/messages?labelIds=INBOX&q=is:unread");
                    }
                    else
                    {
                        response = await client.GetAsync("https://www.googleapis.com/gmail/v1/users/me/messages?labelIds=INBOX&q=is:unread%20is:important");
                    }
                    string body = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(body);
                    if (json["resultSizeEstimate"].ToString() != "0")
                    {
                        JArray jsonArray = JArray.Parse(json["messages"].ToString());
                        if (query.Contains("num"))
                        {
                            return (new string[Convert.ToInt32(json["resultSizeEstimate"].ToString())]);
                        }
                        else
                        {
                            //get senders of individual messages
                            int max = Convert.ToInt32(json["resultSizeEstimate"].ToString());
                            if (max > 5) { max = 5; }
                            string[] senders = new string[max];
                            for(int i = 0; i < max; i++)
                            {
                                response = await client.GetAsync("https://www.googleapis.com/gmail/v1/users/me/messages/" + jsonArray[i]["id"].ToString());
                                body = await response.Content.ReadAsStringAsync();
                                json = JObject.Parse(body);
                                JArray headers = JArray.Parse(json["payload"]["headers"].ToString());
                                for(int x = 0; x < headers.Count(); x++)
                                {
                                    if (headers[x]["name"].ToString() == "From")
                                    {
                                        senders[i] = headers[x]["value"].ToString().Substring(0, headers[x]["value"].ToString().IndexOf('<')).Trim();
                                        break;
                                    }
                                }
                            }
                            return (senders);
                        }
                    }
                    else
                    {
                        return (new string[0]);
                    }
                }
                else
                {
                    return (new string[0]);
                }
            }
            catch(Exception e)
            {
                return (new string[0]);
            }
        }

        public async Task<string> RefreshAccessToken()
        {
            if (MainActivity.currentAccount != null)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync("https://www.googleapis.com/oauth2/v4/token?client_id=868276222444-rk9uq2o9n199nfkvp6viams3pqgt6sf6.apps.googleusercontent.com&refresh_token=" + MainActivity.currentAccount.Properties["refresh_token"] + "&grant_type=refresh_token", null);
                string body = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(body);
                MainActivity.currentAccount.Properties["access_token"] = json["access_token"].ToString();
                AccountStore.Create(Android.App.Application.Context, "briefingPassword").Save(MainActivity.currentAccount, "com.companyname.Briefing");
                return (json["expires_in"].ToString());
            }
            else
            {
                return ("noAccount");
            }
        }

        public async Task<string[][]> GetTodoistTasks()
        {
            try
            {
                if (MainActivity.todoistAccount != null)
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MainActivity.todoistAccount.Properties["access_token"]);
                    //get calendar events
                    HttpResponseMessage response = await client.GetAsync("https://beta.todoist.com/API/v8/tasks");
                    string result = await response.Content.ReadAsStringAsync();
                    JArray taskList = JArray.Parse(result);
                    List<string[]> tasks = new List<string[]>();
                    for(int i = 0; i < taskList.Count; i++)
                    {
                        if(DateTime.ParseExact(taskList[i]["due"]["date"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.Now)
                        {
                            tasks.Add(new string[] { taskList[i]["content"].ToString(), taskList[i]["url"].ToString()});
                        }
                    }
                    return (tasks.ToArray());
                }
                else
                {
                    return (new string[][] { new string[] { "no account" } });
                }
            }
            catch
            {
                return (new string[][] { new string[] { "error" } });
            }
        }

        //constructor
        public AndroidDependencyService()
        {

        }
    }
}
