namespace Project;

public static class AudioFileParser
{
    public static Audio Parse(FileInfo file)
    {
        var format = GetAudioFormat(file.Extension.TrimStart('.'));
        var name = ParseAudioName(file.Name.Replace(file.Extension, ""));
        var author = ParseAudioAuthor(file.Name.Replace(file.Extension, ""));
        
        return new Audio(file, format, name, author);
    }
    
    private static AudioFormat GetAudioFormat(string fileExtension)
    {
        foreach (AudioFormat enumValue in Enum.GetValues(typeof(AudioFormat)))
        {
            if (enumValue.ToString().ToLower().Equals(fileExtension.ToLower()))
            {
                return enumValue;
            }
        }
        
        throw new ParseException();
    }
    
    private static string ParseAudioAuthor(string fileName)
    {
        var components = fileName.Split('-');
        return components.Length > 0 ? components[0].Trim() : throw new ParseException();
    }
    
    private static string ParseAudioName(string fileName)
    {
        var components = fileName.Split('-');
        return components.Length > 1 ? components[1].Trim() : throw new ParseException();
    }
}