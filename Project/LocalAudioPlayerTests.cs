using NAudio.Wave;

namespace Project;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

[TestClass]
public class LocalAudioPlayerTests
{
    [TestMethod]
    public void Play_UpdatesPlaybackState()
    {
        var audioPlayer = new LocalAudioPlayer();
        var audioFile = new FileInfo("test_audio.mp3");
        var audio = new Audio(audioFile, AudioFormat.Mp3, "Name", "Author");

        audioPlayer.Load(audio);

        audioPlayer.Play();

        Assert.AreEqual(PlaybackState.Playing, audioPlayer.AudioStateObservable.Value!.PlaybackState);
    }

    [TestMethod]
    public void Pause_UpdatesPlaybackState()
    {
        var audioPlayer = new LocalAudioPlayer();
        var audioFile = new FileInfo("test_audio.mp3");
        var audio = new Audio(audioFile, AudioFormat.Mp3, "Name", "Author");

        audioPlayer.Load(audio);
        audioPlayer.Play();
        
        audioPlayer.Pause();
        
        Assert.AreEqual(PlaybackState.Paused, audioPlayer.AudioStateObservable.Value!.PlaybackState);
    }

    [TestMethod]
    public void Seek_ChangesCurrentPosition()
    {
        var audioPlayer = new LocalAudioPlayer();
        var audioFile = new FileInfo("test_audio.mp3");
        var audio = new Audio(audioFile, AudioFormat.Mp3, "Name", "Author");

        audioPlayer.Load(audio);
        var newPosition = TimeSpan.FromSeconds(10);

        audioPlayer.Seek(newPosition);

        Assert.AreEqual(newPosition, audioPlayer.AudioStateObservable.Value!.CurrentPosition);
    }

    [TestMethod]
    public void SetVolume_ChangesVolume()
    {
        var audioPlayer = new LocalAudioPlayer();
        var audioFile = new FileInfo("test_audio.mp3");
        var audio = new Audio(audioFile, AudioFormat.Mp3, "Name", "Author");

        audioPlayer.Load(audio);
        var newVolume = 0.5f;
        
        audioPlayer.SetVolume(newVolume);
        
        Assert.AreEqual(newVolume, audioPlayer.AudioStateObservable.Value!.Volume, 0.1f);
    }
}