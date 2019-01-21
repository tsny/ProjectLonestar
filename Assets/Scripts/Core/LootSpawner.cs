using UnityEngine;

public class LootSpawner
{
    public static Loot SpawnLoot(Vector3 position, LootSpawnInfo info)
    {
        Loot loot = Loot.Instantiate(GameSettings.Instance.lootPrefab, position, Quaternion.identity);

        loot.item = info.item;
        loot.SetParticleColors(info.gradient);
        var random = Random.Range(info.impulseLowerRange, info.impulseUpperRange);

        loot.rb.AddForce(random, random, random, ForceMode.Impulse);
        // Do some kinda logic to determine the loot's rarity

        return loot;
    }
}

[System.Serializable]
public class LootSpawnInfo
{
    public Item item;
    public Gradient gradient;
    public float impulseLowerRange = 10;
    public float impulseUpperRange = 30;
}