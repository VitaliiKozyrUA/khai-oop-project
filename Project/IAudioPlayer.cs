using System.Reactive.Subjects;

namespace Project;

public interface IAudioPlayer
{
    BehaviorSubject<AudioState?> AudioStateObservable { get; }
    void Load(Audio audio);
    void Play();
    void Pause();
    void Seek(TimeSpan position);
    void SetVolume(float volume);
}