using UnityEngine;
using System.Collections;

public class ShipSpawner : ScriptableObject
{
    public static Loadout CreateDummyLoadout()
    {
        Loadout loadout = CreateInstance<Loadout>();
        loadout.equipment.Add(CreateInstance<Weapon>());
        loadout.equipment.Add(CreateInstance<Weapon>());

        return loadout;
    }

    public static Ship SpawnShip(GameObject ship, ShipStats stats, Loadout loadout, Vector3 spawnPosition)
    {
        var shipRef = Instantiate(ship, spawnPosition, Quaternion.identity).GetComponent<Ship>();
        shipRef.hardpointSystem.MountLoadout(loadout);
        return shipRef;
    }

    public static Ship SpawnShip(GameObject ship, Loadout loadout, Vector3 spawnPosition)
    {
        var shipRef = Instantiate(ship, spawnPosition, Quaternion.identity).GetComponent<Ship>();
        shipRef.hardpointSystem.MountLoadout(loadout);
        return shipRef;
    }

    public static Ship SpawnDefaultShip()
    {
        var newShip = Instantiate(GameSettings.Instance.shipPrefab);
        return newShip.GetComponent<Ship>();
    }
}

public class ShipSpawnInfo
{
    public int numTimesToSpawn = 1;
    public GameObject shipPrefab;
    public Loadout loadout;
}
