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
    public List<int> unlockedColors;
    public bool canSkipIntro;
    public bool canAgreeToLicensesImmediately;
    public bool canSkipLicenseQuiz;

    public Profile()
    {
        unlockedColors = new List<int>();
        canSkipIntro = false;
        canAgreeToLicensesImmediately = false;
        canSkipLicenseQuiz = false;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("Unlocked colors: ");
        foreach (int color in unlockedColors) builder.Append(color);
        builder.AppendLine();
        builder.Append("canSkipIntro: ");
        builder.Append(canSkipIntro);
        builder.AppendLine();
        builder.Append("canAgreeToLicensesImmediately: ");
        builder.Append(canAgreeToLicensesImmediately);
        builder.AppendLine();
        builder.Append("canSkipLicenseQuiz: ");
        builder.Append(canSkipLicenseQuiz);
        builder.AppendLine();

        return builder.ToString();
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

        // Apply overrides here if needed
        // inMemoryProfile.canSkipIntro = true;
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
