using System.Reactive.Subjects;
using NAudio.Wave;

namespace Project;

public class LocalAudioPlayer : IAudioPlayer
{
    public BehaviorSubject<AudioState?> AudioStateObservable { get; } = new(null);

    public void Load(Audio audio)
    {
        throw new NotImplementedException();
    }

    public void Play()
    {
        throw new NotImplementedException();
    }

    public void Pause()
    {
        throw new NotImplementedException();
    }

    public void Seek(TimeSpan position)
    {
        throw new NotImplementedException();
    }

    public void SetVolume(float volume)
    {
        throw new NotImplementedException();
    }
}