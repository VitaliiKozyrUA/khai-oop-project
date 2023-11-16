using NAudio.Wave;

namespace Project;

public record AudioState(
    Audio CurrentAudio,
    PlaybackState PlaybackState,
    TimeSpan AudioLength,
    TimeSpan CurrentPosition,
    float Volume
)
{
    public AudioState() : this(
        new Audio(),
        PlaybackState.Stopped,
        TimeSpan.Zero,
        TimeSpan.Zero,
        0f
    )
    {
    }
};