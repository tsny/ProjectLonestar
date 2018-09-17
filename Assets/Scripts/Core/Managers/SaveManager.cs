using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveManager
{
    public static void SavePlayerInfo()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerSave.dat", FileMode.Create);
        SaveInfo playerInfo = new SaveInfo();

        bf.Serialize(file, playerInfo);
        file.Close();
    }

    public static SaveInfo LoadPlayerInfo()
    {
        var path = Application.persistentDataPath + "/playerSave.dat";

        if (!File.Exists(path))
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerSave.dat", FileMode.Open);

        SaveInfo playerInfo = (SaveInfo) bf.Deserialize(file);
        file.Close();

        return playerInfo;
    }

    public static void DeleteSave()
    {
        var playerSavePath = Application.persistentDataPath + "/playerSave.dat";
        if (File.Exists(playerSavePath)) File.Delete(playerSavePath);
    }
}

[Serializable]
public class SaveInfo : ScriptableObject
{
    public string saveTime;
    public string saveName = "SaveName";
    public GameObject shipPrefab;

    public SaveInfo()
    {
        saveTime = DateTime.Now.ToShortTimeString();
    }
}
