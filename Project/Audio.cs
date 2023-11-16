namespace Project;

public record Audio(
    FileInfo File,
    AudioFormat Format,
    string Name,
    string Author
)
{
    public Audio() : this(
        new FileInfo("/"),
        AudioFormat.Mp3,
        "",
        ""
    )
    {
    }
}