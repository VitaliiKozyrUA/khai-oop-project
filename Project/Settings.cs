namespace Project;

public record Settings(
    string AudioDirectory = "",
    bool UseLocalAudioDirectory = true
);