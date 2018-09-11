using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using CommandTerminal;

public class GameManager : MonoBehaviour
{
    [Space(5)]
    public static GameManager instance;
    public PlayerController playerController;

    [Header("Ship Details")]
    public GameObject shipPrefab;
    public Vector3 spawnPosition;
    public Loadout playerLoadout;
    public Ship playerShip;

    [Space(5)]

    public bool spawnPlayerAsShip;

    [Space(5)]
    public GameObject shipHUDPrefab;
    public GameObject flycamPrefab;

    private GameObject flyCamHUD;
    private GameObject shipHUD;

    private GameObject currentHUD;
    public bool Paused
    {
        get
        {
            return Time.timeScale == 0;
        }

        set
        {
            playerController.inputAllowed = !value;
            Time.timeScale = value ? 0 : 1;
        }
    }

    private void Awake()
    {
        SingletonInit();
    }

    private void HandleNewScene(Scene arg0, LoadSceneMode arg1)
    {
        CreatePlayerController();

        if (spawnPlayerAsShip) SpawnPlayer();
        else SpawnFlyCam(Vector3.zero);

        new GameObject().AddComponent<FLTerminal>();
    }

    public void SpawnPlayer()
    {
        playerShip = ShipSpawner.instance.SpawnPlayerShip(shipPrefab, playerLoadout, spawnPosition);
        playerController.Possess(playerShip);
        RemoveFlycamFromScene();
        CreateHUD();
    }

    // If unpossessing without possessing new ship: spawn a flyCam
    // Otherwise: Destroy the current flyCam
    private void HandlePossession(PossessionEventArgs args)
    {
        if (args.newShip == null) SpawnFlyCam(args.oldShip.transform.position);
    }

    // Can only be one fly cam in the scene
    public void SpawnFlyCam(Vector3 pos)
    {
        RemoveFlycamFromScene();
        var flyCam = Instantiate(flycamPrefab);
        flyCam.transform.position = pos + new Vector3(0, 10, 0);
    }

    public void RemoveFlycamFromScene()
    {
        var flyCam = FindObjectOfType<Flycam>();
        if (flyCam != null) Destroy(flyCam.gameObject);
    }

    private void SingletonInit()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            // Only do these things once
            SceneManager.sceneLoaded += HandleNewScene;
        }

        else if (instance != this) Destroy(gameObject);
    }

    private PlayerController CreatePlayerController()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            playerController = new GameObject().AddComponent<PlayerController>();
        }

        playerController.Possession += HandlePossession;

        return playerController;
    }

    private HUDManager CreateHUD()
    {
        var hudManager = FindObjectOfType<HUDManager>();

        if (hudManager == null)
        {
            Instantiate(shipHUDPrefab);
        }

        return hudManager;
    }

    private void OnApplicationQuit()
    {
        SavePlayerInfo();
    }

    // TODO: Also disable player input
    public void TogglePause()
    {
        Paused = !Paused;
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