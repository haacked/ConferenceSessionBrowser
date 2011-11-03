using System;
using System.Collections.Generic;
using System.Linq;
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
            var xmlFilename = HttpContext.Current.Server.MapPath("~/App_Data/Sessions/SessionsData.xml");
            var xml = XDocument.Load(xmlFilename);
            var sessions = from track in xml.Root.Element("tracks").Elements("track")
                           let allSpeakers = xml.Root.Element("speakers").Elements("speaker")
                           let trackName = track.Attribute("name").Value
                           let allSessions = track.Element("sessions").Elements("session")
                           from session in allSessions
                           select new Session
                           {
                               Room = trackName,
                               Code = session.Attribute("id").Value,
                               DateText = session.Attribute("startTime").Value,
                               StartDate = Convert.ToDateTime(session.Attribute("startTime").Value),
                               Title = session.Attribute("name").Value.Trim(),
                               Description = session.Element("description").Value.Trim(),
                               Speakers = from sessionSpeaker in session.Element("speakers").Elements("speaker")
                                          join speaker in allSpeakers on sessionSpeaker.Attribute("id").Value equals speaker.Attribute("id").Value
                                          select speaker.Attribute("name").Value.Trim(),
                               Tags = from tag in session.Element("tags").Elements("tag")
                                      where tag != null && tag.Attribute("name") != null &&
                                      !String.IsNullOrEmpty(tag.Attribute("name").Value)
                                      select tag.Attribute("name").Value.Trim()
                           };

            All = sessions.ToList();
        }
    }
}