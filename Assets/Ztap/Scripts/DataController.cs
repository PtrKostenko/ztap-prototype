using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataController
{

    static public void SaveData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    static public void SaveData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    static public void SaveData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }


    static public string LoadString(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    static public float LoadFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
    static public int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    static public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    static public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    static public void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
