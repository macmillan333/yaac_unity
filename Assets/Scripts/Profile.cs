using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public enum Enhancement
{
    IntroSkip = 0,
    InstantAgree = 1,
    QuizSkip = 2,
    StorySkip = 3,
    UpdateSkip = 4,
    SetupSkip = 5,
    TutorialSkip = 6,

    Count = 7
}

// This is NOT a MonoBehavior.
[System.Serializable]
public class Profile
{
    // Unlocks and enhancements
    public List<int> unlockedColors;
    public List<bool> unlockedEnhancements;  // Indexed by Enhancement value

    // Currency
    public int gems;

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
        UnlockColor(215);  // White
        unlockedEnhancements = new List<bool>();
        for (int i = 0; i < (int)Enhancement.Count; i++)
        {
            unlockedEnhancements.Add(false);
        }

        gems = 0;

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

    public bool HasColor(int colorIndex)
    {
        return unlockedColors.Contains(colorIndex);
    }

    public void UnlockColor(int colorIndex)
    {
        unlockedColors.Add(colorIndex);
    }

    public void ResetColors()
    {
        unlockedColors.Clear();
    }

    public bool HasEnhancement(Enhancement e)
    {
        return unlockedEnhancements[(int)e];
    }

    public void SetEnhancement(Enhancement e, bool unlocked)
    {
        unlockedEnhancements[(int)e] = unlocked;
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
