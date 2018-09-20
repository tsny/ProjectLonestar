using UnityEngine;
using System.Collections;

public abstract class HealthComponent : ShipComponent
{
    public abstract void TakeDamage(Weapon weapon);
    public abstract void OnHealthDepleted(Weapon weapon);
}
