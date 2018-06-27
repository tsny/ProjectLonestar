using UnityEngine;

public class Loot : WorldObject
{
    public Item item;
    public Transform target;

    public bool beingLooted;
    public float pickupRange = 5;
    public float outOfBoundsRange = 100;
    [ReadOnly]
    public float distanceToTarget;

    [Range(0, 1)]
    public float pullForce = .5f;

    private Loot()
    {
        worldObjectType = WorldObjectType.Loot;
    }

    protected override void SetName()
    {
        name = "loot_" + item.name + " x" + item.quantity;
    }

    public override void SetupTargetIndicator(TargetIndicator indicator)
    {
        indicator.targetHealth = hullHealth;

        indicator.name = item.name + " x" + item.quantity;
        indicator.header.text = indicator.name;
    }

    public void SetTarget(Transform newTarget, float pullForce)
    {
        target = newTarget;
        beingLooted = true;
        this.pullForce = pullForce;

        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
    }

    public void ClearTarget()
    {
        target = null;
        beingLooted = false;
    }

    public void GravitateTowardsLooter()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget > outOfBoundsRange)
        {
            ClearTarget();
            return;
        }

        if (distanceToTarget < pickupRange)
        {
            Inventory targetInventory = target.root.GetComponentInChildren<Inventory>();

            if (targetInventory == null)
            {
                ClearTarget();
                return;
            }

            targetInventory.AddItem(item);
            Die();
        }

        transform.LookAt(target.transform);
        transform.position = Vector3.Lerp(transform.position, target.transform.position, pullForce * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (beingLooted && target != null)
        {
            GravitateTowardsLooter();
        }
    }

    public override void TakeDamage(Weapon weapon)
    {
        if (invulnerable) return;

        hullHealth -= weapon.hullDamage;

        if (hullHealth <= 0) Die();

        OnTookDamage(false, weapon.hullDamage);
    }
}
