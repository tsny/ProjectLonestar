using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Settings/GameSettings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    public Ship shipPrefab;
    public HUDManager HUDPrefab;
    public FLTerminal terminalPrefab;
    public PlayerController pcPrefab;

    public Loadout defaultLoadout;
    public Inventory playerInventory;

    public GameObject[] globalPrefabs;
    public GameObject[] localPrefabs;

    public static PlayerController pc;

    public string menuSceneName = "SCN_MainMenu";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void GameStartup()
    {
        pc = FindObjectOfType<PlayerController>();
        if (pc == null)
        {
            pc = Instantiate(Instance.pcPrefab);
        }
        DontDestroyOnLoad(pc);

        Instance.SpawnGlobalPrefabs();
        SceneManager.sceneLoaded += HandleNewScene;
        HandleNewScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private static void HandleNewScene(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loading new scene: " + scene.name);

        if (scene.name != Instance.menuSceneName)
        {
            Instance.HandleGameplayScene();
        }
        else
        {
            pc.cam.enabled = false;
            pc.flycam.enabled = false;
            pc.listener.enabled = false;
        }
    }

    public void SpawnGlobalPrefabs()
    {
        var term = new GameObject().AddComponent<FLTerminal>();
    }

    public void HandleGameplayScene()
    {
        // Move this into PlayerController?
        pc.cam.enabled = true;
        pc.flycam.enabled = false;
        pc.listener.enabled = true;
        //

        var playerShip = ShipSpawner.SpawnShip(shipPrefab, Vector3.zero, defaultLoadout);
        pc.Possess(playerShip);

        var hud = FindObjectOfType<HUDManager>();

        if (hud == null) 
            hud = Instantiate(HUDPrefab);

        //DontDestroyOnLoad(hud.gameObject);

        hud.SetPlayerController(pc);
    }
}
