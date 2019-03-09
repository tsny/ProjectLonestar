using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class ShipSpawner : MonoBehaviour
{
    public UnityEvent onTriggered;
    public ShipSpawnInfo[] shipsToSpawn;
    public event TriggeredEventHandler Triggered;
    public delegate void TriggeredEventHandler();

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
        if (Triggered != null) Triggered();
    }

    public void InvokeSelf()
    {
        onTriggered.Invoke();
    }

    public static Ship SpawnNPC(ShipSpawnInfo spawnInfo, Vector3 pos)
    {
        var ship = SpawnShip(spawnInfo, pos);

        ship.ai.currentState = spawnInfo.state;
        ship.ai.Target = spawnInfo.target ?? PlayerController.Instance.ship.gameObject;
        ship.ai.aiIsActive = true;

        var dropper = Utilities.CheckComponent<LootDropper>(ship.gameObject);
        ship.Died += (s) => { dropper.SpawnLootDrops(); };

        return ship;
    }

    public static Ship SpawnShip(ShipSpawnInfo spawnInfo, Vector3 pos)
    {
        if (spawnInfo.ship == null)
            spawnInfo.ship = GameSettings.Instance.defaultShip;

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

        foreach (var ship in spawnedShips)
        {
            if (ship.ai.allies == null)
                ship.ai.allies = new List<Ship>();

            foreach (var innerShip in spawnedShips)
            {
                if (ship != innerShip)
                    ship.ai.allies.Add(innerShip);
            }
        }

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
    public Drop[] drops;
}