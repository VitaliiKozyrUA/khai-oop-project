using NAudio.Wave;
using Terminal.Gui;

namespace Project.presentation;

public class MainWindow : Window
{
    private readonly MainViewModel _viewModel = new();

    private MainViewState MainViewState => _viewModel.MainViewStateObservable.Value;

    private readonly Button _buttonSettings = new("Settings", is_default: true);
    private readonly FrameView _audioListFrame = new();
    private readonly ListView _listViewAudio = new();
    private readonly FrameView _controlsFrame = new();
    private readonly Label _labelCurrentAudio = new("Audio name");
    private readonly Label _labelCurrentAudioPosition = new("");
    private readonly Label _labelCurrentAudioLength = new("");
    private readonly ProgressBar _progressBarAudioTrack = new();
    private readonly Button _buttonPlayPause = new("Play", is_default: true);
    private readonly Label _labelVolume = new("Volume");
    private readonly ProgressBar _progressBarAudioVolume = new();

    public MainWindow()
    {
        Title = " Audio Player (Ctrl+Q to quit)";

        _buttonSettings.X = Pos.AnchorEnd(15);

        _listViewAudio.X = 1;
        _listViewAudio.Width = Dim.Fill();
        _listViewAudio.Height = Dim.Fill();

        _audioListFrame.Y = 1;
        _audioListFrame.Width = Dim.Percent(50);
        _audioListFrame.Height = Dim.Fill();
        _audioListFrame.Title = " Available audio";
        _audioListFrame.Add(_listViewAudio);

        _controlsFrame.X = Pos.Right(_audioListFrame) + 1;
        _controlsFrame.Y = 1;
        _controlsFrame.Width = Dim.Fill();
        _controlsFrame.Height = Dim.Sized(6);
        _controlsFrame.Title = " Audio controls";
        _controlsFrame.Visible = false;

        _labelCurrentAudio.X = Pos.Center();

        _labelCurrentAudioPosition.X = 3;
        _labelCurrentAudioLength.X = Pos.AnchorEnd(11);

        _progressBarAudioTrack.X = 3;
        _progressBarAudioTrack.Y = Pos.Bottom(_labelCurrentAudio);
        _progressBarAudioTrack.Width = Dim.Fill(3);
        _progressBarAudioTrack.ProgressBarStyle = ProgressBarStyle.Continuous;

        _labelVolume.X = 3;
        _labelVolume.Y = Pos.Bottom(_progressBarAudioTrack) + 1;
        
        _progressBarAudioVolume.X = Pos.Right(_labelVolume) + 1;
        _progressBarAudioVolume.Y = Pos.Bottom(_progressBarAudioTrack) + 1;
        _progressBarAudioVolume.Width = Dim.Sized(15);
        _progressBarAudioVolume.ProgressBarStyle = ProgressBarStyle.Continuous;
        
        _buttonPlayPause.X = Pos.Center();
        _buttonPlayPause.Y = Pos.Bottom(_progressBarAudioTrack);
        _buttonPlayPause.NoDecorations = true;
        
        _controlsFrame.Add(
            _labelCurrentAudio,
            _progressBarAudioTrack,
            _labelCurrentAudioPosition,
            _labelCurrentAudioLength,
            _buttonPlayPause,
            _labelVolume,
            _progressBarAudioVolume
        );

        Add(
            _buttonSettings,
            _audioListFrame,
            _controlsFrame
        );

        AssignListeners();
    }

    private void AssignListeners()
    {
        _viewModel.MainViewStateObservable
            .SubscribeDistinct(UpdateViewState);

        _buttonSettings.Clicked += (_, _) => { Application.Run(new SettingsDialog()); };

        _listViewAudio.SelectedItemChanged += (_, args) =>
        {
            var selectedAudio = MainViewState.Audios.ElementAtOrDefault(args.Item);
            if (selectedAudio == null) return;
            _viewModel.LoadAudio(selectedAudio);
            _viewModel.PlayAudio();
        };

        _progressBarAudioTrack.MouseClick += (_, args) =>
        {
            var audioProgress = (float)args.MouseEvent.X / args.MouseEvent.View.Bounds.Width;
            _viewModel.SeekAudio(audioProgress);
        };
        
        _progressBarAudioVolume.MouseClick += (_, args) =>
        {
            var audioVolume = (float)args.MouseEvent.X / args.MouseEvent.View.Bounds.Width;
            _viewModel.SetAudioVolume(audioVolume);
        };

        _buttonPlayPause.Clicked += (_, _) =>
        {
            if (MainViewState.CurrentAudioState?.PlaybackState == PlaybackState.Playing)
                _viewModel.PauseAudio();
            else
                _viewModel.PlayAudio();
        };
    }
    
    private void UpdateViewState(MainViewState state)
    {
        _listViewAudio.SetSource(state.Audios.Select(audio => "[" + audio.Author + "] " + audio.Name).ToList());
        
        if (state.CurrentAudioState != null) UpdateControls(state.CurrentAudioState);
        
        Application.Wakeup();
    }

    private void UpdateControls(AudioState audioState)
    {
        _labelCurrentAudioPosition.Text = FormatTime(audioState.CurrentPosition);
        _labelCurrentAudioLength.Text = FormatTime(audioState.AudioLength);

        var audioName = audioState.CurrentAudio.Name;
        if (audioName.Length > 25) audioName = audioName.Substring(0, 25) + "...";
        _labelCurrentAudio.Text = audioName;

        _progressBarAudioTrack.Fraction =
            (float)audioState.CurrentPosition.Ticks / audioState.AudioLength.Ticks;

        _progressBarAudioVolume.Fraction = audioState.Volume;

        _buttonPlayPause.Text = audioState.PlaybackState == PlaybackState.Playing ? " \u23f8 " : " \u23f5 ";

        _controlsFrame.Visible = audioState.PlaybackState != PlaybackState.Stopped;
    }

    private string FormatTime(TimeSpan time)
    {
        return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }
}