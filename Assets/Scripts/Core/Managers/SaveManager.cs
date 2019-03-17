using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;

public class SaveManager
{
    public static SaveInfo QuickSave()
    {
        return new SaveInfo();
    }

    public static List<SaveInfo> FindSaves()
    {
        var files = Directory.GetFiles(Application.persistentDataPath, "*.dat", SearchOption.TopDirectoryOnly);

        if (files.Length <= 0) return null;

        BinaryFormatter bf = new BinaryFormatter();
        var saves = new List<SaveInfo>();

        foreach (var filePath in files)
        {
            var fs = File.OpenRead(filePath);
            SaveInfo info = (SaveInfo) bf.Deserialize(fs);
            fs.Close();
            if (info == null) continue;
            saves.Add(info);
        }

        return saves;
    }

    public static bool DeleteSave(string filename)
    {
        var playerSavePath = Application.persistentDataPath + "/" + filename;
        if (File.Exists(playerSavePath))
        {
            File.Delete(playerSavePath);
            return (!File.Exists(playerSavePath));
        } 
        else return false;
    }
}

[Serializable]
public class SaveInfo
{
    public string name = "playerSave";
    public string saveTime = DateTime.Now.ToShortTimeString();
    public string sceneName = "SCN_Debug";

    //public Inventory inventory;
    //public Vector3 positionInScene;
    //public GameObject shipPrefab;

    public string Filepath {private set; get;}

    public SaveInfo(string name)
    {
        name = MakeValidFileName(name);
        this.name = name;
        this.Filepath = Application.persistentDataPath + "/" + name + ".dat";
        FileStream fs = File.Open(Filepath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, this);
        fs.Close();
    }

    public SaveInfo() : this("quicksave_" + DateTime.Now.ToLongTimeString()) { }

    private static string MakeValidFileName( string name )
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape( new string( System.IO.Path.GetInvalidFileNameChars() ) );
        string invalidRegStr = string.Format( @"([{0}]*\.+$)|([{0}]+)", invalidChars );

        return System.Text.RegularExpressions.Regex.Replace( name, invalidRegStr, "_" );
    }
}
