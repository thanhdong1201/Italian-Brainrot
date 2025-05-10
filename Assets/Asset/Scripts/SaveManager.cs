using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaveManager
{
    // ----- INT -----
    public static void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    // ----- FLOAT -----
    public static void SaveFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static float LoadFloat(string key, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    // ----- STRING -----
    public static void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    public static string LoadString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    // ----- BOOL -----
    public static void SaveBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
    public static bool LoadBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }

    // ----- LIST<int> -----
    public static void SaveIntList(string key, List<int> list)
    {
        string csv = string.Join(",", list);
        PlayerPrefs.SetString(key, csv);
    }
    public static List<int> LoadIntList(string key)
    {
        string data = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(data)) return new List<int>();
        return data.Split(',').Select(int.Parse).ToList();
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All save data has been reset.");
    }
}
