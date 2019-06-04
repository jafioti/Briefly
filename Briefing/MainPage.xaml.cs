using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Refractored.XamForms.PullToRefresh;
using Xamarin.Auth;
using System.Globalization;
using Plugin.Geolocator;
using XLabs.Forms.Controls;
using Plugin.Connectivity;

namespace Briefing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
	{
        string[] newsSourceColors = { "129AE2", "B91F1F", "B91F1F", "000000", "000000", "FF5600", "1E5F7B", "1E5F7B", "ED3627", "E73F48", "005693", "CB0000", "000000", "004EB1",
            "000000", "40B6E9", "DC0000", "314FBD", "000000", "000000", "003666", "000000", "FF6600", "BD1A1A", "E21528", "00ACEE", "10A6D4", "09C33D", "E81616",
            "3361FF", "F8F806", "F8F806", "FFCC0D", "000000", "000000", "00B1E4", "F72618", "000000", "487FBC", "063669", "000000", "DB092C", "962B82", "ED3028", "FFF100",
            "17CB33", "326E90", "2C3774", "E21913", "D91C24", "2E54C0", "15BC97", "000000", "BA1A20", "000000", "000000", "FF4413",
            "72B77C", "000000", "3C3391", "000000", "000000", "A71124", "E80E0E", "069AFF", "000000", "000000"};
        string[] newsSources = { "Axios", "BBC_News", "BBC_Sport", "Bleacher_Report", "Bloomberg", "Breitbart_News", "Business_Insider", "Business_Insider_UK", "Buzzfeed", "CBC_News", "CNBC", "CNN", "Crypto_Coins_News", "Daily_Mail",
            "Engadget", "Entertainment_Weekly", "ESPN", "Financial_Post", "Financial_Times", "Fortune", "Fox_News", "Google_News", "Hacker_News", "IGN", "Independent", "Mashable", "Medical_News_Today", "Metro", "Mirror",
            "MSNBC", "MTV_News", "MTV_News_UK", "National_Geographic", "National_Review", "NBC_News", "New_Scientist", "Newsweek", "New_York_Magazine", "Next_Big_Future", "NFL_News", "NHL_News", "Politico", "Polygon", "Recode", "TalkSport",
            "TechCrunch", "TechRadar", "The_American_Conservative", "The_Economist", "The_Globe_And_Mail", "The_Hill", "The_Huffington_Post", "The_Irish_Times", "The_Jerusalem_Post", "The_Lad_Bible", "The_New_York_Times", "The_Next_Web",
            "The_Sport_Bible", "The_Telegraph", "The_Verge", "The_Wall_Street_Journal", "The_Washington_Post", "The_Washington_Times", "Time", "USA_Today", "Vice_News", "Wired"};
        string[] quotes = { "Nothing is impossible, the word itself says “I’m possible”! —Audrey Hepburn", "The greatest effort is not concerned with results. —Atisha", "Wisdom equals knowledge plus courage. You have to not only know what to do and when to do it, but you have to also be brave enough to follow through. —Jarod Kintz", "In a battle between two ideas, the best one doesn’t necessarily win. No, the idea that wins is the one with the most fearless heretic behind it. —Seth Godin", "Remember, teamwork begins by building trust. And the only way to do that is to overcome our need for invulnerability. —Patrick Lencioni", "Leadership is an action, not a position. —Donald McGannon", "Surround yourself with great people; delegate authority; get out of the way. —Ronald Reagan",
            "I cannot give you a formula for success, but I can give you the formula for failure, which is: Try to please everybody. —Herbert Bayard Swope", "Show me the man you honor and I will know what kind of man you are. —Thomas John Carlisle", "A man always has two reasons for doing anything: a good reason and the real reason. —J.P. Morgan", "If you spend your life trying to be good at everything, you will never be great at anything. —Tom Rath", "Average leaders raise the bar on themselves; good leaders raise the bar for others; great leaders inspire others to raise their own bar. —Orrin Woodward", "Don’t blow off another’s candle for it won’t make yours shine brighter. —Jaachynma N.E. Agu", "Whenever you see a successful business, someone once made a courageous decision. —Peter F. Drucker", "When you put together deep knowledge about a subject that intensely matters to you, charisma happens. You gain courage to share your passion, and when you do that, folks follow. —Jerry Porras", "People buy into the leader before they buy into the vision. —John Maxwell",
            "A good leader is a person who takes a little more than his share of the blame and a little less than his share of the credit. —John Maxwell", "I alone cannot change the world, but I can cast a stone across the water to create many ripples. —Mother Teresa", "Whether you think you can or you think you can’t, you’re right. —Henry Ford", "If you are not willing to risk the unusual, you will have to settle for the ordinary. —Jim Rohn", "Take up one idea. Make that one idea your life-think of it, dream of it, live on that idea. Let the brain, muscles, nerves, every part of your body, be full of that idea, and just leave every other idea alone. This is the way to success. —Swami Vivekananda", "If you are willing to do more than you are paid to do, eventually you will be paid to do more than you do. —Anonymous",
            "Success is walking from failure to failure with no loss of enthusiasm. —Winston Churchill", "Whatever the mind of man can conceive and believe, it can achieve. —Napoleon Hill", "Whenever you see a successful person, you only see the public glories, never the private sacrifices to reach them. —Vaibhav Shah", "Try not to become a person of success, but rather try to become a person of value. —Albert Einstein", "Success usually comes to those who are too busy to be looking for it. —Henry David Thoreau", "Great minds discuss ideas; average minds discuss events; small minds discuss people. —Eleanor Roosevelt", "I am not a product of my circumstances. I am a product of my decisions. —Stephen Covey", "When everything seems to be going against you, remember that the airplane takes off against the wind, not with it. —Henry Ford", "No one can make you feel inferior without your consent. —Eleanor Roosevelt", "The distance between insanity and genius is measured only by success. —Bruce Feirstein", "Don’t be afraid to give up the good to go for the great. —John D. Rockefeller",
            "If you can’t explain it simply, you don’t understand it well enough. —Albert Einstein", "There are two types of people who will tell you that you cannot make a difference in this world: those who are afraid to try and those who are afraid you will succeed. —Ray Goforth", "Success is the sum of small efforts, repeated day in and day out. —Robert Collier", "The most common way people give up their power is by thinking they don’t have any. —Alice Walker", "The most difficult thing is the decision to act, the rest is merely tenacity. —Amelia Earhart", "It is during our darkest moments that we must focus to see the light. —Aristotle Onassis", "Don’t judge each day by the harvest you reap but by the seeds that you plant. —Robert Louis Stevenson", "Courage is resistance to fear, mastery of fear — not absence of fear. —Mark Twain", "Only put off until tomorrow what you are willing to die having left undone. —Pablo Picasso", "Twenty years from now, you will be more disappointed by the things that you didn’t do than by the ones you did do. So throw off the bowlines. Sail away from the safe harbor. Catch the trade winds in your sails. Explore. Dream. Discover. —Mark Twain",
            "Remember that not getting what you want is sometimes a wonderful stroke of luck. —Dalai Lama", "The successful warrior is the average man, with laserlike focus. —Bruce Lee", "You can’t connect the dots looking forward; you can only connect them looking backward. So you have to trust that the dots will somehow connect in your future. You have to trust in something — your gut, destiny, life, karma, whatever. This approach has never let me down, and it has made all the difference in my life. —Steve Jobs", "Successful people do what unsuccessful people are not willing to do. Don’t wish it were easier; wish you were better. —Jim Rohn",
            "The No. 1 reason people fail in life is because they listen to their friends, family, and neighbors. —Napoleon Hill", "Many of life’s failures are people who did not realize how close they were to success when they gave up. —Thomas A. Edison", "What would you attempt to do if you knew you would not fail? —Robert Schuller", "Always bear in mind that your own resolution to success is more important than any other one thing. —Abraham Lincoln", "Successful and unsuccessful people do not vary greatly in their abilities. They vary in their desires to reach their potential. —John Maxwell", "You may have to fight a battle more than once to win it. —Margaret Thatcher", "I’ve learned that people will forget what you said, people will forget what you did, but people will never forget how you made them feel. —Maya Angelou", "Much of the stress that people feel doesn’t come from having too much to do. It comes from not finishing what they’ve started. —David Allen", "Focus on the journey, not the destination. Joy is found not in finishing an activity but in doing it. —Greg Anderson", "Success at the highest level comes down to one question: Can you decide that your happiness can come from someone else’s success? —Bill Walton",
            "Do what you have always done and you’ll get what you have always got. —Sue Knight", "Think of what you have rather than of what you lack. Of the things you have, select the best and then reflect how eagerly you would have sought them if you did not have them. —Marcus Aurelius", "Happiness is where we find it, but very rarely where we seek it. —J. Petit Senn", "You never regret being kind. —Nicole Shepherd", "To be content means that you realize you contain what you seek. —Alan Cohen", "Expecting life to treat you well because you are a good person is like expecting an angry bull not to charge because you are a vegetarian. —Shari R. Barr", "View your life from your funeral: Looking back at your life experiences, what have you accomplished? What would you have wanted to accomplish but didn’t? What were the happy moments? What were the sad? What would you do again, and what wouldn’t you do? —Victor Frankl", "Boredom is the feeling that everything is a waste of time… serenity, that nothing is. —Thomas Szasz", "To handle yourself, use your head; to handle others, use your heart. —Eleanor Roosevelt", "The mediocre teacher tells. The good teacher explains. The superior teacher demonstrates. The great teacher inspires. —William Arthur Ward",
            "It is not the strongest of the species that survive, nor the most intelligent, but the one most responsive to change. —Charles Darwin", "Keep your fears to yourself, but share your courage with others. —Robert Louis Stevenson", "The greatest leader is not necessarily the one who does the greatest things. He is the one that gets people to do the greatest things. —Ronald Reagan", "I can’t change the direction of the wind, but I can adjust my sails to always reach my destination. —Jimmy Dean", "I have learned over the years that when one’s mind is made up, this diminishes fear. —Rosa Parks", "Victory has a hundred fathers and defeat is an orphan. —John F. Kennedy", "Management is doing things right; leadership is doing the right things. —Peter F. Drucker", "Example is not the main thing in influencing others. It is the only thing. —Albert Schweitzer", "Leaders must be close enough to relate to others, but far enough ahead to motivate them. —John C. Maxwell", "The mark of a great man is one who knows when to set aside the important things in order to accomplish the vital ones. —Brandon Sanderson", "If you want to lift yourself up, lift up someone else. —Booker T. Washington",
            "You have to be burning with an idea, or a problem, or a wrong that you want to right. If you’re not passionate enough from the start, you’ll never stick it out. —Steve Jobs", "A person who never made a mistake never tried anything new. —Albert Einstein", "The only person you are destined to become is the person you decide to be. —Ralph Waldo Emerson", "We can’t help everyone, but everyone can help someone. —Ronald Reagan", "Everything you’ve ever wanted is on the other side of fear. —George Addair", "Being responsible sometimes means pissing people off. —Colin Powell", "Change your thoughts and you change your world. —Norman Vincent Peale", "How wonderful it is that nobody need wait a single moment before starting to improve the world. —Anne Frank", "Life is 10% what happens to me and 90% of how I react to it. —Charles Swindoll", "I cannot trust a man to control others who cannot control himself. —Robert E. Lee",
            "Perfection is not attainable, but if we chase perfection we can catch excellence. —Vince Lombardi", "You get in life what you have the courage to ask for. —Nancy D. Solomon", "In the end, it is important to remember that we cannot become what we need to be by remaining what we are. —Max De Pree", "Believe you can and you’re halfway there. —Theodore Roosevelt", "Too many of us are not living our dreams because we are living our fears. —Les Brown", "If you really want the key to success, start by doing the opposite of what everyone else is doing. —Brad Szollose", "What we achieve inwardly will change outer reality. —Plutarch", "A good plan violently executed now is better than a perfect plan executed next week. —George Patton", "Feeling gratitude and not expressing it is like wrapping a present and not giving it. —William Arthur Ward", "Silent gratitude isn’t very much to anyone. —Gertrude Stein", "The only people with whom you should try to get even are those who have helped you. —John E. Southard", "Keep your eyes open and try to catch people in your company doing something right, then praise them for it. —Tom Hopkins", "You wouldn’t worry so much about what others think of you if you realized how seldom they do. —Eleanor Roosevelt",
            "Shyness has a strange element of narcissism, a belief that how we look, how we perform, is truly important to other people. —Andre Dubus", "Do or do not. There is no try. —Yoda", "Rarely have I seen a situation where doing less than the other guy is a good strategy. —Jimmy Spithill", "The best revenge is massive success. —Frank Sinatra", "The question isn’t who is going to let me; it’s who is going to stop me. —Ayn Rand", "The only way to do great work is to love what you do. —Steve Jobs", "If you would convince a man that he does wrong, do right. But do not care to convince him. Men will believe what they see. Let them see. —Henry David Thoreau", "Low self-confidence isn’t a life sentence. Self-confidence can be learned, practiced, and mastered-just like any other skill. Once you master it, everything in your life will change for the better. —Barrie Davenport" };
        string[] morningImages = {"sunrise1.jpg", "sunrise2.jpg", "sunrise3.jpg", "sunrise4.jpg", "sunrise5.png", "sunrise6.png", "sunrise7.png", "sunrise8.png"};
        string[] dayImages = { "day1.jpg", "day2.jpg", "day3.jpg", "day4.jpg", "day5.jpg", "day6.jpg", "day7.jpg", "day8.jpg" };
        string[] nightImages = { "night1.jpg", "night2.jpg", "night3.jpg", "night4.jpg", "night5.jpg", "night6.jpg", "night7.jpg", "night8.jpg" };
        List<string> newsUrls = new List<string>();
        List<string> calendarUrls = new List<string>();
        PullToRefreshLayout refreshLayout = new PullToRefreshLayout();
        string briefString = "";
        bool briefingDoneLoading = false;
        bool briefingPlaying = false;
        int currentUtterance = 0;
        int maxUtterance = 0;
        Random rand = new Random();
        bool isAppearing = false;
        bool weatherLocationError = false;
        Label loadingProgressLabel;
        Label loadingLabel;
        StackLayout mainNewsStack, mainWeatherStack, mainTodoStack, mainCalendarStack, mainEmailStack, mainStocksStack;

        public MainPage()
		{
			InitializeComponent();

            #region key initialization
            //make sure all keys exist
            for (int i = 0; i < newsSources.Length; i++)
            {
                if (!Application.Current.Properties.ContainsKey(newsSources[i]))
                {
                    Application.Current.Properties.Add(newsSources[i], "false");
                }
            }
            if (!Application.Current.Properties.ContainsKey("weatherTown"))
            {
                Application.Current.Properties.Add("weatherTown", "New York");
            }
            if (!Application.Current.Properties.ContainsKey("userName"))
            {
                Application.Current.Properties.Add("userName", "");
            }
            if (!Application.Current.Properties.ContainsKey("userCalendars"))
            {
                Application.Current.Properties.Add("userCalendars", "-1");
            }
            if (!Application.Current.Properties.ContainsKey("googleTokenExpiry"))
            {
                Application.Current.Properties.Add("googleTokenExpiry", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm tt"));
            }
            if (!Application.Current.Properties.ContainsKey("appTheme"))
            {
                Application.Current.Properties.Add("appTheme", "dark");
            }
            if (!Application.Current.Properties.ContainsKey("timeAppTheme"))
            {
                Application.Current.Properties.Add("timeAppTheme", "t");
            }
            if (!Application.Current.Properties.ContainsKey("enabledSections"))
            {
                Application.Current.Properties.Add("enabledSections", "4");
            }
            if (!Application.Current.Properties.ContainsKey("sunsetTime"))
            {
                Application.Current.Properties.Add("sunsetTime", new DateTime(1, 1, 1, 1, 1, 1).ToString("yyyy-MM-dd HH:mm tt"));
            }
            if (!Application.Current.Properties.ContainsKey("settingsChanged"))
            {
                Application.Current.Properties.Add("settingsChanged", "f");
            }
            if (!Application.Current.Properties.ContainsKey("fullCalendarBriefing"))
            {
                Application.Current.Properties.Add("fullCalendarBriefing", "t");
            }
            if (!Application.Current.Properties.ContainsKey("cloudCoverOutline"))
            {
                Application.Current.Properties.Add("cloudCoverOutline", "f");
            }
            if (!Application.Current.Properties.ContainsKey("stocks"))
            {
                Application.Current.Properties.Add("stocks", "");
            }
            if (!Application.Current.Properties.ContainsKey("googleLogin"))
            {
                Application.Current.Properties.Add("googleLogin", "f");
            }
            if (!Application.Current.Properties.ContainsKey("todoistLogin"))
            {
                Application.Current.Properties.Add("todoistLogin", "f");
            }
            if (!Application.Current.Properties.ContainsKey("stocksPercentChange"))
            {
                Application.Current.Properties.Add("stocksPercentChange", "f");
            }
            if (!Application.Current.Properties.ContainsKey("lastRefresh"))
            {
                Application.Current.Properties.Add("lastRefresh", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm tt"));
            }
            if (!Application.Current.Properties.ContainsKey("useLocationWeather"))
            {
                Application.Current.Properties.Add("useLocationWeather", "t");
            }
            if (!Application.Current.Properties.ContainsKey("customWeatherLocation"))
            {
                Application.Current.Properties.Add("customWeatherLocation", "");
            }
            if (!Application.Current.Properties.ContainsKey("localWeatherCoords"))
            {
                Application.Current.Properties.Add("localWeatherCoords", "");
            }
            if (!Application.Current.Properties.ContainsKey("customWeatherLocationTimeOffset"))
            {
                Application.Current.Properties.Add("customWeatherLocationTimeOffset", "");
            }
            if (!Application.Current.Properties.ContainsKey("localWeatherLocationTimeOffset"))
            {
                Application.Current.Properties.Add("localWeatherLocationTimeOffset", "");
            }
            if (!Application.Current.Properties.ContainsKey("customWeatherLocationCoords"))
            {
                Application.Current.Properties.Add("customWeatherLocationCoords", "0,0");
            }
            if (!Application.Current.Properties.ContainsKey("dailyRefreshes"))
            {
                Application.Current.Properties.Add("dailyRefreshes", "0");
            }
            if (!Application.Current.Properties.ContainsKey("stocksSection"))
            {
                Application.Current.Properties.Add("stocksSection", "f");
            }
            if (!Application.Current.Properties.ContainsKey("travelSection"))
            {
                Application.Current.Properties.Add("travelSection", "f");
            }
            if (!Application.Current.Properties.ContainsKey("commuteDestinations"))
            {
                Application.Current.Properties.Add("commuteDestinations", "");
            }
            if (!Application.Current.Properties.ContainsKey("commuteCoords"))
            {
                Application.Current.Properties.Add("commuteCoords", "");
            }
            if (!Application.Current.Properties.ContainsKey("commuteTimes"))
            {
                Application.Current.Properties.Add("commuteTimes", "");
            }
            if (!Application.Current.Properties.ContainsKey("commuteNames"))
            {
                Application.Current.Properties.Add("commuteNames", "");
            }
            if (!Application.Current.Properties.ContainsKey("stocksChange"))
            {
                Application.Current.Properties.Add("stocksChange", "f");
            }
            if (!Application.Current.Properties.ContainsKey("todoSection"))
            {
                Application.Current.Properties.Add("todoSection", "f");
            }
            if (!Application.Current.Properties.ContainsKey("showFullTodo"))
            {
                Application.Current.Properties.Add("showFullTodo", "t");
            }
            if (!Application.Current.Properties.ContainsKey("weatherSection"))
            {
                Application.Current.Properties.Add("weatherSection", "t");
                Application.Current.Properties.Add("emailSection", "t");
                Application.Current.Properties.Add("newsSection", "t");
                Application.Current.Properties.Add("calendarSection", "t");
            }
            if (!Application.Current.Properties.ContainsKey("dailyHighLow"))
            {
                Application.Current.Properties.Add("dailyHighLow", "t");
                Application.Current.Properties.Add("precipitationOutline", "t");
                Application.Current.Properties.Add("dailyHumidity", "t");
                Application.Current.Properties.Add("sunriseSunset", "t");
            }
            if (!Application.Current.Properties.ContainsKey("infoOrder"))
            {
                Application.Current.Properties.Add("infoOrder", "weather-email-calendar-todo-travel-stocks-news");
            }
            if (!Application.Current.Properties.ContainsKey("useImportantEmails"))
            {
                Application.Current.Properties.Add("useImportantEmails", "f");
                Application.Current.Properties.Add("readImportantSenders", "f");
                Application.Current.Properties.Add("readAllSenders", "f");
            }
            #endregion
            Application.Current.Resources["mainLabelStyle"] = Resources["lightThemeLabelStyle"];
            Application.Current.Resources["mainLineStyle"] = Resources["lightThemeLineStyle"];
            

            refreshLayout.RefreshColor = Color.FromHex("#006EFF");
            refreshLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            refreshLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            
            refreshLayout.Content = MainScrollLayout;
            Content = refreshLayout;
            refreshLayout.RefreshCommand = new Command(Refresh);
        }

        #region News
        async Task LoadAllNews()
        {
            //Make main news stack
            Label newsTitleLabel = new Label { Text = "News", TextColor = Color.FromHex("FF7F00"), FontSize = 15, HorizontalOptions = LayoutOptions.StartAndExpand };
            mainNewsStack.Children.Add(newsTitleLabel);
            StackLayout NewsStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand };
            try
            {
                string newsString = "";
                List<string[]> enabledSources = new List<string[]>();
                for (int i = 0; i < newsSources.Length; i++)
                {
                    if (Convert.ToBoolean((string)Application.Current.Properties[newsSources[i]]))
                    {
                        enabledSources.Add(new string[] { newsSources[i], i.ToString() });
                    }
                }
                if (enabledSources.Count == 0)
                {
                    Label tempLabel = new Label { Text = "Select some news sources from the news settings.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                    tempLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                    mainNewsStack.Children.Add(tempLabel);
                    return;  
                }
                newsString += "Here's some news for you.";
                
                Label newsLabel = new Label { Text = newsString.Replace("-", " "), FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand};
                newsLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                mainNewsStack.Children.Add(newsLabel);
                //Loop through all news sources
                for (int i = 0; i < enabledSources.Count; i++)
                {
                    //add "from" label
                    StackLayout newsSourceStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 40, Padding = 0 };
                    Label newsSourceFromLabel = new Label { Text = "From", HorizontalOptions = LayoutOptions.Start, FontSize = 23};
                    newsSourceFromLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                    newsSourceStack.Children.Add(newsSourceFromLabel);
                    Label newsSourceLabel = new Label { Text = enabledSources[i][0].Replace("_", " "), FontSize = 23, TextColor = Color.FromHex(newsSourceColors[Convert.ToInt32(enabledSources[i][1])]), HorizontalOptions = LayoutOptions.StartAndExpand, FontAttributes = FontAttributes.Bold};
                    newsSourceStack.Children.Add(newsSourceLabel);
                    NewsStack.Children.Add(newsSourceStack);
                    NewsStack.Children.Add(new Image { Source = "/drawable/line.png" });

                    newsString += "From " + newsSources[i].Replace("_", " ") + "-";
                    //get news from source
                    Task<string[][]> articlesTask = GetNews(enabledSources[i][0].Replace("_", "-").ToLower());
                    string[][] articles = await articlesTask;
                    int max = articles.Length;
                    if(max > 5) { max = 5; }
                    for(int x = 0; x < max; x++)
                    {
                        if (articles[x].Length > 0)
                        {
                            newsString += articles[x][0].Replace("-", " ");
                            Label newsArticleLabel = new Label { Text = articles[x][0], HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, ClassId = articles[x][1]};
                            newsArticleLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                            TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
                            gestureRecognizer.Tapped += NewsGestureRecognizer_Tapped;
                            newsArticleLabel.GestureRecognizers.Add(gestureRecognizer);
                            NewsStack.Children.Add(newsArticleLabel);
                            newsString += "-";
                            if (x < max - 1 || i == enabledSources.Count - 1)
                            {
                                NewsStack.Children.Add(new Image { Source = "/drawable/line.png" });
                            }
                            else
                            {
                                Image line = new Image {HorizontalOptions = LayoutOptions.FillAndExpand};
                                line.SetDynamicResource(VisualElement.StyleProperty, "mainLineStyle");
                                NewsStack.Children.Add(line);
                            }
                        }
                    }
                }
                mainNewsStack.Children.Add(NewsStack);
                briefString += newsString;
                
            }
            catch
            {
                DisplayAlert("Failed to load News", "Your news headlines failed to load", "OK");
            }
        }

        async Task<string[][]> GetNews(string source)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://newsapi.org/v2/top-headlines?sources=" + source + "&apiKey=12b669bc914d412bba4c4fa1c5772f13");
                string result = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                List<string[]> articles = new List<string[]>();
                for (int i = 0; i < json["articles"].Count(); i++)
                {
                    if (!json["articles"][i]["title"].ToString().Contains("CNN Video") && !json["articles"][i]["title"].ToString().Contains("Live Updates - CNN") && (json["articles"][i]["title"].ToString().Length > 15))
                    {
                        articles.Add(new string[] { "", "" });
                        if (json["articles"][i]["title"].ToString().Contains("(CNN)"))
                        {
                            articles[articles.Count - 1][0] = json["articles"][i]["title"].ToString().Substring(json["articles"][i]["content"].ToString().IndexOf("(CNN)") + 5, (json["articles"][i]["content"].ToString().Length) - (json["articles"][i]["content"].ToString().IndexOf("(CNN)") + 5));
                        }
                        else
                        {
                            articles[articles.Count - 1][0] = json["articles"][i]["title"].ToString();
                        }
                        articles[articles.Count - 1][1] = json["articles"][i]["url"].ToString();
                    }
                }

                return (articles.ToArray());
            }
            catch(Exception e)
            {
                return (new string[0][]);
            }
        }

        private void NewsGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(((Label)sender).ClassId));
        }
        #endregion

        #region Weather
        async Task LoadWeather()
        {
            if (mainWeatherStack.Children.Count == 0)
            {
                Label weatherTitleLabel = new Label { Text = "Weather", HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 15, TextColor = Color.FromHex("56A5FF") };
                mainWeatherStack.Children.Add(weatherTitleLabel);
                try
                {
                    HttpClient client = new HttpClient();
                    string lat = "";
                    string lon = "";
                    if (Application.Current.Properties["useLocationWeather"].ToString() == "t" || Application.Current.Properties["customWeatherLocationCoords"].ToString() == "0,0")
                    {
                        try
                        {
                            var locator = CrossGeolocator.Current;
                            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                            lat = position.Latitude.ToString();
                            lon = position.Longitude.ToString();
                        }
                        catch
                        {
                            weatherLocationError = true;
                            Label errorLabel = new Label { Text = "The weather could not be loaded because there was an error finding your location. Make sure location is on and permissions are granted, and then refresh.", HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20 };
                            errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                            mainWeatherStack.Children.Add(errorLabel);
                            DisplayAlert("Error finding location", "The weather could not be loaded because there was an error finding your location. Typically this is due to either having your location turned off, or not granting location permissions to the app.", "OK");
                            return;
                        }
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
                    string high = "";
                    string low = "";
                    if (Application.Current.Properties["dailyHighLow"].ToString() == "t")
                    {
                        if (json["main"]["temp_max"].ToString().Contains('.'))
                        {
                            high = json["main"]["temp_max"].ToString().Substring(0, json["main"]["temp_max"].ToString().ToString().IndexOf('.'));
                        }
                        else
                        {
                            high = json["main"]["temp_max"].ToString();
                        }
                        if (json["main"]["temp_min"].ToString().Contains('.'))
                        {
                            low = json["main"]["temp_min"].ToString().Substring(0, json["main"]["temp_min"].ToString().ToString().IndexOf('.'));
                        }
                        else
                        {
                            low = json["main"]["temp_min"].ToString();
                        }
                    }

                    DateTime[] getSunriseSetTimes = await GetSunRiseSetTimes(lat, lon, json);
                    DateTime sunriseTime = getSunriseSetTimes[0];
                    DateTime sunsetTime = getSunriseSetTimes[1];
                    Application.Current.Properties["sunsetTime"] = sunsetTime.ToString("yyyy-MM-dd HH:mm tt");

                    string temperature = "";
                    if (json["main"]["temp"].ToString().Contains('.'))
                    {
                        temperature = json["main"]["temp"].ToString().Substring(0, json["main"]["temp"].ToString().ToString().IndexOf('.'));
                    }
                    else
                    {
                        temperature = json["main"]["temp"].ToString();
                    }

                    string connectingWord = "";
                    if (json["weather"][0]["description"].ToString()[json["weather"][0]["description"].ToString().Length - 1] == 's' || json["weather"][0]["description"].ToString()[json["weather"][0]["description"].ToString().Length - 1] == 't' || json["weather"][0]["description"].ToString()[json["weather"][0]["description"].ToString().Length - 1] == 'n' || json["weather"][0]["description"].ToString()[json["weather"][0]["description"].ToString().Length - 1] == 'e')
                    {
                        connectingWord = "with";
                    }
                    else if (json["weather"][0]["description"].ToString()[json["weather"][0]["description"].ToString().Length - 1] == 'y')
                    {
                        connectingWord = "with a";
                    }
                    else
                    {
                        connectingWord = "and";
                    }

                    //setup briefing string
                    string weatherBriefString = "It is currently " + temperature + " degrees " + connectingWord + " " + json["weather"][0]["description"].ToString() + ".-";
                    if (Application.Current.Properties["dailyHighLow"].ToString() == "t")
                    {
                        weatherBriefString += "Today there ";
                        if (DateTime.Now.Hour < 12)
                        {
                            weatherBriefString += "will be ";
                        }
                        else
                        {
                            weatherBriefString += "is ";
                        }
                        weatherBriefString += "a high of " + high + " and a low of " + low + ".-";
                    }
                    //humidity
                    if (Application.Current.Properties["dailyHumidity"].ToString() == "t")
                    {
                        weatherBriefString += "The humidity is currently " + Math.Round(Convert.ToDouble(json["main"]["humidity"].ToString()), 0).ToString() + " percent.-";
                    }
                    //precipitation outline
                    if (Application.Current.Properties["precipitationOutline"].ToString() == "t" || Application.Current.Properties["cloudCoverOutline"].ToString() == "t")
                    {
                        response = await client.GetAsync("https://api.openweathermap.org/data/2.5/forecast/?" + "lat=" + lat + "&lon=" + lon + "&mode=json&units=imperial&appid=f65268dddf50ca94e95bedb00e196013");
                        result = await response.Content.ReadAsStringAsync();
                        JArray hourly = JArray.Parse(JObject.Parse(result)["list"].ToString());
                        double[] temps = new double[8];
                        int[] humidity = new int[8];
                        int[] clouds = new int[8];
                        double[] rain = new double[8];
                        double[] snow = new double[8];
                        for (int i = 0; i < 8; i++)
                        {
                            temps[i] = Convert.ToDouble(hourly[i]["main"]["temp"].ToString());
                            humidity[i] = Convert.ToInt32(hourly[i]["main"]["humidity"].ToString());
                            clouds[i] = Convert.ToInt32(hourly[i]["clouds"]["all"].ToString());
                            try
                            {
                                rain[i] = Convert.ToDouble(hourly[i]["rain"]["1h"].ToString());
                            }
                            catch
                            {
                                rain[i] = -1;
                            }
                            try
                            {
                                snow[i] = Convert.ToDouble(hourly[i]["snow"]["1h"].ToString());
                            }
                            catch
                            {
                                snow[i] = 0;
                            }
                        }
                        if (Application.Current.Properties["cloudCoverOutline"].ToString() == "t")
                        {
                            weatherBriefString += CreateCloudSummary(clouds) + " -";
                        }
                        if (Application.Current.Properties["precipitationOutline"].ToString() == "t")
                        {
                            weatherBriefString += CreateRainSummary(rain) + " -";
                        }
                    }
                    //sunrise/sunset
                    if (Application.Current.Properties["sunriseSunset"].ToString() == "t")
                    {
                        if (DateTime.Now < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunriseTime.Hour, sunriseTime.Minute, 0))
                        {
                            weatherBriefString += "The sun will rise today at " + sunriseTime.ToShortTimeString() + " and set at " + sunsetTime.ToShortTimeString();
                        }
                        else if (DateTime.Now < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunsetTime.Hour, sunsetTime.Minute, 0))
                        {
                            weatherBriefString += "The sun will set today at " + sunsetTime.ToShortTimeString();
                        }
                    }
                    Label weatherLabel = new Label { Text = weatherBriefString.Replace("-", " "), HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, FontAttributes = FontAttributes.Bold };
                    weatherLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                    mainWeatherStack.Children.Add(weatherLabel);
                    briefString += weatherBriefString;
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("An error occured while sending the request"))
                    {
                        DisplayAlert("Failed to load Weather", "An internet connection is required to get the weather forecast", "OK");
                    }
                    else
                    {
                        DisplayAlert("Failed to load Weather", "Your weather summary failed to load", "OK");
                    }
                }
            }
        }

        string CreateCloudSummary(int[] cloudRatings)
        {
            //create groups
            List<int[]> groups = new List<int[]>();
            bool currentGroup = false;
            int totalCloudyHours = 0;
            for(int i = 0; i < 8; i++)
            {
                if(cloudRatings[i] > 90)
                {
                    if (currentGroup)
                    {
                        groups[groups.Count - 1][1] += 1;
                        totalCloudyHours++;
                    }
                    else
                    {
                        currentGroup = true;
                        groups.Add(new int[] { i, 0 });
                        totalCloudyHours++;
                    }
                }
                else
                {
                    currentGroup = false;
                }
            }

            //throw away the groups that have already occured
            List<int[]> newGroups = new List<int[]>();
            for(int i = 0; i < groups.Count(); i++)
            {
                if(groups[i][0] * 3 + groups[i][1] * 3 > DateTime.Now.Hour)
                {
                    newGroups.Add(groups[i]);
                }
            }
            groups = newGroups;
            
            //create string to describe groups
            if(totalCloudyHours >= 6)
            {
                if (totalCloudyHours >= 7)
                {
                    return ("It will be cloudy all day.");
                }
                return ("It will be cloudy most of the day.");
            }
            else if(totalCloudyHours <= 1 || groups.Count == 0)
            {
                return ("It will be sunny all day.");
            }
            else
            {
                string temp = "";
                if (groups[0][0] * 3 < DateTime.Now.Hour)
                {
                    if (rand.Next(0, 1) == 0)
                    {
                        temp += "There is an overcast now";
                    }
                    else
                    {
                        temp += "It is cloudy now";
                    }
                }
                else
                {
                    if (rand.Next(0, 1) == 0)
                    {
                        temp += "There will be an overcast starting at " + twelveHourFormat(groups[0][0] * 3);
                    }
                    else
                    {
                        temp += "It will be cloudy today starting at " + twelveHourFormat(groups[0][0] * 3);
                    }
                }
                if (groups[0][0] * 3 + groups[0][1] * 3 < 20)
                {
                    if(groups[0][1] * 3 == 0)
                    {
                        temp += " for an hour";
                    }
                    temp += " until " + twelveHourFormat(groups[0][0] * 3 + groups[0][1] * 3);
                    if (groups.Count > 1)
                    {
                        temp += ", then clear until " + twelveHourFormat(groups[1][0] * 3);
                    }
                    for (int i = 1; i < groups.Count; i++)
                    {
                        temp += ", then cloudy";
                        if(groups[i][0] * 3 + groups[i][1] * 3 > 21)
                        {
                            temp += " for the rest of the day.";
                        }
                        else
                        {
                            temp += " until " + twelveHourFormat(groups[i][0] * 3 + groups[i][1] * 3);
                        }
                        if(i == groups.Count - 1)
                        {
                            temp += ", then clear for the rest of the day.";
                        }
                    }
                }
                else
                {
                    temp += " for the rest of the day.";
                }
                
                return (temp);
            }
            return ("");
        }

        string CreateRainSummary(double[] rainRatings)
        {
            //create groups
            List<int[]> groups = new List<int[]>();
            bool currentGroup = false;
            int totalRainyHours = 0;
            for (int i = 0; i < 8; i++)
            {
                if (rainRatings[i] > 0)
                {
                    if (currentGroup)
                    {
                        groups[groups.Count - 1][1] += 1;
                        totalRainyHours++;
                        if(rainRatings[i] > 0.3f)
                        {
                            //heavy rain
                            groups[groups.Count - 1][2] = 1;
                        }
                    }
                    else
                    {
                        currentGroup = true;
                        //codes = {startTime, length, heavyRating} (0 for light, 1 for heavy)
                        if (rainRatings[i] > 0.3f)
                        {
                            groups.Add(new int[] { i, 0, 1});
                        }
                        else
                        {
                            groups.Add(new int[] { i, 0, 0});
                        }
                    }
                }
                else
                {
                    currentGroup = false;
                }
            }

            //create string to describe groups
            if (totalRainyHours >= 6)
            {
                if (totalRainyHours >= 7)
                {
                    return ("It will be raining all day.");
                }
                return ("It will be raining most of the day.");
            }
            else if (totalRainyHours == 0 || groups.Count == 0)
            {
                return ("There won't be any rain today.");
            }
            else
            {
                string temp = "";
                if (rand.Next(0, 1) == 0)
                {
                    if (groups[0][2] == 1)
                    {
                        temp += "There will be heavy rain starting at " + twelveHourFormat(groups[0][0] * 3);
                    }
                    else
                    {
                        temp += "There will be light rain starting at " + twelveHourFormat(groups[0][0] * 3);
                    }
                }
                else
                {
                    if (groups[0][2] == 1)
                    {
                        temp += "It will be raining heavy starting at " + twelveHourFormat(groups[0][0] * 3);
                    }
                    else
                    {
                        temp += "It will be raining lightly starting at " + twelveHourFormat(groups[0][0] * 3);
                    }
                }
                if (groups[0][0] * 3 + groups[0][1] * 3 < 20)
                {
                    temp += " until " + twelveHourFormat(groups[0][0] * 3 + groups[0][1] * 3);
                    if (groups.Count > 1)
                    {
                        temp += ", then clear until " + twelveHourFormat(groups[1][0] * 3);
                    }
                    for (int i = 1; i < groups.Count; i++)
                    {
                        temp += ", then ";
                        if(groups[i][2] == 1)
                        {
                            temp += "heavy rain";
                        }
                        else
                        {
                            temp += "light rain";
                        }
                        if (groups[i][0] * 3 + groups[i][1] * 3 > 21)
                        {
                            temp += " for the rest of the day.";
                            break;
                        }
                        else
                        {
                            temp += " until " + twelveHourFormat(groups[i][0] * 3 + groups[i][1] * 3);
                        }
                        if (i == groups.Count - 1)
                        {
                            temp += ", then clear for the rest of the day.";
                        }
                    }
                }
                else
                {
                    temp += ".";
                }

                return (temp);
            }
        }

        string twelveHourFormat(int num)
        {
            if (num > 11)
            {
                if (num > 12)
                {
                    num -= 12;
                }
                return (num.ToString() + " PM");
            }
            else if (num == 0)
            {
                return ("12 AM");
            }
            return (num.ToString() + " AM");
        }
        #endregion

        #region Calendar
        async Task LoadCalendarEvents()
        {
            if (mainCalendarStack.Children.Count == 0)
            {
                try
                {
                    Label calendarTitleLabel = new Label { Text = "Schedule", TextColor = Color.FromHex("A642FF"), FontSize = 15, HorizontalOptions = LayoutOptions.StartAndExpand };
                    mainCalendarStack.Children.Add(calendarTitleLabel);
                    if (Application.Current.Properties["googleLogin"].ToString() == "f")
                    {
                        Label errorLabel = new Label { Text = "You must sign in to google in settings to get calendar briefings.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                        errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                        mainCalendarStack.Children.Add(errorLabel);
                        return;
                    }
                    StackLayout CalendarStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand };
                    //token
                    DateTime expiryTime = DateTime.ParseExact(Application.Current.Properties["googleTokenExpiry"].ToString(), "yyyy-MM-dd HH:mm tt", CultureInfo.InvariantCulture);
                    if (expiryTime < DateTime.Now)
                    {
                        //refresh token
                        Task<string> tempTask = DependencyService.Get<ITextToSpeech>().RefreshAccessToken();
                        string temp = await tempTask;
                        if (temp == "noAccount")
                        {
                            Label errorLabel = new Label { Text = "You must sign in to google in settings to get calendar briefings.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                            errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                            mainCalendarStack.Children.Add(errorLabel);
                            return;
                        }
                        expiryTime = DateTime.Now.AddSeconds(Convert.ToDouble(temp));
                        Application.Current.Properties["googleTokenExpiry"] = expiryTime.ToString("yyyy-MM-dd HH:mm tt");
                    }
                    //calendars
                    string[] calendarsToUse = new string[0];
                    if (Application.Current.Properties["userCalendars"].ToString() == "-1")
                    {
                        Task<string[]> calendarsTask = DependencyService.Get<ITextToSpeech>().GetCalendars();
                        calendarsToUse = await calendarsTask;
                        //Only use first calendar
                        calendarsToUse = new string[] { calendarsToUse[0] };
                        Application.Current.Properties["userCalendars"] = calendarsToUse[0];
                    }
                    else
                    {
                        string temp = Application.Current.Properties["userCalendars"].ToString();
                        calendarsToUse = temp.Split(',');
                    }
                    //events
                    if (calendarsToUse.Length > 0)
                    {
                        Task<string[][]> eventsTask = DependencyService.Get<ITextToSpeech>().GetCalendarEvents(calendarsToUse);
                        string[][] events = await eventsTask;
                        List<string[]> newEvents = new List<string[]>();
                        for (int i = 0; i < events.Length; i++)
                        {
                            try
                            {
                                DateTime tempDate = DateTime.Parse(events[i][1]);
                                DateTime eventDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tempDate.Hour, tempDate.Minute, 0);
                                if (DateTime.Now < eventDate)
                                {
                                    newEvents.Add(events[i]);
                                }
                            }
                            catch { }
                        }
                        string calendarString = "";
                        if (newEvents.Count() == events.Length && events.Length > 0)
                        {
                            if (newEvents.Count() == 1)
                            {
                                calendarString = "You have only " + newEvents.Count().ToString() + " event today";
                            }
                            else
                            {
                                calendarString = "You have " + newEvents.Count().ToString() + " events today";
                            }
                        }
                        else if (newEvents.Count() > 0)
                        {
                            if (newEvents.Count() == 1)
                            {
                                calendarString = "You have only " + newEvents.Count().ToString() + " event left today";
                            }
                            else
                            {
                                calendarString = "You have " + newEvents.Count().ToString() + " events left today";
                            }
                        }
                        else
                        {
                            calendarString = "You don't have any events left today";
                        }
                        if (Application.Current.Properties["fullCalendarBriefing"].ToString() == "t")
                        {
                            calendarString += ".-";
                            Label calendarLabel = new Label { Text = calendarString.Replace("-", " "), FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand };
                            calendarLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                            mainCalendarStack.Children.Add(calendarLabel);

                            //Full calendar Briefing
                            for (int i = 0; i < newEvents.Count(); i++)
                            {
                                CalendarStack.Children.Add(new Image { Source = "/drawable/line.png" });
                                Label eventLabel = new Label { Text = "At " + DateTime.Parse(newEvents[i][1]).ToShortTimeString() + " you have " + newEvents[i][0] + ".", HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20, ClassId = newEvents[i][2] };
                                TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
                                gestureRecognizer.Tapped += CalendarGestureRecognizer_Tapped;
                                eventLabel.GestureRecognizers.Add(gestureRecognizer);
                                CalendarStack.Children.Add(eventLabel);
                                eventLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                                //Add audio briefing
                                calendarString += "At " + DateTime.Parse(newEvents[i][1]).ToShortTimeString() + " you have " + newEvents[i][0] + ".";
                                calendarString += "-";
                                if (i == newEvents.Count() - 1)
                                {
                                    CalendarStack.Children.Add(new Image { Source = "/drawable/line.png" });
                                }
                            }
                            mainCalendarStack.Children.Add(CalendarStack);
                        }
                        else
                        {
                            if (newEvents.Count() == 0)
                            {
                                calendarString += ".-";
                            }
                            else
                            {
                                if (newEvents.Count() == 1)
                                {
                                    calendarString += " and it starts in";
                                }
                                else
                                {
                                    calendarString += " and the first one starts in";
                                }
                                if (DateTime.Now.AddMinutes(59) > DateTime.Parse(newEvents[0][1]))
                                {
                                    calendarString += Math.Round((DateTime.Now - DateTime.Parse(newEvents[0][1])).TotalMinutes, 0).ToString().Trim();
                                    if (Math.Round((DateTime.Now - DateTime.Parse(newEvents[0][1])).TotalMinutes, 0) == 1)
                                    {
                                        calendarString += " minute.-";
                                    }
                                    else
                                    {
                                        calendarString += " minutes.-";
                                    }
                                }
                                else
                                {
                                    calendarString += Math.Round((DateTime.Now - DateTime.Parse(newEvents[0][1])).TotalHours, 0).ToString().Trim();
                                    if (Math.Round((DateTime.Now - DateTime.Parse(newEvents[0][1])).TotalHours, 0) == 1)
                                    {
                                        calendarString += " hour.-";
                                    }
                                    else
                                    {
                                        calendarString += " hours.-";
                                    }
                                }
                            }
                            Label calendarLabel = new Label { Text = calendarString.Replace("-", " "), FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand };
                            calendarLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                            mainCalendarStack.Children.Add(calendarLabel);
                        }
                        briefString += calendarString;
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("An error occured while sending the request"))
                    {
                        DisplayAlert("Failed to load Calendar", "An internet connection is required to get calendar events", "OK");
                    }
                    else
                    {
                        DisplayAlert("Failed to load Calendar", "Your calendar events failed to load", "OK");
                    }
                }
            }
        }

        private void CalendarGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(((Label)sender).ClassId));
        }
        #endregion

        #region Email
        async Task LoadEmails()
        {
            if (mainEmailStack.Children.Count == 0)
            {
                try
                {
                    Label emailTitleLabel = new Label { Text = "Email", TextColor = Color.FromHex("FF6877"), FontSize = 15, HorizontalOptions = LayoutOptions.StartAndExpand };
                    mainEmailStack.Children.Add(emailTitleLabel);
                    if (Application.Current.Properties["googleLogin"].ToString() == "f")
                    {
                        Label errorLabel = new Label { Text = "You must sign in to google in settings to get email briefings.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                        errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                        mainEmailStack.Children.Add(errorLabel);
                        return;
                    }
                    //token
                    DateTime expiryTime = DateTime.ParseExact(Application.Current.Properties["googleTokenExpiry"].ToString(), "yyyy-MM-dd HH:mm tt", CultureInfo.InvariantCulture);
                    if (expiryTime < DateTime.Now)
                    {
                        //refresh token
                        Task<string> tempTask = DependencyService.Get<ITextToSpeech>().RefreshAccessToken();
                        string temp = await tempTask;
                        if (temp == "noAccount")
                        {
                            Label errorLabel = new Label { Text = "You must sign in to google in settings to get email briefings.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                            errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                            mainEmailStack.Children.Add(errorLabel);
                            return;
                        }
                        expiryTime = DateTime.Now.AddSeconds(Convert.ToDouble(temp));
                        Application.Current.Properties["googleTokenExpiry"] = expiryTime.ToString("yyyy-MM-dd HH:mm tt");
                    }

                    //Get Emails
                    string emailQuery = "";
                    if (Application.Current.Properties["useImportantEmails"].ToString() == "t")
                    {
                        if (Application.Current.Properties["readImportantSenders"].ToString() == "t")
                        {
                            emailQuery += "read";
                        }
                        else
                        {
                            emailQuery += "num";
                        }
                        emailQuery += "important";
                    }
                    else
                    {
                        if (Application.Current.Properties["readAllSenders"].ToString() == "t")
                        {
                            emailQuery += "read";
                        }
                        else
                        {
                            emailQuery += "num";
                        }
                        emailQuery += "all";
                    }
                    Task<string[]> unreadEmailsTask = DependencyService.Get<ITextToSpeech>().GetUnreadEmails(emailQuery);
                    string[] unreadEmails = await unreadEmailsTask;
                    string emailString = "";
                    if (unreadEmails.Length == 1)
                    {
                        if (unreadEmails[0] == null)
                        {
                            if (Application.Current.Properties["useImportantEmails"].ToString() == "t")
                            {
                                briefString += "You have one important unread email.";
                                emailString = "You have one important unread email.";
                            }
                            else
                            {
                                briefString += "You have one unread email.";
                                emailString = "You have one unread email.";
                            }
                        }
                        else
                        {
                            if (Application.Current.Properties["useImportantEmails"].ToString() == "t")
                            {
                                briefString += "You have one important unread email from " + unreadEmails[0] + ".";
                                emailString = "You have one important unread email from " + unreadEmails[0] + ".";
                            }
                            else
                            {
                                briefString += "You have one unread email from " + unreadEmails[0] + ".";
                                emailString = "You have one unread email from " + unreadEmails[0] + ".";
                            }
                        }
                    }
                    else if (unreadEmails.Length > 1)
                    {
                        if (unreadEmails[0] == null)
                        {
                            if (Application.Current.Properties["useImportantEmails"].ToString() == "t")
                            {
                                briefString += "You have " + unreadEmails.Length.ToString() + " important unread emails.";
                                emailString = "You have " + unreadEmails.Length.ToString() + " important unread emails.";
                            }
                            else
                            {
                                briefString += "You have " + unreadEmails.Length.ToString() + " unread emails.";
                                emailString = "You have " + unreadEmails.Length.ToString() + " unread emails.";
                            }
                        }
                        else
                        {
                            if (Application.Current.Properties["useImportantEmails"].ToString() == "t")
                            {
                                briefString += "You have " + unreadEmails.Length.ToString() + " important unread emails from ";
                                emailString = "You have " + unreadEmails.Length.ToString() + " important unread emails from ";
                            }
                            else
                            {
                                briefString += "You have " + unreadEmails.Length.ToString() + " unread emails from ";
                                emailString = "You have " + unreadEmails.Length.ToString() + " unread emails from ";
                            }
                            List<string> unreadEmailList = unreadEmails.ToList();
                            unreadEmailList = unreadEmailList.Distinct().ToList();
                            for (int i = 0; i < unreadEmailList.Count; i++)
                            {
                                if (i == unreadEmailList.Count - 2)
                                {
                                    briefString += unreadEmailList[i] + " and ";
                                    emailString += unreadEmailList[i] + " and ";
                                }
                                else if (i == unreadEmailList.Count - 1)
                                {
                                    briefString += unreadEmailList[i] + ".";
                                    emailString += unreadEmailList[i] + ".";
                                }
                                else
                                {
                                    briefString += unreadEmailList[i] + ", ";
                                    emailString += unreadEmailList[i] + ", ";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties["useImportantEmails"].ToString() == "t")
                        {
                            briefString += "You don't have any important unread emails.";
                            emailString = "You don't have any important unread emails.";
                        }
                        else
                        {
                            briefString += "You don't have any unread emails.";
                            emailString = "You don't have any unread emails.";
                        }
                    }
                    Label emailLabel = new Label { Text = emailString, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                    emailLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                    mainEmailStack.Children.Add(emailLabel);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("An error occured while sending the request"))
                    {
                        DisplayAlert("Failed to load Email", "An internet connection is required to get emails", "OK");
                    }
                    else
                    {
                        DisplayAlert("Failed to load Email", "Your emails failed to load", "OK");
                    }
                }
            }
        }
        #endregion

        #region Stocks
        async Task LoadStocks()
        {
            if (mainStocksStack.Children.Count == 0)
            {
                try
                {
                    string stockString = "";
                    Label stockTitleLabel = new Label { Text = "Stocks", TextColor = Color.FromHex("93FFA5"), FontSize = 15, HorizontalOptions = LayoutOptions.StartAndExpand };
                    mainStocksStack.Children.Add(stockTitleLabel);
                    string[] stocks = Application.Current.Properties["stocks"].ToString().Split(',');
                    if (Application.Current.Properties["stocks"].ToString().Replace(" ", "").Length == 0)
                    {
                        Label errorLabel = new Label { Text = "Add some stocks in settings.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                        errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                        mainStocksStack.Children.Add(errorLabel);
                        return;
                    }
                    //get stock quotes
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync("https://cloud.iexapis.com/stable/stock/market/batch?symbols=" + Application.Current.Properties["stocks"].ToString() + "&types=quote&token=pk_f51846cce15f403fa11f3b676aaf111b");
                    string result = await response.Content.ReadAsStringAsync();
                    JObject fullJson = JObject.Parse(result);
                    int successStocks = 0;
                    for (int i = 0; i < stocks.Length; i++)
                    {
                        try
                        {
                            //make horizontal stack and stock ticker label
                            StackLayout horizontalStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 50 };
                            Label stockLabel = new Label { Text = stocks[i].ToUpper(), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 20, FontAttributes = FontAttributes.Bold };
                            stockLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                            horizontalStack.Children.Add(stockLabel);
                            //get stock price
                            JObject json = JObject.Parse(fullJson[stocks[i].ToUpper()]["quote"].ToString());
                            string price = json["iexRealtimePrice"].ToString() + "000000";
                            if (json["iexRealtimePrice"].ToString().Replace(" ", "") == "" || !price.Contains("."))
                            {
                                price = json["previousClose"].ToString() + "000000";
                            }
                            price = price.Substring(0, price.IndexOf('.') + 3);
                            string change = json["change"].ToString() + "000000";
                            if (!change.Contains("."))
                            {
                                change = json["extendedChange"].ToString() + "000000";
                            }
                            change = change.Substring(0, change.IndexOf('.') + 3);
                            string name = json["companyName"].ToString();
                            if (!change.Contains("-"))
                            {
                                change = "+" + change;
                            }
                            //build string 
                            stockString += name + " is currently ";
                            bool hasMoved = false;
                            if (change.Contains("+") && Convert.ToDouble(change.Remove(0, 1)) > 0)
                            {
                                stockString += "up ";
                                hasMoved = true;
                            }
                            else if (change.Contains("-"))
                            {
                                stockString += "down ";
                                hasMoved = true;
                            }
                            if (hasMoved)
                            {
                                if (change.Remove(0, 1).Split('.')[0] != "0" || change.Split('.')[1] == "00")
                                {
                                    stockString += change.Remove(0, 1).Split('.')[0] + " dollars";
                                }
                                if (change.Split('.')[1] != "00")
                                {
                                    stockString += " and " + change.Split('.')[1] + " cents";
                                }
                                stockString += " to ";
                            }
                            else
                            {
                                stockString += "at ";
                            }
                            if (price.Split('.')[0] != "0" || price.Split('.')[1] == "00")
                            {
                                stockString += price.Split('.')[0] + " dollars";
                            }
                            if (price.Split('.')[1] != "00")
                            {
                                stockString += " and " + price.Split('.')[1] + " cents";
                            }
                            stockString += ".";
                            if (i < stocks.Length - 1)
                            {
                                stockString += "-";
                            }
                            //display price and change color
                            if (Application.Current.Properties["stocksChange"].ToString() == "t")
                            {
                                Label priceLabel = new Label { Text = price + " ", HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 20, FontAttributes = FontAttributes.Bold };
                                Label changeLabel = new Label { Text = Math.Abs(Convert.ToDouble(change)).ToString(), HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 20, FontAttributes = FontAttributes.Bold };
                                if (changeLabel.Text.Split('.')[1].Length == 1)
                                {
                                    changeLabel.Text += "0";
                                }
                                if (Application.Current.Properties["stocksPercentChange"].ToString() == "t")
                                {
                                    changeLabel.Text = (Math.Round((Math.Abs(Convert.ToDouble(change)) / Convert.ToDouble(price)) * 100, 2)).ToString() + "%";
                                }
                                Image arrowImage = new Image { Source = "uparrowgreen.png", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 17 };
                                if (change.Contains("-"))
                                {
                                    priceLabel.TextColor = Color.FromHex("FF4C4C");
                                    changeLabel.TextColor = Color.FromHex("FF4C4C");
                                    arrowImage.Source = "downarrowred.png";
                                }
                                else if (Convert.ToDouble(change) > 0)
                                {
                                    priceLabel.TextColor = Color.FromHex("3DFF5D");
                                    changeLabel.TextColor = Color.FromHex("3DFF5D");
                                }
                                else
                                {
                                    priceLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                                    changeLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                                    arrowImage.IsVisible = false;
                                }
                                horizontalStack.Children.Add(priceLabel);
                                horizontalStack.Children.Add(changeLabel);
                                horizontalStack.Children.Add(arrowImage);
                            }
                            else
                            {
                                Label priceLabel = new Label { Text = price, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 20, FontAttributes = FontAttributes.Bold };
                                Image arrowImage = new Image { Source = "uparrowgreen.png", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 17 };
                                if (change.Contains("-"))
                                {
                                    priceLabel.TextColor = Color.FromHex("FF4C4C");
                                    arrowImage.Source = "downarrowred.png";
                                }
                                else if (Convert.ToDouble(change) > 0)
                                {
                                    priceLabel.TextColor = Color.FromHex("3DFF5D");
                                }
                                else
                                {
                                    priceLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                                    arrowImage.IsVisible = false;
                                }
                                horizontalStack.Children.Add(priceLabel);
                                horizontalStack.Children.Add(arrowImage);
                            }
                            mainStocksStack.Children.Add(horizontalStack);
                            if (i < stocks.Length - 1)
                            {
                                mainStocksStack.Children.Add(new Image { Source = "/drawable/line.png" });
                            }
                            successStocks++;
                        }
                        catch
                        {
                        }
                    }
                    if (successStocks < Application.Current.Properties["stocks"].ToString().Split(',').Length)
                    {
                        if (successStocks > 0)
                        {
                            if ((Application.Current.Properties["stocks"].ToString().Split(',').Length - successStocks) == 1)
                            {
                                DisplayAlert("Failed to load Stocks", (Application.Current.Properties["stocks"].ToString().Split(',').Length - successStocks).ToString() + " stock failed to load.", "OK");
                            }
                            else
                            {
                                DisplayAlert("Failed to load Stocks", (Application.Current.Properties["stocks"].ToString().Split(',').Length - successStocks).ToString() + " stocks failed to load.", "OK");
                            }
                        }
                        else
                        {
                            DisplayAlert("Failed to load Stocks", "Your stock quotes failed to load", "OK");
                        }
                    }
                    briefString += stockString;
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("An error occured while sending the request"))
                    {
                        DisplayAlert("Failed to load Stocks", "An internet connection is required to get stock quotes", "OK");
                    }
                    else
                    {
                        DisplayAlert("Failed to load Stocks", "Your stock quotes failed to load", "OK");
                    }
                }
            }
        }
        #endregion

        #region Travel

        #endregion

        #region Todoist
        async Task LoadTodoist()
        {
            if (mainTodoStack.Children.Count == 0)
            {
                //title
                string todoString = "";
                Label todoTitleLabel = new Label { Text = "To Do List", TextColor = Color.FromHex("006EFF"), FontSize = 15, HorizontalOptions = LayoutOptions.StartAndExpand };
                mainTodoStack.Children.Add(todoTitleLabel);
                if (Application.Current.Properties["todoistLogin"].ToString() == "f")
                {
                    Label errorLabel = new Label { Text = "You must sign into Todoist in the todo list settings to use the todo list briefing.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                    errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                    mainTodoStack.Children.Add(errorLabel);
                    return;
                }

                string[][] response = await DependencyService.Get<ITextToSpeech>().GetTodoistTasks();
                if (response.Length > 0)
                {
                    if (response[0][0] != "no account" && response[0][0] != "error")
                    {
                        if (response.Length > 0)
                            todoString = "Today you have ";
                        else
                            todoString += "You are all done with your to do list for the day!";
                        if (response.Length > 1)
                        {
                            todoString += response.Length.ToString() + " tasks left to do.-";
                        }
                        else if (response.Length == 1)
                        {
                            todoString += "only one task left to do.-";
                        }
                        Label firstLabel = new Label { Text = todoString.Replace("-", ""), FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand };
                        firstLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                        mainTodoStack.Children.Add(firstLabel);
                        if (Application.Current.Properties["showFullTodo"].ToString() == "t")
                        {
                            for (int i = 0; i < response.Length; i++)
                            {
                                mainTodoStack.Children.Add(new Image { Source = "line.png", HorizontalOptions = LayoutOptions.FillAndExpand });
                                Label taskLabel = new Label { Text = response[i][0], FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand, ClassId = response[i][1] };
                                TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
                                gestureRecognizer.Tapped += NewsGestureRecognizer_Tapped;
                                taskLabel.GestureRecognizers.Add(gestureRecognizer);
                                taskLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                                mainTodoStack.Children.Add(taskLabel);
                                todoString += response[i][0] + "-";
                                if(i == response.Length - 2)
                                {
                                    todoString += " and ";
                                }
                                if (i == response.Length - 1)
                                {
                                    mainTodoStack.Children.Add(new Image { Source = "line.png", HorizontalOptions = LayoutOptions.FillAndExpand });
                                }
                                todoString += "-";
                            }
                        }
                    }
                    else
                    {
                        if (response.Length > 0)
                        {
                            if (response[0][0] == "no account")
                            {
                                Label errorLabel = new Label { Text = "You must sign into Todoist in the todo list settings to use the todo list briefing.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                                errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                                mainTodoStack.Children.Add(errorLabel);
                            }
                            else
                            {
                                Label errorLabel = new Label { Text = "There was an error loading your todo list.", FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                                errorLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                                mainTodoStack.Children.Add(errorLabel);
                                DisplayAlert("Todo Loading Error", "There was an error loading your todo list", "OK");
                            }
                        }
                    }
                }
                else
                {
                    Label firstLabel = new Label { Text = "You are all done with your to do list for the day!", FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand };
                    firstLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                    mainTodoStack.Children.Add(firstLabel);
                }
                briefString += todoString;
            }
        }

        private void GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Other
        async void DisplayNoInternet()
        {
            //Display error message when no internet
            await DisplayAlert("Not Connected", "An internet connection is needed to get data for your briefing. Please connect and try again.", "RELOAD");
            Refresh();
        }

        public async void OpenSettings(object sender, EventArgs args)
        {
            await this.Navigation.PushAsync(new Settings());
        }

        public void OnAppearing(object sender, EventArgs args)
        {
            if (Application.Current.Properties["settingsChanged"].ToString() == "t" || MainStackLayout.Children.Count <= 1)
            {
                if (Application.Current.Properties["timeAppTheme"].ToString() == "t")
                {
                    DateTime lastSunset = DateTime.ParseExact(Application.Current.Properties["sunsetTime"].ToString(), "yyyy-MM-dd HH:mm tt", CultureInfo.InvariantCulture);
                    if (lastSunset.Year != 1)
                    {
                        if (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, lastSunset.Hour, lastSunset.Minute, 0) <= DateTime.Now)
                        {
                            Application.Current.Properties["appTheme"] = "dark";
                        }
                        else
                        {
                            Application.Current.Properties["appTheme"] = "light";
                        }
                    }
                }
                if (Application.Current.Properties["appTheme"].ToString() == "light")
                {
                    BackgroundColor = Color.FromHex("#FFFFFF");
                    Application.Current.Resources["mainLabelStyle"] = Resources["lightThemeLabelStyle"];
                    Application.Current.Resources["mainLineStyle"] = Resources["lightThemeLineStyle"];
                }
                else
                {
                    Application.Current.Resources["mainLabelStyle"] = Resources["darkThemeLabelStyle"];
                    Application.Current.Resources["mainLineStyle"] = Resources["darkThemeLineStyle"];
                    BackgroundColor = Color.FromHex("#3a3a3a");
                }
                MainStackLayout.Children.Clear();
                Application.Current.Properties["settingsChanged"] = "f";
                if (Convert.ToInt32(Application.Current.Properties["dailyRefreshes"].ToString()) >= 200)
                {
                    Label alertLabel = new Label { Text = "You've reached 200 briefings today which is the maximum amount of briefings we allow for one day. Come back tomorrow for some more.", FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.CenterAndExpand, HorizontalTextAlignment = TextAlignment.Center };
                    alertLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                    MainStackLayout.Children.Add(alertLabel);
                }
                else
                {
                    if (!InternetTest())
                    {
                        Label alertLabel = new Label { Text = "An internet connection is needed to get data for your briefing. Please connect and try again.", FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.CenterAndExpand, HorizontalTextAlignment = TextAlignment.Center };
                        alertLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                        LoadingStack.Children.Add(alertLabel);
                        DisplayNoInternet();
                    }
                    if (Application.Current.Properties["settingsChanged"].ToString() == "f" && MainStackLayout.Children.Count > 1)
                    {
                        return;
                    }
                    Refresh();
                }
            }
        }

        public void PlayBriefing(object sender, EventArgs e)
        {
            if (!briefingPlaying)
            {
                if (briefingDoneLoading)
                {
                    string[] temp = briefString.Split('-').Skip(currentUtterance).ToArray();
                    maxUtterance = temp.Length;
                    DependencyService.Get<ITextToSpeech>().SpeakText(temp);
                    briefingPlaying = true;
                    PlayButton.Icon = "pause.png";
                }
            }
            else
            {
                //stop playing
                currentUtterance = DependencyService.Get<ITextToSpeech>().StopSpeaking();
                if (currentUtterance == maxUtterance)
                {
                    currentUtterance = 0;
                    maxUtterance = 0;
                }
                briefingPlaying = false;
                PlayButton.Icon = "play.png";
            }
        }

        public bool InternetTest()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
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

            sunsetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunsetTime.Hour, sunsetTime.Minute, sunsetTime.Second, sunsetTime.Millisecond, System.DateTimeKind.Utc);

            sunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunriseTime.Hour, sunriseTime.Minute, sunriseTime.Second, sunriseTime.Millisecond, System.DateTimeKind.Utc);

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
        #endregion

        async void Refresh()
        {
            if(!InternetTest())
            {
                DisplayNoInternet();
                return;
            }
            
            //daily refresh limit reset
            DateTime lastRefresh = DateTime.ParseExact(Application.Current.Properties["lastRefresh"].ToString(), "yyyy-MM-dd HH:mm tt", CultureInfo.InvariantCulture);
            if(lastRefresh.ToShortDateString() != DateTime.Now.ToShortDateString())
            {
                Application.Current.Properties["dailyRefreshes"] = "0";
            }

            //one minute limit on refreshing
            lastRefresh = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, lastRefresh.Hour, lastRefresh.Minute, lastRefresh.Second, 0);
            if(lastRefresh > DateTime.Now.AddMinutes(-1) && MainStackLayout.Children.Count > 0 && !weatherLocationError)
            {
                refreshLayout.IsRefreshing = false;
                briefingDoneLoading = true;
                return;
            }
            weatherLocationError = false;

            //prevent user from refreshing more than 200 times in one day
            if (Convert.ToInt32(Application.Current.Properties["dailyRefreshes"].ToString()) >= 200)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Maximum Briefings Reached", "You've reached 200 briefings today which is the maximum amount of briefings we allow for one day. Come back tomorrow for some more. Daily: " + Application.Current.Properties["dailyRefreshes"].ToString(), "OK");
                });
                refreshLayout.IsRefreshing = false;
                briefingDoneLoading = true;
                Application.Current.Properties["dailyRefreshes"] = (Convert.ToInt32(Application.Current.Properties["dailyRefreshes"].ToString()) + 1).ToString();
                return;
            }
            else
            {
                Application.Current.Properties["dailyRefreshes"] = (Convert.ToInt32(Application.Current.Properties["dailyRefreshes"].ToString()) + 1).ToString();
            }

            //START REFRESHING
            LoadingStack.IsVisible = true;
            LoadingStack.Children.Clear();
            bool isDark = false;
            //theme
            if (Application.Current.Properties["timeAppTheme"].ToString() == "t")
            {
                DateTime lastSunset = DateTime.ParseExact(Application.Current.Properties["sunsetTime"].ToString(), "yyyy-MM-dd HH:mm tt", CultureInfo.InvariantCulture);
                if (lastSunset.Year != 1)
                {
                    if (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, lastSunset.Hour, lastSunset.Minute, 0) <= DateTime.Now)
                    {
                        Application.Current.Properties["appTheme"] = "dark";
                    }
                    else
                    {
                        Application.Current.Properties["appTheme"] = "light";
                    }
                }
            }

            if (Application.Current.Properties["appTheme"].ToString() == "light")
            {
                BackgroundColor = Color.FromHex("#FFFFFF");
                Application.Current.Resources["mainLabelStyle"] = Resources["lightThemeLabelStyle"];
                Application.Current.Resources["mainLineStyle"] = Resources["lightThemeLineStyle"];
            }
            else
            {
                BackgroundColor = Color.FromHex("#3a3a3a");
                Application.Current.Resources["mainLabelStyle"] = Resources["darkThemeLabelStyle"];
                Application.Current.Resources["mainLineStyle"] = Resources["darkThemeLineStyle"];
                isDark = true;
            }
            
            StartAbsoluteLayout.Children.Clear();
            MainStackLayout.Children.Clear();
            MainStackLayout.IsVisible = false;
            
            refreshLayout.IsRefreshing = true;
            briefingDoneLoading = false;
            
            //background
            Image backgroundImage = new Image { HorizontalOptions = LayoutOptions.FillAndExpand, WidthRequest = 800, HeightRequest = 300, Aspect = Aspect.AspectFill};
            if(DateTime.Now.Hour < 11)
            {
                backgroundImage.Source = morningImages[rand.Next(0, morningImages.Length)];
            }
            else if(DateTime.Now.Hour < 18)
            {
                backgroundImage.Source = dayImages[rand.Next(0, dayImages.Length)];
            }
            else
            {
                backgroundImage.Source = nightImages[rand.Next(0, nightImages.Length)];
            }
            StartAbsoluteLayout.Children.Add(backgroundImage, new Rectangle(0, 0, 800, 300));
            //gradient
            Image gradientImage = new Image { HorizontalOptions = LayoutOptions.FillAndExpand, WidthRequest = 800, HeightRequest = 100, Aspect = Aspect.AspectFill };
            if(Application.Current.Properties["appTheme"].ToString() == "light")
            {
                gradientImage.Source = "lightGradient.png";
            }
            else
            {
                gradientImage.Source = "darkGradient.png";
            }
            StartAbsoluteLayout.Children.Add(gradientImage, new Rectangle(0, 140, 400, 200));
            //title
            briefString = "";
            string greeting = "";
            double width = 0;
            if(DateTime.Now.Hour < 12)
            {
                int randint = rand.Next(0, 3);
                if(randint == 0)
                {
                    briefString = "Mornin' " + Application.Current.Properties["userName"].ToString() + ", ";
                    greeting = "Mornin'";
                }
                else
                {
                    briefString = "Good Morning " + Application.Current.Properties["userName"].ToString() + ", ";
                    greeting = "Good Morning";
                }
            }
            else if(DateTime.Now.Hour < 17)
            {
                int randint = rand.Next(0, 3);
                briefString = "Good Afternoon " + Application.Current.Properties["userName"].ToString() + ", ";
                greeting = "Good Afternoon";
            }
            else
            {
                briefString = "Good Evening " + Application.Current.Properties["userName"].ToString() + ", ";
                greeting = "Good Evening";
                width = 190;
            }
            StackLayout titleLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HeightRequest = 100, WidthRequest = 600 };
            Label startBriefLabel = new Label { Text = greeting, FontSize = 25, TextColor = Color.White, HorizontalOptions = LayoutOptions.Start };
            Label nameBriefLabel = new Label { Text = Application.Current.Properties["userName"].ToString(), FontSize = 25, TextColor = Color.White, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.StartAndExpand };
            titleLayout.Children.Add(startBriefLabel);
            titleLayout.Children.Add(nameBriefLabel);
            StartAbsoluteLayout.Children.Add(titleLayout, new Rectangle(15, 130, 320, 50));
            //quote
            string quote = "";
            while (true)
            {
                quote = quotes[rand.Next(0, quotes.Length)];
                if(quote.Length < 150)
                {
                    break;
                }
            }
            Label quoteLabel = new Label { Text = quote, FontSize = 17, FontAttributes = FontAttributes.Italic, TextColor = Color.White };
            if(quote.Length > 50)
            {
                quoteLabel.FontSize = 15;
            }
            StartAbsoluteLayout.Children.Add(quoteLabel, new Rectangle(15, 160, 320, 100));

            //create loading stack
            if (LoadingStack.Children.Count() == 0)
            {
                loadingLabel = new Label { Text = "Your Briefing is Loading", TextColor = Color.FromHex("1E1E1E"), HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = 20 };
                loadingProgressLabel = new Label { Text = "0%", FontSize = 20, TextColor = Color.FromHex("1E1E1E"), FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.CenterAndExpand };
                //loadingProgressLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                //loadingLabel.SetDynamicResource(VisualElement.StyleProperty, "mainLabelStyle");
                if (BackgroundColor == Color.FromHex("3a3a3a") || isDark)
                {
                    loadingLabel.TextColor = Color.White;
                    loadingProgressLabel.TextColor = Color.White;
                }
                LoadingStack.Children.Add(loadingLabel);

                LoadingStack.Children.Add(loadingProgressLabel);
            }
            //main briefing
            briefString += "-";
            string[] order = Application.Current.Properties["infoOrder"].ToString().Split('-');
            int enabledSections = 0;
            int totalEnabledSections = 0;
            for(int i = 0; i < order.Length; i++)
            {
                if(Application.Current.Properties[order[i] + "Section"].ToString() == "t")
                {
                    totalEnabledSections++;
                }
            }
            
            for (int i = 0; i < order.Length; i++)
            {
                if (BackgroundColor == Color.FromHex("3a3a3a") || BackgroundColor == Color.FromHex("#3a3a3a"))
                {
                    loadingLabel.TextColor = Color.White;
                    loadingProgressLabel.TextColor = Color.White;
                }
                else
                {
                    loadingLabel.TextColor = Color.FromHex("1E1E1E");
                    loadingProgressLabel.TextColor = Color.FromHex("1E1E1E");
                }
                if (order[i] == "weather")
                {
                    //Load Weather
                    if (Application.Current.Properties["weatherSection"].ToString() == "t" && MainStackLayout.Children.IndexOf(mainWeatherStack) == -1)
                    {
                        mainWeatherStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0 };
                        MainStackLayout.Children.Add(mainWeatherStack);
                        await LoadWeather();
                        enabledSections++;
                    }
                    else
                    {
                        //get sunset time indepentent of weather
                        HttpClient client = new HttpClient();
                        string lat = "";
                        string lon = "";
                        if (Application.Current.Properties["useLocationWeather"].ToString() == "t" || Application.Current.Properties["customWeatherLocationCoords"].ToString() == "0,0")
                        {
                            try
                            {
                                var locator = CrossGeolocator.Current;
                                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                                lat = position.Latitude.ToString();
                                lon = position.Longitude.ToString();
                            }
                            catch
                            {
                                DisplayAlert("Error finding location", "There was an error finding your location. Typically this is due to either having your location turned off, or not granting location permissions to the app.", "OK");
                                continue;
                            }
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
                    }
                }
                
                if (order[i] == "email")
                {
                    //Load Email
                    if (Application.Current.Properties["emailSection"].ToString() == "t" && MainStackLayout.Children.IndexOf(mainEmailStack) == -1)
                    {
                        mainEmailStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0 };
                        MainStackLayout.Children.Add(mainEmailStack);
                        await LoadEmails();
                        enabledSections++;
                    }
                }else if (order[i] == "calendar")
                {
                    //Load Calendar
                    if (Application.Current.Properties["calendarSection"].ToString() == "t" && MainStackLayout.Children.IndexOf(mainCalendarStack) == -1)
                    {
                        mainCalendarStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0 };
                        MainStackLayout.Children.Add(mainCalendarStack);
                        await LoadCalendarEvents();
                        enabledSections++;
                    }
                }
                else if(order[i] == "stocks")
                {
                    //Load stocks
                    if(Application.Current.Properties["stocksSection"].ToString() == "t" && MainStackLayout.Children.IndexOf(mainStocksStack) == -1)
                    {
                        mainStocksStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0 };
                        MainStackLayout.Children.Add(mainStocksStack);
                        await LoadStocks();
                        enabledSections++;
                    }
                }
                else if(order[i] == "travel")
                {
                    //Load travel
                    if (Application.Current.Properties["travelSection"].ToString() == "t")
                    {
                        
                        enabledSections++;
                    }
                }
                else if(order[i] == "news")
                {
                    //Load News
                    if (Application.Current.Properties["newsSection"].ToString() == "t" && MainStackLayout.Children.IndexOf(mainNewsStack) == -1)
                    {
                        mainNewsStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0 };
                        MainStackLayout.Children.Add(mainNewsStack);
                        await LoadAllNews();
                        enabledSections++;
                    }
                }
                else if(order[i] == "todo")
                {
                    //Load Todoist
                    if (Application.Current.Properties["todoSection"].ToString() == "t" && MainStackLayout.Children.IndexOf(mainTodoStack) == -1)
                    {
                        mainTodoStack = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 0};
                        MainStackLayout.Children.Add(mainTodoStack);
                        await LoadTodoist();
                        enabledSections++;
                    }
                }
                briefString += "-";
                loadingProgressLabel.Text = Math.Round(Convert.ToDouble(enabledSections) / Convert.ToDouble(totalEnabledSections) * 100, 0).ToString() + "%";
                
            }
            
            if (enabledSections != totalEnabledSections)
            {
                //correct the enabled sections count
                Application.Current.Properties["enabledSections"] = enabledSections.ToString();
            }
              
            if (briefString.ToLower().Contains("morning") || briefString.ToLower().Contains("afternoon"))
            {
                briefString += "Have a great day!";
            }
            else
            {
                briefString += "Have a great evening!";
            }
            Application.Current.Properties["lastRefresh"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm tt");
            LoadingStack.IsVisible = false;
            MainStackLayout.IsVisible = true;
            refreshLayout.IsRefreshing = false;
            briefingDoneLoading = true;
        }
    }
}
