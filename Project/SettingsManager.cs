using System.Reactive.Subjects;
using Newtonsoft.Json;

namespace Project;

public static class SettingsManager
{
    private static readonly string SettingsFileName = "settings.dat";

    public static BehaviorSubject<Settings> SettingsObservable { get; } = new (GetSettings());
    
    private static Settings GetSettings()
    {
        string settingsJsonText;
        var settings = new Settings();

        try
        {
            settingsJsonText = File.ReadAllText(SettingsFileName);
        }
        catch (FileNotFoundException e)
        {
            return settings;
        }
        
        try
        {
            var newSettings = JsonConvert.DeserializeObject<Settings>(settingsJsonText);
            if (newSettings != null)
            {
                settings = newSettings;
            }
        }
        catch (JsonException e)
        {
        }
        
        return settings;
    }

    public static void SaveSettings(Settings settings)
    {
        var settingsJsonText = JsonConvert.SerializeObject(settings);
        File.WriteAllText(SettingsFileName, settingsJsonText);
        SettingsObservable.OnNext(settings);
    }
}