using System.Reactive.Subjects;
using NAudio.Wave;

namespace Project;

public class LocalAudioPlayer : IAudioPlayer, IDisposable
{
    private readonly WaveOutEvent _outputDevice = new();
    private AudioFileReader? _audioFileReader;
    private readonly System.Timers.Timer _updateTimer;

    public BehaviorSubject<AudioState?> AudioStateObservable { get; } = new(null);

    public LocalAudioPlayer()
    {
        _updateTimer = new System.Timers.Timer(100);
        _updateTimer.Elapsed += (_, _) => UpdateState();
        _updateTimer.AutoReset = true;
        _updateTimer.Enabled = true;
    }

    private void UpdateState(Audio? audio = null)
    {
        if (_audioFileReader == null) return;

        var state = (AudioStateObservable.Value ?? new AudioState()) with
        {
            PlaybackState = _outputDevice.PlaybackState,
            AudioLength = _audioFileReader.TotalTime,
            CurrentPosition = _audioFileReader.CurrentTime,
            Volume = _outputDevice.Volume
        };

        if (audio != null) state = state with { CurrentAudio = audio };

        AudioStateObservable.OnNext(state);
    }

    public void Load(Audio audio)
    {
        _audioFileReader = new AudioFileReader(audio.File.FullName);
        _outputDevice.Dispose();
        _outputDevice.Init(_audioFileReader);

        UpdateState(audio);
    }

    public void Play()
    {
        _outputDevice.Play();
        UpdateState();
    }

    public void Pause()
    {
        _outputDevice.Pause();
        UpdateState();
    }

    public void Seek(TimeSpan position)
    {
        if (_audioFileReader == null) return;
        _audioFileReader.CurrentTime = position;
        UpdateState();
    }

    public void SetVolume(float volume)
    {
        _outputDevice.Volume = volume;
        UpdateState();
    }

    public void Dispose()
    {
        _audioFileReader?.Dispose();
        _outputDevice.Dispose();
    }
}