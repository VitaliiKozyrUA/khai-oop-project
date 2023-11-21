namespace Project;

public abstract class AudioFileScanner
{
    public abstract List<Audio> Scan(DirectoryInfo directory);
}