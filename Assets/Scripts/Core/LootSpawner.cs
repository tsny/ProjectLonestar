using System.Collections.Generic;
using UnityEngine;

public class LootSpawner
{
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

        // Do some kinda logic to determine the loot's rarity

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

        loot.rb.AddForce(randomX, randomY, randomZ, ForceMode.Impulse);

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