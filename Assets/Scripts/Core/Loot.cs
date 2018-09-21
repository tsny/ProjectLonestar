using System.Collections;
using UnityEngine;

public class Loot : MonoBehaviour, ITargetable
{
    public Item item;
    public Transform target;

    public bool beingLooted;
    public float pickupRange = 5;
    public float outOfBoundsRange = 100;

    public float distanceToTarget;

    [Range(0, 10)]
    public float pullForce = .5f;

    public event TargetEventHandler BecameTargetable;
    public event TargetEventHandler BecameUntargetable;

    private void OnBecameUntargetable()
    {
        if (BecameUntargetable != null) BecameUntargetable(this);
    }

    public void SetTarget(Transform newTarget, float pullForce)
    {
        ClearTarget();

        target = newTarget;
        beingLooted = true;
        this.pullForce = pullForce;
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        StartCoroutine(GravitateCoroutine());
    }

    public void ClearTarget()
    {
        StopAllCoroutines();

        target = null;
        beingLooted = false;
    }

    private IEnumerator GravitateCoroutine()
    {
        for (; ;)
        {
            GravitateTowardsLooter();
            yield return new WaitForFixedUpdate();
        }
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
                print("ERROR: No inventory");
                return;
            }

            targetInventory.AddItem(item);

            OnBecameUntargetable();
            Destroy(gameObject);
            return;
        }

        transform.LookAt(target.transform);
        transform.position = Vector3.Lerp(transform.position, target.transform.position, pullForce * Time.deltaTime);
    }

    public bool IsTargetable()
    {
        throw new System.NotImplementedException();
    }

    public void SetupTargetIndicator(TargetIndicator indicator)
    {
        //throw new System.NotImplementedException();
    }
}
