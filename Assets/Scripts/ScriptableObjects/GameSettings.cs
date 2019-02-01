using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Settings/GameSettings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    [Header("Default Prefabs")]
    public Ship defaultShip;
    public HUDManager HUDPrefab;
    public FLTerminal terminalPrefab;
    public PlayerController pcPrefab;
    public Canvas nebulaCanvasPrefab;
    public Loot lootPrefab;

    public Loadout defaultLoadout;
    public Inventory playerInventory;

    public GameObject[] globalPrefabs;
    public GameObject[] localPrefabs;

    public static PlayerController pc;

    public string menuSceneName = "SCN_MainMenu";

    public Resolution[] resolutions;
    public List<string> resolutionOptions;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void GameLoading()
    {
        Instance.InitResolutions();

        pc = FindObjectOfType<PlayerController>();

        if (pc == null)
            pc = Instantiate(Instance.pcPrefab);

        DontDestroyOnLoad(pc);

        DontDestroyOnLoad(pc.GetComponent<NebulaCameraFog>().fadeQuad = Instantiate(Instance.nebulaCanvasPrefab).GetComponent<Image>());

        Instance.playerInventory = Utilities.CheckScriptableObject<Inventory>(Instance.playerInventory);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void GameStartup()
    {
        Instance.SpawnGlobalPrefabs();
        SceneManager.sceneLoaded += HandleNewScene;
        HandleNewScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private static void HandleNewScene(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loading new scene: " + scene.name);

        pc.GetComponent<NebulaCameraFog>().Init();

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
        new GameObject().AddComponent<FLTerminal>();
    }

    public void HandleGameplayScene()
    {
        // Move this into PlayerController?
        pc.cam.enabled = true;
        pc.flycam.enabled = false;
        pc.listener.enabled = true;
        //

        var playerShip = ShipSpawner.SpawnShip(defaultShip, Vector3.zero);
        pc.Possess(playerShip);

        var hud = FindObjectOfType<HUDManager>();

        if (hud == null) 
            hud = Instantiate(HUDPrefab);

        //DontDestroyOnLoad(hud.gameObject);

        hud.SetPlayerController(pc);
    }

    private void InitResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionOptions = new List<string>();

        //int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " : " + resolutions[i].refreshRate + "hz";
            resolutionOptions.Add(option);

            // if (resolutions[i].Equals(Screen.currentResolution))
            // {
            //     currentResolutionIndex = i;
            // }
        }
    }
}
