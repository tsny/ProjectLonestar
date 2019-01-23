using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameStateUtils : UnityEngine.Object
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

    public static void Quit()
    {
        Application.Quit();
    }

    public static Vector2 GetMousePositionOnScreen()
    {
        int width = Screen.width;
        int height = Screen.height;

        Vector2 center = new Vector2(width / 2, height / 2);
        Vector3 mousePosition = Input.mousePosition;

        // Shifts the origin of the screen to be in the middle instead of the bottom left corner

        var mouseX = mousePosition.x - center.x;
        var mouseY = mousePosition.y - center.y;

        mouseX = Mathf.Clamp(mouseX / center.x, -1, 1);
        mouseY = Mathf.Clamp(mouseY / center.y, -1, 1);

        return new Vector2(mouseX, mouseY);
    }

    // NOT DONE
    public static GameObject NearestShip(GameObject sender)
    {
        GameObject nearest = null;

        //List<Ship> ships = FindObjectsOfType<Ship>().ToList();

        return nearest;
    }
}

