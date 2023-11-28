using System.Reactive.Subjects;

namespace Project.presentation;

public delegate void SettingsViewStateChanged(SettingsViewState state);

public class SettingsViewModel
{
    private readonly SettingsViewStateChanged _settingsViewStateChanged;
    private SettingsViewState _settingsViewState;

    public SettingsViewModel(SettingsViewStateChanged settingsViewStateChanged)
    {
        _settingsViewStateChanged = settingsViewStateChanged;
        SettingsManager.SettingsObservable.Subscribe(settings =>
            UpdateSettingsViewState(new SettingsViewState(settings))
        );
    }

    public void SetAudioDirectory(string directory)
    {
        var newSettingsViewState = new SettingsViewState(
            _settingsViewState.Settings with { AudioDirectory = directory }
        );

        UpdateSettingsViewState(newSettingsViewState);
    }

    public void SetUseLocalAudioDirectory(bool useLocalAudioDirectory)
    {
        var newSettingsViewState = new SettingsViewState(
            _settingsViewState.Settings with
            {
                UseLocalAudioDirectory = useLocalAudioDirectory
            }
        );

        UpdateSettingsViewState(newSettingsViewState);
    }

    private void UpdateSettingsViewState(SettingsViewState state)
    {
        if(_settingsViewState == state) return;
        _settingsViewState = state;
        _settingsViewStateChanged.Invoke(state);
    }
    
    public void SaveSettings()
    {
        SettingsManager.SaveSettings(_settingsViewState.Settings);
    }
}