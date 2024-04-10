using System.Management;
using Microsoft.Extensions.Logging;

namespace PenShortcutService;
public enum ToggleResult
{
    Fail,
    Disabled,
    Enabled
}
public class PenShortcutService(ILogger<PenShortcutService> logger)
{
    
    public ToggleResult ToggleTouchscreen()
    {
        logger.LogInformation("Toggling touchscreen state");
        string deviceName = "HID-compliant touch screen"; 
        ManagementObject device = GetDeviceByName(deviceName);
        if (device is null)
        {
            logger.LogInformation($"Device with name {deviceName} not found.");
            return ToggleResult.Fail;
        }
        
        bool isEnabled = (string)device["Status"] == "OK"; // "Status" property indicates the device's enabled/disabled state
        
        // Toggle the device state
        try
        {
            if (!isEnabled) device.InvokeMethod("Enable", null);
            else device.InvokeMethod("Disable", null);
        }
        catch
        {
            // for some reason this is expected to fail
        }
        
        // get device again and check
        device = GetDeviceByName(deviceName);
        if (device == null) return ToggleResult.Fail;
        
        var toggled = isEnabled != ((string)device["Status"] == "OK");
        if (!toggled) return ToggleResult.Fail;

        return isEnabled ? ToggleResult.Disabled : ToggleResult.Enabled;
    }
    
    ManagementObject GetDeviceByName(string deviceName)
    {
        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Name = '" + deviceName + "'"))
        {
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj;
            }
        }
        return null;
    }
}