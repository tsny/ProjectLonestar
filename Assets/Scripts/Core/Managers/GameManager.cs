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
    public MonoBehaviour[] requiredSceneObjects;

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
    public GameObject flycamPrefab;

    private GameObject currentHUD;

    private void Awake()
    {
        SingletonInit();
    }

    private void HandleNewScene(Scene arg0, Scene arg1)
    {
        if (spawnPlayer)
        {
            playerShip = ShipSpawner.instance.SpawnPlayerShip(shipPrefab, playerLoadout, spawnPosition);
            RemoveFlyCamFromScene();
        }

        else SpawnFlyCam(Vector3.zero);
    }

    // If unpossessing without possessing new ship: spawn a flyCam
    // Otherwise: Destroy the current flyCam
    private void HandlePossession(PossessionEventArgs args)
    {
        if (args.newShip == null) SpawnFlyCam(args.oldShip.transform.position);

        else RemoveFlyCamFromScene();
    }

    private void SpawnFlyCam(Vector3 pos)
    {
        var flyCam = Instantiate(flycamPrefab);
        flyCam.transform.position = pos + new Vector3(0, 10, 0);
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

    private void SingletonInit()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            // Only do these things once
            SceneManager.activeSceneChanged += HandleNewScene;
        }

        else if (instance != this) Destroy(gameObject);

        playerController = FindObjectOfType<PlayerController>();
        playerController.Possession += HandlePossession;
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

    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
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