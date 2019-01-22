using System.Collections.Generic;
using UnityEngine;

public class LootSpawner
{
    // Spawns ALL loot in SpawnInfo
    public static void SpawnLoot(Vector3 position, ShipSpawnInfo info)
    {
        SpawnLoot(position, info.dl);
        SpawnLoot(position, info.lootInfo);
    }

    public static void SpawnLoot(Vector3 position, DropList dropList)
    {
        foreach (Drop drop in dropList.drops)
        {
            if (Utilities.Chance(drop.chance, true))
            {
                SpawnLootDrop(position, drop.spawnInfo);
            }
        }
    }

    public static Loot[] SpawnLoot(Vector3 position, LootSpawnInfo[] infoArray)
    {
        Loot[] lootDrops = new Loot[infoArray.Length];

        for (int i = 0; i < infoArray.Length; i++)
        {
            lootDrops[i] = SpawnLootDrop(position, infoArray[i]);
        }

        return lootDrops;
    }

    public static Loot SpawnLootDrop(Vector3 position, LootSpawnInfo info)
    {
        Loot loot = Loot.Instantiate(GameSettings.Instance.lootPrefab, position, Quaternion.identity);

        loot.item = info.item;
        loot.SetParticleColors(info.gradient);

        var randomX = Random.Range(-info.impulse, info.impulse);
        var randomY = Random.Range(-info.impulse, info.impulse);
        var randomZ = Random.Range(-info.impulse, info.impulse);

        loot.Init(new Vector3 (randomX, randomY, randomZ));
        return loot;
    }

    // Add ships stats to this
    public void CalculateLootDrop(Ship ship)
    {

    }
}

[System.Serializable]
public class LootSpawnInfo
{
    public Item item;
    public Gradient gradient;
    public float impulse = 10;
}