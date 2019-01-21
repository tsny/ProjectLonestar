using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine.Events;

public class ShipSpawner : MonoBehaviour
{
    public bool destroyOnTrigger = true;

    public int numToSpawn = 1;
    public float spawnRadius = 50;
    public float spawnDelay = 1;
    public float destroyDelay = 3;
    public float distanceApart = 5;

    public Transform target;

    // Default State
    // Default Actions?
    // Need to make this more complicated in order to individually make some ships hostile/friendly to one another
    public ShipSpawn[] shipsToSpawn;

    // Experiment
    private int wave = 1;
    private int shipsPerWave = 1;
    private float waveDuration = 10;

    private Collider coll;

    private void Awake()
    {
        coll = Utilities.CheckComponent<Collider>(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(coll);
            TriggerSelf();
        }
    }

    public void TriggerSelf()
    {
        SpawnShips(shipsToSpawn, transform.position, coll.bounds);

        // Spawn all
        // Destroy self or implement wave feature if we want more ships to spawn later
        if (destroyOnTrigger)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    public static Ship SpawnShip(Ship shipToSpawn, ShipSpawn spawnInfo, Vector3 spawnPosition)
    {
        var ship = Instantiate(shipToSpawn, spawnPosition, Quaternion.identity);

        //ship.Died += (s) => { LootSpawner.SpawnLoot(s.transform.position, spawnInfo.lootInfo); };
        ship.Died += (s) => { LootSpawner.SpawnLoot(s.transform.position, spawnInfo.dl); };

        var ai = ship.GetComponent<StateController>();

        if (ai == null)
        {
            Debug.LogWarning("Spawning ship without AI Controller...");
        }
        else
        {
            ai.currentState = spawnInfo.state;
            ai.targetTrans = spawnInfo.target;
            ai.aiIsActive = true;
        }

        return ship;
    }

    public static Ship SpawnShip(Ship shipToSpawn, Vector3 spawnPosition, Loadout loadout = null)
    {
        var ship = Instantiate(shipToSpawn, spawnPosition, Quaternion.identity);

        if (ship == null)
        {
            Debug.LogError("Tried spawning ship without Ship component attached...");
            return null;
        }

        if (loadout == null)
        {
           loadout = ScriptableObject.CreateInstance<Loadout>();
        }

        //ship.hardpointSystem.MountLoadout(loadout);
        foreach (Hardpoint hp in ship.hpSys.hardpoints)
        {
            // Try mount
        }


        return ship;
    }

    public static List<Ship> SpawnShips(ShipSpawn[] shipsToSpawn, Vector3 spawnPos, Bounds bounds)
    {
        List<Ship> spawnedShips = new List<Ship>();

        foreach (var spawnInfo in shipsToSpawn)
        {
            for (int i = 0; i < spawnInfo.numToSpawn; i++)
                spawnedShips.Add(SpawnShip(spawnInfo.ship, spawnInfo, spawnPos + Utilities.RandomPointInBounds(bounds)));
        }

        return spawnedShips;
    }
}
[System.Serializable]
public class ShipSpawn
{
    public bool isHostile;
    public int numToSpawn = 1;
    public Transform target;
    public LootSpawnInfo[] lootInfo;
    public Vector3 spawnOffset = Vector3.one;
    public State state;
    public Ship ship;
    public Loadout loadout;
    public DropList dl;
    // Relations to other ships??
}