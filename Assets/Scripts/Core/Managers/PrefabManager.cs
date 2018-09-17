using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "PrefabManager")]
public class PrefabManager : SingletonScriptableObject<PrefabManager>
{
    public GameObject HUDPrefab;
    public GameObject terminalPrefab;
    public GameObject shipPrefab;
    public GameObject flycamPrefab;
    public Loadout defaultLoadout;

    public string test;

    public void SpawnPrefabs()
    {
        Instantiate(terminalPrefab);
    }
}
