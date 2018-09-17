using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Trash,
    Common,
    Uncommon,
    Rare,
    Unique,
    Alien,
    Artifact,
    Blueprint,
    Solar
}

[Serializable]
public class Item : ScriptableObject 
{
    public new string name = "item";
    public int quantity = 1;
    public int stackLimit = 1;
    public bool canStack = false;
    public string description = "description";
    public int SpaceRemaining { get { return stackLimit - quantity; } }
    public bool StackIsFull { get { return (quantity == stackLimit) ? true : false; } }
    public Rarity rarity = Rarity.Common;

    public static Color GetMatchingColor(Item item)
    {
        Color newColor;

        switch (item.rarity)
        {
            case Rarity.Trash:
                newColor = Color.gray;
                break;
            case Rarity.Common:
                newColor = Color.white;
                break;
            case Rarity.Uncommon:
                newColor = Color.green;
                break;
            case Rarity.Rare:
                newColor = Color.blue;
                break;
            case Rarity.Unique:
                newColor = Color.cyan;
                break;
            case Rarity.Alien:
                newColor = Color.magenta;
                break;
            case Rarity.Solar:
                newColor = Color.red;
                break;
            default:
                newColor = Color.white;
                break;
        }

        return newColor;
    }
}


