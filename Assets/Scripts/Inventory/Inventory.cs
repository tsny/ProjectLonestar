using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : ShipComponent
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
    
    protected override void Awake()
    {
        base.Awake();

        items = new List<Item>(owningShip.cargoHoldCapacity);
    }

    public void Initialize(Loadout loadout)
    {
        items.AddRange(loadout.equipment.ToArray());

        MountHardpoints(loadout);
    }

    public bool AddItem(Item item)
    {
        if (item.canStack)
        {
            Item foundItem = FindNonFullStack(item);

            if(foundItem != null)
            {
                if (foundItem.SpaceRemaining >= item.quantity)
                {
                    foundItem.quantity += item.quantity;
                    return true;
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
                return true;
            }
        }

        else
        {
            if(!IsFull)
            {
                items.Add(item);
                return true;
            }

            else return false;
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

    public void JettisonItem()
    {
        Instantiate(lootPrefab, transform.position - Vector3.down, Quaternion.identity);
    }

    public void MountHardpoints(Loadout newLoadout)
    {
        List<Hardpoint> hardpoints = owningShip.GetComponentInChildren<HardpointSystem>().hardpoints;

        foreach (Equipment equipment in newLoadout.equipment)
        {
            foreach(Hardpoint hardpoint in hardpoints)
            {
                if (hardpoint.IsMounted) continue;

                if (hardpoint.associatedEquipmentType == equipment.GetType())
                {
                    hardpoint.Mount(equipment);
                    break;
                }
            }
        }
    }
}

