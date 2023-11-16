namespace Project.presentation;

public record SettingsViewState(Settings Settings)
{
    public SettingsViewState() : this(new Settings())
    { }
};