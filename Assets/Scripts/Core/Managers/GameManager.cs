using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    public static event EventHandler GamePaused;
    public delegate void EventHandler(bool paused);

    public static void TogglePause()
    {
        Paused = !Paused;
    }

    public static void SetPause(bool pause)
    {
        Paused = pause;
    }
}

