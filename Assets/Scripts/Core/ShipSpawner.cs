using UnityEngine;
using System.Collections;

public class ShipSpawner : MonoBehaviour
{
    public static ShipSpawner instance;

    public static Loadout CreateDummyLoadout()
    {
        Loadout loadout = ScriptableObject.CreateInstance<Loadout>();
        loadout.equipment.Add(ScriptableObject.CreateInstance<Weapon>());
        loadout.equipment.Add(ScriptableObject.CreateInstance<Weapon>());

        return loadout;
    }

    public static Ship SpawnShip(GameObject ship, ShipStats stats, Loadout loadout, Vector3 spawnPosition)
    {
        var shipRef = Instantiate(ship, spawnPosition, Quaternion.identity).GetComponent<Ship>();
        shipRef.inventory.Initialize(loadout);
        return shipRef;
    }

    public static Ship SpawnShip(GameObject ship, Loadout loadout, Vector3 spawnPosition)
    {
        var shipRef = Instantiate(ship, spawnPosition, Quaternion.identity).GetComponent<Ship>();
        shipRef.inventory.Initialize(loadout);
        return shipRef;
    }
}

public class ShipSpawnInfo
{
    public int numTimesToSpawn = 1;
    public GameObject shipPrefab;
    public Loadout loadout;
}
