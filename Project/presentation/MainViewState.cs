namespace Project.presentation;

public record MainViewState(
    AudioState? CurrentAudioState,
    List<Audio> Audios
)
{
    public MainViewState() : this(
        null,
        new List<Audio>()
    )
    {
    }
};