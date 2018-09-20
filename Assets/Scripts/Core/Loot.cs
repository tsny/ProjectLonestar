using UnityEngine;

public class Loot : MonoBehaviour
{
    public Item item;
    public Transform target;

    public bool beingLooted;
    public float pickupRange = 5;
    public float outOfBoundsRange = 100;

    public float distanceToTarget;

    [Range(0, 1)]
    public float pullForce = .5f;

    void Awake()
    {
        enabled = false;
    }

    void LateUpdate()
    {
        if (beingLooted && target != null) GravitateTowardsLooter();
    }

    public void SetHierarchyName()
    {
        if (item == null) return;
        name = "loot_" + item.name + " x" + item.quantity;
    }

    public void SetTarget(Transform newTarget, float pullForce)
    {
        target = newTarget;
        this.pullForce = pullForce;
        beingLooted = true;

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
            Inventory targetInventory = GameSettings.Instance.playerInventory;

            if (targetInventory == null)
            {
                ClearTarget();
                return;
            }

            targetInventory.AddItem(item);

            Destroy(gameObject);

            return;
        }

        transform.LookAt(target.transform);
        transform.position = Vector3.Lerp(transform.position, target.transform.position, pullForce * Time.deltaTime);
    }
}
