using System.Diagnostics;
using System.Management;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PenShortcutService;

public class WindowsBackgroundService(PenShortcutService service, ILogger<WindowsBackgroundService> logger) : BackgroundService
{
    private readonly EventLog _log = new EventLog(Program.EVENTLOG_NAME);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.EntryWritten += OnEventWritten;
        _log.EnableRaisingEvents = true;
        _log.Source = Program.EVENTLOG_NAME + "Source";

        // Keep the service running until cancellation is requested
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void OnEventWritten(object sender, EntryWrittenEventArgs e)
    {
        logger.LogInformation("Received event log entry");

        if (e.Entry is not null && e.Entry.Message == "TRIGGER")
        {
            
            var result = service.ToggleTouchscreen();
            logger.LogInformation("Resulted in: " + result);

            if (result == ToggleResult.Fail) return;
            
            _log.WriteEntry(result == ToggleResult.Enabled ? "ENABLED" : "DISABLED", EventLogEntryType.Information);
        }
        
    }
    
    
}