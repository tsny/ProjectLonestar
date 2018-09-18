using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Settings/GameSettings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    public GameObject shipPrefab;
    public GameObject flycamPrefab;
    public GameObject HUDPrefab;

    public Loadout defaultLoadout;

    public GameObject[] prefabsToSpawnAtLoad;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnRuntimeMethod()
    {
        Instance.SpawnLoadPrefabs();
        SceneManager.activeSceneChanged += HandleNewScene;
    }

    private static void HandleNewScene(Scene arg0, Scene arg1)
    {
        Instance.SpawnNewScenePrefabs();
    }

    public void SpawnLoadPrefabs()
    {
        foreach (var prefab in prefabsToSpawnAtLoad)
        {
            DontDestroyOnLoad(Instantiate(prefab));
        }
    }

    public void SpawnNewScenePrefabs()
    {
        var playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            playerController = new GameObject().AddComponent<PlayerController>();
        }

        var playerShip = playerController.SpawnPlayer(shipPrefab, defaultLoadout);

        playerController.Possess(playerShip);

        var hud = playerController.SpawnHUD();
    }
}
