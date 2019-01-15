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
        if (coll == null)
        {
            coll = GetComponent<Collider>();
        }
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

    public static Ship SpawnShip(Ship shipGO, Vector3 spawnPosition, Loadout loadout = null)
    {
        var ship = Instantiate(shipGO, spawnPosition, Quaternion.identity);

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
        foreach (Hardpoint hp in ship.hardpointSystem.hardpoints)
        {
            // Try mount
        }

        return ship;
    }

    public static List<Ship> SpawnShips(ShipSpawn[] shipsToSpawn, Vector3 spawnPos, Bounds bounds)
    {
        List<Ship> spawnedShips = new List<Ship>();

        foreach (var spawn in shipsToSpawn)
        {
            for (int i = 0; i < spawn.numToSpawn; i++)
            {
                var ship = SpawnShip(spawn.ship, spawnPos + RandomPointInBounds(bounds), spawn.loadout);
                ship.Init();
                spawnedShips.Add(ship);

                var ai = ship.GetComponent<StateController>();

                if (ai == null)
                {
                    Debug.LogWarning("Spawning ship without AI Controller...");
                }
                else
                {
                    ai.currentState = spawn.state;
                    ai.targetTrans = spawn.target;
                    ai.aiIsActive = true;
                }
            }
        }

        return spawnedShips;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);
        var z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    } 
}

[System.Serializable]
public class ShipSpawn
{
    public bool isHostile;
    public int numToSpawn = 1;
    public Transform target;

    public Vector3 spawnOffset = Vector3.one;
    public State state;
    public Ship ship;
    public Loadout loadout;
    // Relations to other ships??
}