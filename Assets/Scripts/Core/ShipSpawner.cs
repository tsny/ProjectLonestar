using UnityEngine;
using System.Collections;

public class ShipSpawner : ScriptableObject
{
    public static Ship SpawnShip(GameObject shipGO, Vector3 spawnPosition, Loadout loadout = null)
    {
        var ship = Instantiate(shipGO, spawnPosition, Quaternion.identity).GetComponent<Ship>();

        //if (loadout == null)
        //{
        //    loadout = CreateInstance<Loadout>();
        //}

        //shipRef.hardpointSystem.MountLoadout(loadout);

        return ship;
    }
}

public class ShipSpawnInfo
{
    public int numTimesToSpawn = 1;
    public GameObject shipPrefab;
    public Loadout loadout;
}
