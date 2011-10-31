using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace ConferenceSessionBrowser
{
    // Could be loaded from a DB, but an in-memory collection is fine for this example
    public static class Sessions
    {
        public static IList<Session> All { get; private set; }

        static Sessions()
        {
            // Parse the XML
            string pathFormat = "~/App_Data/Sessions/{0}SessionsData.xml";
            string path = String.Format(pathFormat, Config.ConferenceId);
            var xmlFilename = HttpContext.Current.Server.MapPath(path);
            var xml = XDocument.Load(xmlFilename);
            var sessions = from tr in xml.Root.Elements("tr")
                           let cols = tr.Elements().ToArray()
                           select new Session
                           {
                               Room = cols[1].Value,
                               Code = cols[2].Value,
                               Title = ToOneLine(cols[3].Value),
                               Speakers = cols[4].Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()),
                               Tags = cols[5].Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()),
                               DateText = MakeSessionDateText(SessionNumberToDate(cols[0].Value), 60),
                               StartDate = SessionNumberToDate(cols[0].Value),
                               Description = "Session description not available"
                           };
            All = sessions.ToList();
        }

        private static string ToOneLine(string value)
        {
            value = value.Replace("\n", " ");
            value = Regex.Replace(value, "/s+", " ");
            return value.Trim();
        }

        private static string MakeSessionDateText(DateTime startTime, int durationMinutes)
        {
            return startTime.ToString("h:mm tt") + " - " + startTime.AddMinutes(durationMinutes).ToString("h:mm tt") + ", " + startTime.ToString("ddd MMM d, yyyy");
        }

        private static DateTime SessionNumberToDate(string sessionNumber)
        {
            var number = int.Parse(sessionNumber.Substring(8));
            switch (number)
            {
                case 1: return new DateTime(2011, 9, 14, 11, 30, 0);
                case 2: return new DateTime(2011, 9, 14, 14, 0, 0);
                case 3: return new DateTime(2011, 9, 14, 15, 30, 0);
                case 4: return new DateTime(2011, 9, 14, 17, 0, 0);
                case 5: return new DateTime(2011, 9, 15, 9, 0, 0);
                case 6: return new DateTime(2011, 9, 15, 10, 30, 0);
                case 7: return new DateTime(2011, 9, 15, 13, 0, 0);
                case 8: return new DateTime(2011, 9, 15, 14, 30, 0);
                case 9: return new DateTime(2011, 9, 15, 16, 0, 0);
                case 10: return new DateTime(2011, 9, 16, 9, 0, 0);
                case 11: return new DateTime(2011, 9, 16, 10, 30, 0);
                case 12: return new DateTime(2011, 9, 16, 12, 30, 0);
                case 13: return new DateTime(2011, 9, 16, 14, 0, 0);
            }
            throw new ArgumentException("Unexpected session number: " + sessionNumber);
        }
    }
}