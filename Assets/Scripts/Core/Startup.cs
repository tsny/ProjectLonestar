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

        if (SceneManager.GetActiveScene().name != "SCN_MainMenu")
        {
            DebugSettings.Instance.SpawnPrefabs();
        }
    }

    private static void HandleNewScene(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "SCN_MainMenu") return;

        DebugSettings.Instance.SpawnPrefabs();
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
