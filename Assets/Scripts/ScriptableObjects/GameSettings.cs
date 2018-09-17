using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Settings/GameSettings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    public GameObject shipPrefab;
    public GameObject flycamPrefab;
    public GameObject HUDPrefab;

    public Loadout defaultLoadout;

    public GameObject[] prefabsToSpawnAtLoad;

    public void SpawnLoadPrefabs()
    {
        foreach (var prefab in prefabsToSpawnAtLoad)
        {
            Instantiate(prefab);
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
