using System.Reactive.Subjects;

namespace Project.presentation;

public class MainViewModel
{
    public BehaviorSubject<MainViewState> MainViewStateObservable { get; } =
        new(new MainViewState());

    private readonly AudioFileScanner _audioFileScanner = new();
    private readonly IAudioPlayer _audioPlayer = new LocalAudioPlayer();

    public MainViewModel()
    {
        SettingsManager.SettingsObservable.Subscribe(settings =>
        {
            DirectoryInfo audioDirectory;

            if (settings.UseLocalAudioDirectory)
                audioDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            else
                audioDirectory = new DirectoryInfo(settings.AudioDirectory);

            var audios = _audioFileScanner.Scan(audioDirectory);

            MainViewStateObservable.OnNext(
                MainViewStateObservable.Value with { Audios = audios }
            );
        });

        _audioPlayer.AudioStateObservable.SubscribeDistinct(audioState =>
        {
            MainViewStateObservable.OnNext(
                MainViewStateObservable.Value with
                {
                    CurrentAudioState = audioState
                }
            );
        });
    }

    public void LoadAudio(Audio audio)
    {
        _audioPlayer.Load(audio);
    }
    
    public void PlayAudio()
    {
        _audioPlayer.Play();
    }

    public void PauseAudio()
    {
        _audioPlayer.Pause();
    }

    public void SeekAudio(float audioProgress)
    {
        var newPosition = MainViewStateObservable.Value.CurrentAudioState?.AudioLength
            .Multiply(audioProgress);
        if (newPosition == null) return;

        _audioPlayer.Seek(newPosition.Value);
    }

    public void SetAudioVolume(float volume)
    {
        _audioPlayer.SetVolume(volume);
    }
}