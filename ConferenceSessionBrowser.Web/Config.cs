using System.Configuration;

namespace ConferenceSessionBrowser
{
    public static class Config
    {
        public static string ConferenceId
        {
            get
            {
                return ConfigurationManager.AppSettings["ConferenceId"] as string;
            }
        }
    }
}