using System.Reactive.Subjects;

namespace Project.presentation;

public class SettingsViewModel
{
    public BehaviorSubject<SettingsViewState> SettingsViewStateObservable { get; } =
        new(new SettingsViewState());

    public SettingsViewModel()
    {
        SettingsManager.SettingsObservable.Subscribe(settings =>
            SettingsViewStateObservable.OnNext(new SettingsViewState(settings))
        );
    }

    public void SetAudioDirectory(string directory)
    {
        var newSettingsViewState = new SettingsViewState(
            SettingsViewStateObservable.Value.Settings with { AudioDirectory = directory }
        );

        SettingsViewStateObservable.OnNext(newSettingsViewState);
    }

    public void SetUseLocalAudioDirectory(bool useLocalAudioDirectory)
    {
        var newSettingsViewState = new SettingsViewState(
            SettingsViewStateObservable.Value.Settings with
            {
                UseLocalAudioDirectory = useLocalAudioDirectory
            }
        );

        SettingsViewStateObservable.OnNext(newSettingsViewState);
    }

    public void SaveSettings()
    {
        SettingsManager.SaveSettings(SettingsViewStateObservable.Value.Settings);
    }
}