using Terminal.Gui;

namespace Project.presentation;

public class SettingsDialog : Dialog
{
    private readonly SettingsViewModel _viewModel;
    private readonly Action _beep = Console.Beep;
    private readonly Action<int, int> _beepWithDuration = Console.Beep;
    
    private readonly Button _buttonSave = new("Save", is_default: true);
    private readonly Button _buttonCancel = new("Cancel", is_default: true);
    private readonly Label _labelAudioDirectory = new("Audio directory:");
    private readonly TextField _textFieldAudioDirectory = new("");
    private readonly CheckBox _useLocalDirCheckBox = new("Use local audio directory");

    public SettingsDialog()
    {
        _viewModel = new(UpdateViewState);
        
        Width = Dim.Sized(50);
        Height = Dim.Sized(7);

        _labelAudioDirectory.X = 1;

        _textFieldAudioDirectory.X = Pos.Right(_labelAudioDirectory) + 1;
        _textFieldAudioDirectory.Y = Pos.Top(_labelAudioDirectory);
        _textFieldAudioDirectory.Width = Dim.Fill();

        _useLocalDirCheckBox.X = 1;
        _useLocalDirCheckBox.Y = Pos.Bottom(_textFieldAudioDirectory);

        AssignListeners();

        AddButton(_buttonSave);
        AddButton(_buttonCancel);
        
        Add(_labelAudioDirectory, _textFieldAudioDirectory, _useLocalDirCheckBox);
    }
    
    private void AssignListeners()
    {
        _textFieldAudioDirectory.TextChanging += (_, e) =>
        {
            _viewModel.SetAudioDirectory(e.NewText);
        };

        _useLocalDirCheckBox.Toggled += (_, e) =>
        {
            _beepWithDuration(1000, 100);
            _viewModel.SetUseLocalAudioDirectory(e.NewValue!.Value);
        };

        _buttonSave.Clicked += (_, e) =>
        {
            _beep();
            _viewModel.SaveSettings();
            Application.RequestStop();
        };

        _buttonCancel.Clicked += (_, e) =>
        {
            _beep();
            Application.RequestStop();
        };
    }

    private void UpdateViewState(SettingsViewState state)
    {
        _textFieldAudioDirectory.Text = state.Settings.AudioDirectory;
        _useLocalDirCheckBox.Checked = state.Settings.UseLocalAudioDirectory;
    }
}