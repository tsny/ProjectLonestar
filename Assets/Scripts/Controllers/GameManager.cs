using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController playerController;
    //public ShipSpawner shipSpawner;
    public GameObject shipPrefab;
    public Vector3 spawnPosition;
    public Loadout playerLoadout;
    public Ship playerShip;

    public bool debugMode;
    public int locationID;
    public bool spawnPlayer;
    private bool paused;

    #region properties
    public KeyCode ThrottleUp { get; set; }
    public KeyCode ThrottleDown { get; set; }
    public KeyCode Reverse { get; set; }

    public KeyCode StrafeRight { get; set; }
    public KeyCode StrafeLeft { get; set; }

    public KeyCode ToggleMouseFlight { get; set; }
    public KeyCode ToggleCruise { get; set; }
    public KeyCode Afterburner { get; set; }
    public KeyCode KillEngines { get; set; }

    public KeyCode ManualMouseFlight { get; set; }
    public KeyCode Fire { get; set; }

    public KeyCode Nanobots { get; set; }
    public KeyCode ShieldBots { get; set; }

    public KeyCode Hardpoint1 { get; set; }
    public KeyCode Hardpoint2 { get; set; }
    public KeyCode Hardpoint3 { get; set; }
    public KeyCode Hardpoint4 { get; set; }
    public KeyCode Hardpoint5 { get; set; }
    public KeyCode Hardpoint6 { get; set; }
    public KeyCode Hardpoint7 { get; set; }
    public KeyCode Hardpoint8 { get; set; }
    public KeyCode Hardpoint9 { get; set; }
    public KeyCode Hardpoint10 { get; set; }

    public KeyCode LootAll { get; set; }

    public KeyCode FireMineDropper { get; set; }
    public KeyCode FireCountermeasure { get; set; }
    public KeyCode FireMissiles { get; set; }

    public KeyCode NearestEnemy { get; set; }
    public KeyCode NextTarget { get; set; }
    public KeyCode ClearTarget { get; set; }
    public KeyCode PrevTarget { get; set; }

    public KeyCode PauseGame { get; set; }

    public KeyCode RequestDock { get; set; }
    public KeyCode GoToTarget { get; set; }
    public KeyCode JoinFormation { get; set; }
    public KeyCode FreeFlight { get; set; }

    public KeyCode StoryStar { get; set; }
    public KeyCode NavMap { get; set; }
    public KeyCode Inventory { get; set; }
    public KeyCode Reputation { get; set; }
    public KeyCode Information { get; set; }
    public KeyCode MinimizeHUD { get; set; }
    public KeyCode SwitchCamera { get; set; }
    #endregion

    private void Awake()
    {
        SingletonInit();

        SceneManager.activeSceneChanged += HandleNewScene;

        //shipSpawner = GetComponent<ShipSpawner>();
        playerController = FindObjectOfType<PlayerController>();

        AssignKeyCodes();
    }

    private void HandleNewScene(Scene arg0, Scene arg1)
    {
    }

    private void SingletonInit()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        else if (instance != this) Destroy(gameObject);
    }

    private void AssignKeyCodes()
    {
        ThrottleUp = GetKeycode("ThrottleUpKey", "W");
        ThrottleDown = GetKeycode("ThrottleDown", "S");
        StrafeRight = GetKeycode("StrafeRightKey", "D");
        StrafeLeft = GetKeycode("StrafeLeftKey", "A");
        Reverse = GetKeycode("Reverse", "X");
        Afterburner = GetKeycode("AfterburnerKey", "Tab");
        KillEngines = GetKeycode("KillEnginesKey", "Z");
        ToggleMouseFlight = GetKeycode("ToggleMouseFlightKey", "Space");

        ManualMouseFlight = GetKeycode("ManualMouseFlightKey", "Mouse0");
        Fire = GetKeycode("FireKey", "Mouse1");

        Nanobots = GetKeycode("NanobotsKey", "G");
        ShieldBots = GetKeycode("ShieldbotsKey", "F");

        Hardpoint1 = GetKeycode("Hardpoint1Key", "Alpha1");
        Hardpoint2 = GetKeycode("Hardpoint2Key", "Alpha2");
        Hardpoint3 = GetKeycode("Hardpoint3Key", "Alpha3");
        Hardpoint4 = GetKeycode("Hardpoint4Key", "Alpha4");
        Hardpoint5 = GetKeycode("Hardpoint5Key", "Alpha5");
        Hardpoint6 = GetKeycode("Hardpoint6Key", "Alpha6");
        Hardpoint7 = GetKeycode("Hardpoint7Key", "Alpha7");
        Hardpoint8 = GetKeycode("Hardpoint8Key", "Alpha8");
        Hardpoint9 = GetKeycode("Hardpoint9Key", "Alpha9");
        Hardpoint10 = GetKeycode("Hardpoint10Key", "Alpha0");

        LootAll = GetKeycode("LootAllKey", "B");

        FireMineDropper = GetKeycode("MineDropperKey", "E");
        FireMissiles = GetKeycode("FireMissilesKey", "Q");
        FireCountermeasure = GetKeycode("FireCountermeasureKey", "C");

        PauseGame = GetKeycode("PauseKey", "F1");
        RequestDock = GetKeycode("RequestDockKey", "F3");
        GoToTarget = GetKeycode("GoToKey", "F2");
        JoinFormation = GetKeycode("JoinFormKey", "F4");
    }

    private void Start()
    {
        //LoadPlayerInfo();

        if (spawnPlayer)
        {
            playerShip = ShipSpawner.instance.SpawnPlayerShip(shipPrefab, playerLoadout, spawnPosition);
        }
    }

    private void OnApplicationQuit()
    {
        SavePlayerInfo();
    }

    public KeyCode GetKeycode(string keyName, string defaultName)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyName, defaultName));
    }

    public void TogglePause()
    {
        paused = !paused;

        if(paused) Time.timeScale = 0.0f;

        else Time.timeScale = 1.0f;
    }

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
        if (File.Exists(Application.persistentDataPath + "/playerSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerSave.dat", FileMode.Open);

            PlayerInfo playerInfo = (PlayerInfo) bf.Deserialize(file);
            file.Close();

            print("Loaded Save 1, ID: " + playerInfo.saveTime);
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/playerSave.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerSave.dat");
        }
    }

    public void LoadNewSystem(string systemToLoad, int ID = 0)
    {
        locationID = ID;
        SceneManager.LoadScene(systemToLoad);
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