using UnityEngine;

[CreateAssetMenu(fileName = "DropList", menuName = "Loot/DropList", order = 0)]
public class DropList : ScriptableObject 
{
    public Drop[] drops;

    public void Evaluate()
    {
        foreach (Drop drop in drops)
        {
            if (Utilities.Chance(drop.chance))
            {

            }
        }
    }
}

[System.Serializable]
public struct Drop
{
    public LootSpawnInfo spawnInfo;
    public float chance;
}