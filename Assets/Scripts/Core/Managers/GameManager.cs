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

