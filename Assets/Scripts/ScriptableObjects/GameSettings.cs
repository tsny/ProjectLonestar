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
    public Inventory playerInventory;

    public GameObject[] globalPrefabs;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnRuntimeMethod()
    {
        Instance.SpawnGlobalPrefabs();

        //#if (!UNITY_EDITOR)

        SceneManager.activeSceneChanged += HandleNewScene;

        if (SceneManager.GetActiveScene().name != "SCN_MainMenu")
        {
            Instance.SpawnLocalPrefabs();
        }

        //#endif
    }

    private static void HandleNewScene(Scene arg0, Scene arg1)
    {
        Instance.SpawnLocalPrefabs();
    }

    public void SpawnGlobalPrefabs()
    {
        foreach (var prefab in globalPrefabs)
        {
            DontDestroyOnLoad(Instantiate(prefab));
        }
    }

    public void SpawnLocalPrefabs()
    {
        var playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            playerController = new GameObject().AddComponent<PlayerController>();
        }

        var playerShip = ShipSpawner.SpawnShip(shipPrefab, Vector3.zero, defaultLoadout);

        playerController.Possess(playerShip);

        //playerController.SpawnHUD();
    }
}
