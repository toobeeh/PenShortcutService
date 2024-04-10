using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

namespace PenShortcutService;

class Program
{
    public static readonly string EVENTLOG_NAME = "PENSSSERVICE";
    
    static void Main(string[] args)
    {
        // Create an event source if it doesn't exist
        if (!EventLog.SourceExists(EVENTLOG_NAME + "Source"))
        {
            EventLog.CreateEventSource(EVENTLOG_NAME + "Source", EVENTLOG_NAME);
        }
        
        
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddWindowsService(options =>
        {
            options.ServiceName = "Pen Shortcut Service";
        });

        LoggerProviderOptions.RegisterProviderOptions<
            EventLogSettings, EventLogLoggerProvider>(builder.Services);

        builder.Services.AddSingleton<PenShortcutService>();
        builder.Services.AddHostedService<WindowsBackgroundService>();

        IHost host = builder.Build();
        host.Run();
    }
}