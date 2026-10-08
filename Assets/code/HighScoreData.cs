using UnityEngine;

public static class HighScoreData
{
    public static int Easy;
    public static int Normal;
    public static int Hard;
    public static int Hell;

    public static bool HellSeen;

    public static void Load()
{
    Easy = PlayerPrefs.GetInt("HighScore_Easy", 0);
    Normal = PlayerPrefs.GetInt("HighScore_Normal", 0);
    Hard = PlayerPrefs.GetInt("HighScore_Hard", 0);
    Hell = PlayerPrefs.GetInt("HighScore_Hell", 0);

    HellSeen = PlayerPrefs.GetInt("HellSeen", 0) == 1;
}

public static void Save()
{
    PlayerPrefs.SetInt("HighScore_Easy", Easy);
    PlayerPrefs.SetInt("HighScore_Normal", Normal);
    PlayerPrefs.SetInt("HighScore_Hard", Hard);
    PlayerPrefs.SetInt("HighScore_Hell", Hell);

    PlayerPrefs.SetInt("HellSeen", HellSeen ? 1 : 0);

    PlayerPrefs.Save();
}

    public static bool IsHellUnlocked()
    {
        return Easy >= 3000 &&
            Normal >= 3000 &&
            Hard >= 3000;
    }
}