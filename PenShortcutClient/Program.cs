using System.Diagnostics;

namespace PenShortcutClient;

class Program
{
    public static readonly string EVENTLOG_NAME = "PENSSSERVICE";
    static async Task Main(string[] args)
    {
        
        // Log an event
        using (EventLog eventLog = new EventLog(EVENTLOG_NAME))
        {
            eventLog.Source = EVENTLOG_NAME + "Source";
            eventLog.WriteEntry("TRIGGER", EventLogEntryType.Information);

        }
    }
}