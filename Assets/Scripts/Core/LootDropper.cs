using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public Drop[] dropList;

    [ContextMenu("Spawn Drops")]
    public void SpawnLootDrops()
    {
        if (dropList == null) return;

        foreach (Drop drop in dropList)
        {
            if (Utilities.Chance(drop.chance))
            {
                SpawnDropObject(drop);
            }
        }
    }

    private Loot SpawnDropObject(Drop drop)
    {
        Loot loot = Loot.Instantiate(GameSettings.Instance.lootPrefab, transform.position, Quaternion.identity);

        loot.item = drop.item;
        loot.SetParticleColors(drop.item.gradient);

        var randomX = Random.Range(-drop.impulse, drop.impulse);
        var randomY = Random.Range(-drop.impulse, drop.impulse);
        var randomZ = Random.Range(-drop.impulse, drop.impulse);

        loot.Init(new Vector3 (randomX, randomY, randomZ));
        return loot;
    }
}

[System.Serializable]
public class Drop
{
    public Item item;
    public float impulse = 10;
    public float chance;
}