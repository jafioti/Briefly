using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Briefing.iOS.iOSDependencyService))]
namespace Briefing.iOS
{
    public class iOSDependencyService : ITextToSpeech
    {
        public void MakeToast(string Message)
        {

        }

        public void SpeakText(string[] message)
        {
            var speechSynthesizer = new AVSpeechSynthesizer();

            var speechUtterance = new AVSpeechUtterance(message[0])
            {
                Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
                Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                Volume = 0.5f,
                PitchMultiplier = 1.0f
            };

            speechSynthesizer.SpeakUtterance(speechUtterance);
        }

        public void SynthText(string message)
        {

        }

        public int StopSpeaking()
        {
            return (0);
        }

        public void ResetCurrentUtterance()
        {

        }

        public void Login()
        {

        }

        public void TodoistLogin()
        {

        }

        public async Task<string[]> GetCalendars()
        {
            return (new string[0]);
        }

        public async Task<string[][]> GetCalendarEvents(string[] calendars)
        {
            return (new string[0][]);
        }

        public async Task<string[]> GetUnreadEmails(string query)
        {
            return (new string[0]);
        }

        public async Task<string> RefreshAccessToken()
        {
            return ("");
        }

        public bool GetAccount()
        {
            return (false);
        }

        public async Task<string[][]> GetTodoistTasks()
        {
            return (new string[0][]);
        }
    }
}