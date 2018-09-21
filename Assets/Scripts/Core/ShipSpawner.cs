using UnityEngine;
using System.Collections;

public class ShipSpawner : ScriptableObject
{
    public static Ship SpawnShip(GameObject ship, Vector3 spawnPosition, Loadout loadout = null)
    {
        var shipRef = Instantiate(ship, spawnPosition, Quaternion.identity).GetComponent<Ship>();

        //if (loadout == null)
        //{
        //    loadout = CreateInstance<Loadout>();
        //}

        //shipRef.hardpointSystem.MountLoadout(loadout);
        return shipRef;
    }
}

public class ShipSpawnInfo
{
    public int numTimesToSpawn = 1;
    public GameObject shipPrefab;
    public Loadout loadout;
}
