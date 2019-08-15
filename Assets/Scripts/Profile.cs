using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

// This is NOT a MonoBehavior.
[System.Serializable]
public class Profile
{
    // Unlocks and features
    public List<int> unlockedColors;
    public bool canSkipIntro;
    public bool canAgreeToLicensesImmediately;
    public bool canSkipLicenseQuiz;
    public bool noUpdates;
    public bool canSkipSettings;
    public bool canSkipStory;
    public bool canSkipTutorial;

    // Settings
    public int difficulty;
    public int brightness;
    public int resolutionWidth;
    public int resolutionHeight;
    public int refreshRate;
    public bool fullscreen;
    public int musicVolume;
    public int sfxVolume;
    public int voiceVolume;
    public bool subtitles;
    public int colorIndex;

    public Profile()
    {
        unlockedColors = new List<int>();
        unlockedColors.Add(215);  // White
        canSkipIntro = false;
        canAgreeToLicensesImmediately = false;
        canSkipLicenseQuiz = false;
        noUpdates = false;
        canSkipSettings = false;
        canSkipStory = false;
        canSkipTutorial = false;

        difficulty = 1;
        brightness = 10;
        resolutionWidth = 1920;
        resolutionHeight = 1080;
        refreshRate = 60;
        fullscreen = true;
        musicVolume = 10;
        sfxVolume = 10;
        voiceVolume = 10;
        subtitles = true;
        colorIndex = 215;  // White
    }
}

public static class ProfileManager
{
    public static Profile inMemoryProfile;

    static ProfileManager()
    {
        inMemoryProfile = new Profile();
    }

    private static string ProfileFilePath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            + "\\Yaac\\profile.bin";
    }

    // Throws FileNotFoundException if file not found.
    public static void LoadFromFile()
    {
        FileStream stream = new FileStream(ProfileFilePath(), FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        inMemoryProfile = (Profile)formatter.Deserialize(stream);
        stream.Close();
    }
    
    public static void SaveToFile()
    {
        FileStream stream = new FileStream(ProfileFilePath(), FileMode.Truncate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, inMemoryProfile);
        stream.Close();
    }

    public static void CreateAndSave()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ProfileFilePath()));
        File.Create(ProfileFilePath()).Close();
        inMemoryProfile = new Profile();
        SaveToFile();
    }
}
