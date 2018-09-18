using UnityEngine;

public class Loot : WorldObject
{
    public Item item;
    public Transform target;

    public bool beingLooted;
    public float pickupRange = 5;
    public float outOfBoundsRange = 100;

    public float distanceToTarget;

    [Range(0, 1)]
    public float pullForce = .5f;

    protected override void Awake()
    {
        base.Awake();
        enabled = false;
    }

    protected override void SetHierarchyName()
    {
        if (item == null) return;
        name = "loot_" + item.name + " x" + item.quantity;
    }

    public void SetTarget(Transform newTarget, float pullForce)
    {
        target = newTarget;
        beingLooted = true;
        this.pullForce = pullForce;

        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        enabled = true;
    }

    public void ClearTarget()
    {
        target = null;
        beingLooted = false;
        enabled = false;
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
            return;
        }

        transform.LookAt(target.transform);
        transform.position = Vector3.Lerp(transform.position, target.transform.position, pullForce * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (beingLooted && target != null) GravitateTowardsLooter();
    }

    public override void TakeDamage(Weapon weapon)
    {
        base.TakeDamage(weapon);

        hullHealth -= weapon.hullDamage;

        if (hullHealth <= 0) Die();

        OnTookDamage(false, weapon.hullDamage);
    }
}
