using UnityEngine;

[CreateAssetMenu(fileName = "DropList", menuName = "Loot/DropList", order = 0)]
public class DropList : ScriptableObject 
{
    public Drop[] drops;
}

[System.Serializable]
public struct Drop
{
    public LootSpawnInfo spawnInfo;
    public float chance;
}