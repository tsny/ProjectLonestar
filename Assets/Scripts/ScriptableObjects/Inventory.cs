using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : ScriptableObject
{
    public int Capacity { get { return items.Capacity; } }
    public int CurrentVolume { get { return items.Count; } }
    public int RemainingCapacity { get { return Capacity - CurrentVolume; } }

    public bool IsFull { get { return (CurrentVolume == Capacity); } }
    public bool IsEmpty { get { return (CurrentVolume == 0); } }

    public List<Item> Items
    {
        get
        {
            return items;
        }
        set
        {
            items = value;
        }
    }

    public List<Item> items;
    public GameObject lootPrefab;
    
    /// <summary>
    /// Adds an item to the inventory and returns a new item if the inventory is full 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Item AddItem(Item item)
    {
        if (item.canStack)
        {
            Item foundItem = FindNonFullStack(item);

            if(foundItem != null)
            {
                if (foundItem.SpaceRemaining >= item.quantity)
                {
                    foundItem.quantity += item.quantity;
                    return null;
                }

                else
                {
                    item.quantity -= foundItem.SpaceRemaining;
                    foundItem.quantity = foundItem.stackLimit;
                    return AddItem(item);
                }
            }

            else
            {
                items.Add(item);
                return null;
            }
        }

        else
        {
            if(!IsFull)
            {
                items.Add(item);
                return null;
            }

            else return item;
        }
    }

    public Item FindNonFullStack(Item item)
    {
        foreach(Item loopItem in items)
        {
            if (!loopItem.StackIsFull && loopItem.name == item.name)
            {
                return loopItem;
            }
        }

        return null;
    }

    public void JettisonItem(Item item, Vector3 spawnPosition)
    {
        Loot loot = Instantiate(lootPrefab, spawnPosition - Vector3.down, Quaternion.identity).GetComponent<Loot>();
        loot.item = item;
    }
}

