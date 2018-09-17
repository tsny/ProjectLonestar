using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;

public class Startup : ScriptableObject
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("After scene is loaded and game is running");
        SceneManager.sceneLoaded += HandleNewScene;
    }

    private static void HandleNewScene(Scene arg0, LoadSceneMode arg1)
    {
        SpawnPlayerController();
    }

    public static PlayerController SpawnPlayerController()
    {
        var playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            playerController = new GameObject().AddComponent<PlayerController>();
        }

        return playerController;
    }
}
