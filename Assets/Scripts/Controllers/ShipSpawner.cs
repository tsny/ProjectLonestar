using UnityEngine;
using System.Collections;

public class ShipSpawner : MonoBehaviour
{
    public GameObject defaultShip;

    public static Loadout CreateLoadout()
    {
        Loadout loadout = ScriptableObject.CreateInstance<Loadout>();
        loadout.equipment.Add(ScriptableObject.CreateInstance<Weapon>());
        loadout.equipment.Add(ScriptableObject.CreateInstance<Weapon>());

        return loadout;
    }

    public void SpawnDefaultShip()
    {
        Instantiate(defaultShip);
    }

    public Ship SpawnPlayerShip(GameObject shipPrefab, Loadout loadout, Vector3 spawnPosition)
    {
        Ship playerShip = SpawnShip(shipPrefab, loadout, spawnPosition);

        FindObjectOfType<PlayerController>().Possess(playerShip);

        return playerShip;
    }

    public Ship SpawnShip(GameObject shipPrefab, Loadout loadout, Vector3 spawnPosition)
    {
        Ship ship = Instantiate(shipPrefab, spawnPosition, Quaternion.identity).GetComponent<Ship>();
        Inventory inventory = ship.GetComponentInChildren<Inventory>();

        if (loadout == null)
        {
            print("Loadout was null, giving ship defualt loadout...");
            inventory.Initialize(CreateLoadout());
            return ship;
        }

        inventory.Initialize(loadout);
        return ship;
    }
}

public class ShipSpawnInfo
{
    public int numTimesToSpawn = 1;
    public GameObject shipPrefab;
    public Loadout loadout;
}
