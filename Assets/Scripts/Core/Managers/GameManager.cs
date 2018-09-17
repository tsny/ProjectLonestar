using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Space(5)]
    public static GameManager instance;

    public PlayerController playerController;
    public PrefabManager prefabManager;

    public static bool Paused
    {
        get
        {
            return Time.timeScale == 0;
        }

        set
        {
            if (GamePaused != null) GamePaused(value);
            Time.timeScale = value ? 0 : 1;
        }
    }

    public delegate void EventHandler(bool paused);
    public delegate void ManagerSpawnedEventHandler(GameManager sender, PlayerController playerController);
    public static event EventHandler GamePaused;
    public event ManagerSpawnedEventHandler PlayerControllerSpawned;

    private void Awake()
    {
        name = "GAMEMANAGER";
        SingletonInit();
    }

    private void HandleNewScene(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "SCN_MainMenu") return;

        SpawnPlayerController();
    }

    public void SpawnPlayerController()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            playerController.Possession -= HandlePossession;
        }

        playerController = new GameObject().AddComponent<PlayerController>();
        playerController.Possession += HandlePossession;

        if (PlayerControllerSpawned != null) PlayerControllerSpawned(this, playerController);
    }

    private void HandlePossession(PossessionEventArgs args)
    {

    }

    private void SingletonInit()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            // Only do these things once, when the game loads

            DontDestroyOnLoad(Instantiate(prefabManager.terminalPrefab));
            SceneManager.sceneLoaded += HandleNewScene;
        }

        else if (instance != this) Destroy(gameObject);
    }

    public static void TogglePause()
    {
        Paused = !Paused;
    }

    public static void SetPause(bool pause)
    {
        Paused = pause;
    }
}

