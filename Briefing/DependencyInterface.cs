using System;
using System.Threading.Tasks;

namespace Briefing
{
    public interface ITextToSpeech
    {
        void MakeToast(string Message);

        void SpeakText(string[] message);

        void SynthText(string message);

        int StopSpeaking();

        void ResetCurrentUtterance();

        void Login();

        void TodoistLogin();

        Task<string[]> GetCalendars();

        Task<string[][]> GetCalendarEvents(string[] calendars);

        Task<string[]> GetUnreadEmails(string query);

        Task<string> RefreshAccessToken();

        bool GetAccount();

        Task<string[][]> GetTodoistTasks();
    }
}
