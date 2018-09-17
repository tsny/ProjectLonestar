using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    public void SavePlayerInfo()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerSave.dat", FileMode.Create);
        PlayerInfo playerInfo = new PlayerInfo();

        bf.Serialize(file, playerInfo);
        file.Close();
    }

    public void LoadPlayerInfo()
    {
        var path = Application.persistentDataPath + "/playerSave.dat";

        if (!File.Exists(path)) return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerSave.dat", FileMode.Open);

        PlayerInfo playerInfo = (PlayerInfo) bf.Deserialize(file);
        file.Close();

        print("Loaded Save 1, ID: " + playerInfo.saveTime);
    }

    public void DeleteSave()
    {
        var playerSavePath = Application.persistentDataPath + "/playerSave.dat";
        if (File.Exists(playerSavePath)) File.Delete(playerSavePath);
    }
}

[Serializable]
public class PlayerInfo
{
    public string saveTime;

    public PlayerInfo()
    {
        saveTime = DateTime.Now.ToShortTimeString();
    }
}
