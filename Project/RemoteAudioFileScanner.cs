namespace Project;

public class RemoteAudioFileScanner : AudioFileScanner
{
    public override List<Audio> Scan(DirectoryInfo directory)
    {
        return new List<Audio>();
    }
}