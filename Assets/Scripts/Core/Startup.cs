using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;

public class Startup : ScriptableObject
{
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    //static void OnRuntimeMethodLoad()
    //{
    //    SceneManager.sceneLoaded += HandleNewScene;

    //    GameSettings.Instance.SpawnLoadPrefabs();

    //    if (SceneManager.GetActiveScene().name != "SCN_MainMenu")
    //    {
    //        GameSettings.Instance.SpawnNewScenePrefabs();
    //    }
    //}

    //private static void HandleNewScene(Scene arg0, LoadSceneMode arg1)
    //{
    //    if (arg0.name == "SCN_MainMenu")
    //    {
    //        return;
    //    }

    //    GameSettings.Instance.SpawnNewScenePrefabs();
    //}
}
