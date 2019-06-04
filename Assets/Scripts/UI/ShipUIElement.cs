using System.Collections;
using UnityEngine;

public abstract class ShipUIElement : MonoBehaviour
{
    public PlayerController pc;
    public Ship ship;

    public virtual void Init(PlayerController pc)
    {
        this.pc = pc;
        ship = pc.ship;
        pc.PossessedNewShip += OnPossessed;
        pc.ReleasedShip += OnReleased;
    }

    protected virtual void Clear()
    {
        pc = null;
        ship = null;
    }

    public abstract void OnPossessed(PlayerController pc, PossessionEventArgs e);
    public abstract void OnReleased(PlayerController pc, PossessionEventArgs e);
}