using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine.Events;

public class ShipSpawner : MonoBehaviour
{
    public bool destroyOnTrigger = true;

    public float spawnDelay = 1;
    public float destroyDelay = 3;
    public float distanceApart = 5;

    public Transform target;
    public UnityEvent onTriggered;

    // Default State
    // Default Actions?
    // Need to make this more complicated in order to individually make some ships hostile/friendly to one another
    public ShipSpawnInfo[] shipsToSpawn;

    // TODO: Experiment with these
    // private int wave = 1;
    // private int shipsPerWave = 1;
    // private float waveDuration = 10;

    private BoxCollider coll;

    private void Awake()
    {
        coll = Utilities.CheckComponent<BoxCollider>(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { TriggerSelf(); }
    }

    public void TriggerSelf()
    {
        SpawnShips(shipsToSpawn, transform.position, coll.bounds);

        onTriggered.Invoke();

        // Spawn all
        // Destroy self or implement wave feature if we want more ships to spawn later
        if (destroyOnTrigger)
        {
            coll.enabled = false;
            Destroy(gameObject, destroyDelay);
        }
    }

    public static Ship SpawnShip(ShipSpawnInfo spawnInfo, Vector3 spawnPosition, bool forPlayer = false)
    {
        if (spawnInfo.shipBase == null)
        {
            Debug.LogWarning("Spawning ship w/o selecting base, spawning default ship...");
            spawnInfo.shipBase = GameSettings.Instance.defaultShipBase;
        }

        var ship = Instantiate(spawnInfo.ship, spawnPosition, Quaternion.identity);
        ship.ShipBase = spawnInfo.shipBase;

        var ai = ship.GetComponent<StateController>();

        if (ai == null)
        {
            ship.Died += (s) => { LootSpawner.SpawnLoot(s.transform.position, spawnInfo); };
            Debug.LogWarning("Spawning ship without AI Controller...");
        }
        else if (forPlayer)
        {
            ai.currentState = null;
        }
        else
        {
            ship.Died += (s) => { LootSpawner.SpawnLoot(s.transform.position, spawnInfo); };
            ai.currentState = spawnInfo.state;
            ai.Target = spawnInfo.target;
            ai.aiIsActive = true;
        }

        return ship;
    }

    public static Ship SpawnShip(Ship ship, Vector3 spawn, bool forPlayer = false)
    {
        var spawnInfo = new ShipSpawnInfo();
        spawnInfo.ship = ship;
        return SpawnShip(spawnInfo, spawn, forPlayer);
    }

    public static List<Ship> SpawnShips(ShipSpawnInfo[] shipsToSpawn, Vector3 spawnPos, Bounds bounds)
    {
        List<Ship> spawnedShips = new List<Ship>();

        for (int i = 0; i < shipsToSpawn.Length; i++)
            spawnedShips.Add(SpawnShip(shipsToSpawn[i], spawnPos + Utilities.RandomPointInBounds(bounds)));

        return spawnedShips;
    }
}
[System.Serializable]
public class ShipSpawnInfo
{
    public GameObject target;
    public Vector3 spawnOffset = Vector3.one;
    public State state;
    public Ship ship;
    public Loadout loadout;
    public GameObject shipBase;

    public DropList dl;
    public LootSpawnInfo[] lootInfo;
    // Relations to other ships??
}