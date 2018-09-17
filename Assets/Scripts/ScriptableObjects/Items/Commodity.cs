using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/Commodity")]
public class Commodity : Item
{
    public Commodity()
    {
        canStack = true;
        name = "commodity";
        stackLimit = 10;
    }
}
