﻿namespace Project;

public class LocalAudioFileScanner : AudioFileScanner
{
    public override List<Audio> Scan(DirectoryInfo directory)
    {
        var audios = new List<Audio>();

        if (!directory.Exists) return new List<Audio>();
        
        foreach (var fileInfo in directory.GetFiles())
        {
            try
            {
                audios.Add(AudioFileParser.Parse(fileInfo));
            }
            catch (ParseException e)
            {
            }
        }

        return audios;
    }
}