using UnityEngine;

public enum WorldObjectType
{
    Ship,
    Station,
    Loot,
    Wreck,
    Jumpgate,
    Tradelane,
    Star,
    Anomaly,
    Jumphole
}

public class WorldObject : MonoBehaviour
{
    protected WorldObjectType worldObjectType;

    public bool invulnerable;
    public float hullHealth = 100;
    public float hullFullHealth = 100;

    public delegate void WorldObjectExplodedEventHandler(WorldObject sender);
    public event WorldObjectExplodedEventHandler WorldObjectExploded;

    protected virtual void OnDestroyed()
    {
        if (WorldObjectExploded != null) WorldObjectExploded(this);
    }

    protected virtual void SetName()
    {
        name = "World Object";
    }

    protected virtual void Awake()
    {
        SetName();
    }

    public virtual void SetupTargetIndicator(TargetIndicator indicator)
    {

    }

    public virtual void TakeDamage(Weapon weapon)
    {
        if (invulnerable)
        {
            return;
        }
    }
}
