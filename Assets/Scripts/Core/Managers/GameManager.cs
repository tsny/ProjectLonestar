using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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

    public bool debugMode;
    public bool spawnPlayer;
    private bool paused;

    [Space(5)]
    public GameObject shipHUD;
    public GameObject flyCamHUD;

    private GameObject currentHUD;

    //public int locationID;

    private void Awake()
    {
        SingletonInit();

        SceneManager.activeSceneChanged += HandleNewScene;

        playerController = FindObjectOfType<PlayerController>();
        playerController.Possession += HandlePossession;
    }

    private void Start()
    {
        //LoadPlayerInfo();
        if (spawnPlayer)
        {
            playerShip = ShipSpawner.instance.SpawnPlayerShip(shipPrefab, playerLoadout, spawnPosition);
            RemoveFlyCamFromScene();
        }

        else SpawnFlyCam(Vector3.zero);

        //CalculateMapSize();
    }

    private void HandlePossession(PossessionEventArgs args)
    {
        // If unpossessing without possessing new ship: spawn a flyCam
        // Otherwise: Destroy the current flyCam

        if (args.newShip == null) SpawnFlyCam(args.oldShip.transform.position);

        else RemoveFlyCamFromScene();
    }

    private void SpawnFlyCam(Vector3 pos)
    {
        var flyCam = new GameObject().AddComponent<Flycam>();
        flyCam.transform.position = pos + new Vector3(0, 10, 0);
        flyCam.name = "FLYCAM";

        SwapHUD(flyCamHUD);
    }

    private void RemoveFlyCamFromScene()
    {
        var flyCam = FindObjectOfType<Flycam>();
        if (flyCam != null) Destroy(flyCam.gameObject);

        SwapHUD(shipHUD);
    }

    private void SwapHUD(GameObject newHUD)
    {
        if (newHUD == null) return;

        if (currentHUD != null && currentHUD.activeSelf) currentHUD.SetActive(false);
        currentHUD = newHUD;
        currentHUD.SetActive(true);
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

    private void OnApplicationQuit()
    {
        SavePlayerInfo();
    }

    // TODO: Also disable player input
    public void TogglePause()
    {
        paused = !paused;

        playerController.inputAllowed = !paused;

        Time.timeScale = paused ? 0.0f : 1.0f;
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

    public void LoadNewSystem(string systemToLoad, int ID = 0)
    {
        //locationID = ID;
        SceneManager.LoadScene(systemToLoad);
    }

    public void CalculateMapSize()
    {
        var objects = FindObjectsOfType<WorldObject>();

        var closestObject = objects.OrderBy(t => Vector3.Distance(Vector3.zero, t.transform.position)).FirstOrDefault();
        var farthestObject = objects.OrderBy(t => Vector3.Distance(Vector3.zero, t.transform.position)).LastOrDefault();

        objects.ToList().ForEach(i => print(Vector3.Distance(Vector3.zero, i.transform.position) + i.name));
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