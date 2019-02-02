using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine.Events;

public class ShipSpawner : MonoBehaviour
{
    public bool destroyOnTrigger = true;
    public bool targetPlayer;

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

    public static Ship SpawnNPC(ShipSpawnInfo spawnInfo, Vector3 pos)
    {
        var ship = SpawnShip(spawnInfo, pos);
        var ai = ship.GetComponent<StateController>();

        if (ai == null)
        {
            Debug.LogWarning("Spawning NPC without AI Controller...");
        }
        else
        {
            ai.currentState = spawnInfo.state;
            ai.Target = spawnInfo.target ?? GameSettings.pc.ship.gameObject;
            ai.aiIsActive = true;
        }

        ship.Died += (s) => { LootSpawner.SpawnLoot(s.transform.position, spawnInfo); };

        return ship;
    }

    public static Ship SpawnShip(ShipSpawnInfo spawnInfo, Vector3 pos)
    {
        if (spawnInfo.ship == null)
        {
            Debug.LogWarning("Spawning default ship");
            spawnInfo.ship = GameSettings.Instance.defaultShip;
        }

        var ship = Instantiate(GameSettings.Instance.defaultShip, pos, Quaternion.identity);

        if (spawnInfo.loadout != null)
        {
            foreach (var proj in spawnInfo.loadout.projectiles)
            {
                // 1st pass: give each gun a projectile?
                // find a gun    
                //ship.hpSys.guns.ForEach(x => x.projectile = proj);
                print("this worked?");
            }
        }

        return ship;
    }

    public static Ship SpawnShip(Ship ship, Vector3 spawn)
    {
        var spawnInfo = new ShipSpawnInfo();
        spawnInfo.ship = GameSettings.Instance.defaultShip;
        return SpawnShip(spawnInfo, spawn);
    }

    public static List<Ship> SpawnShips(ShipSpawnInfo[] shipsToSpawn, Vector3 spawnPos, Bounds bounds)
    {
        List<Ship> spawnedShips = new List<Ship>();

        for (int i = 0; i < shipsToSpawn.Length; i++)
            spawnedShips.Add(SpawnNPC(shipsToSpawn[i], spawnPos + Utilities.RandomPointInBounds(bounds)));

        return spawnedShips;
    }
}

[System.Serializable]
public class ShipSpawnInfo
{
    public GameObject target;
    public Vector3 spawnOffset = Vector3.one;
    public State state;
    public Loadout loadout;
    public Ship ship;
    public DropList dl;
    public LootSpawnInfo[] lootInfo;
    // Relations to other ships??
}