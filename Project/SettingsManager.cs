using System.Reactive.Subjects;
using Newtonsoft.Json;

namespace Project;

public static class SettingsManager
{
    public static BehaviorSubject<Settings> SettingsObservable { get; } = new (GetSettings());
    
    private static Settings GetSettings()
    {
        throw new NotImplementedException();
    }

    public static void SaveSettings(Settings settings)
    {
        throw new NotImplementedException();
    }
}