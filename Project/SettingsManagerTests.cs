namespace Project;

using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class SettingsManagerTests
{
    [TestMethod]
    public void SettingsObservable_NotNull()
    {
        var settingsObservable = SettingsManager.SettingsObservable;
        
        Assert.IsNotNull(settingsObservable);
    }

    [TestMethod]
    public void SettingsObservable_DefaultSettings()
    {
        var settingsObservable = SettingsManager.SettingsObservable.Value;
        
        Assert.AreEqual("directory", settingsObservable.AudioDirectory);
        Assert.IsFalse(settingsObservable.UseLocalAudioDirectory);
    }

    [TestMethod]
    public void SaveSettings_UpdatesSettings()
    {
        var newSettings = new Settings("directory", false);
        
        SettingsManager.SaveSettings(newSettings);
        var updatedSettings = SettingsManager.SettingsObservable.Value;

        Assert.AreEqual(newSettings.AudioDirectory, updatedSettings.AudioDirectory);
        Assert.AreEqual(newSettings.UseLocalAudioDirectory, updatedSettings.UseLocalAudioDirectory);
    }
}