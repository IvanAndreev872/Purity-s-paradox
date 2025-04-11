using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ConfigManager
{
    public static void SaveConfig(ConfigData configData)
    {
        string json = JsonUtility.ToJson(configData);
        string filePath = Application.streamingAssetsPath + "/config.json";
        File.WriteAllText(filePath, json);
    }
    public static ConfigData LoadConfig()
    {
        string filePath = Application.streamingAssetsPath + "/config.json";
        string json = File.ReadAllText(filePath);
        ConfigData configData = JsonUtility.FromJson<ConfigData>(json);
        return configData;
    }
}
