using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "PrefabManager")]
public class DebugSettings : SingletonScriptableObject<DebugSettings>
{
    public GameObject HUDPrefab;
    public GameObject terminalPrefab;
    public GameObject shipPrefab;
    public GameObject flycamPrefab;
    public Loadout defaultLoadout;

    public void FirstMethodRun()
    {
        Instantiate(terminalPrefab);
    }

    public void SpawnPrefabs()
    {
        var playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            playerController = new GameObject().AddComponent<PlayerController>();
        }

        var playerShip = playerController.SpawnPlayer(shipPrefab, defaultLoadout);

        playerController.Possess(playerShip);

        var hud = playerController.SpawnHUD();

        hud.gameObject.SetActive(true);
    }
}
