using System.Collections;
using UnityEngine;

public class Loot : MonoBehaviour, ITargetable
{
    public Item item;
    public Transform target;
    public Rigidbody rb;

    public ParticleSystem deathFX;

    public bool beingLooted;
    public float pickupRange = 5;
    public float outOfBoundsRange = 100;

    public float distanceToTarget;

    [Range(0, 10)]
    public float pullForce = .5f;

    private Health health;

    public event TargetEventHandler BecameTargetable;
    public event TargetEventHandler BecameUntargetable;

    public Gradient grad;

    public ParticleSystem baseSystem;
    public ParticleSystem subSystem;

    private void OnBecameUntargetable() { if (BecameUntargetable != null) BecameUntargetable(this); }
    private void OnBecameTargetable() { if (BecameTargetable != null) BecameTargetable(this); }

    private void Awake() 
    {
        health = Utilities.CheckComponent<Health>(gameObject);
        health.HealthDepleted += HandleHealthDepleted;
        item = Utilities.CheckScriptableObject<Item>(item);
        rb = Utilities.CheckComponent<Rigidbody>(gameObject);
    }

    private void HandleHealthDepleted()
    {
        if (deathFX != null)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
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
            // Inventory targetInventory = GameSettings.Instance.playerInventory;

            // if (targetInventory == null)
            // {
            //     ClearTarget();
            //     print("ERROR: No inventory");
            //     return;
            // }

            // targetInventory.AddItem(item);

            // OnBecameUntargetable();
            Destroy(gameObject);
            return;
        }

        transform.LookAt(target.transform);
        transform.position = Vector3.Lerp(transform.position, target.transform.position, pullForce * Time.deltaTime);
    }

    public bool IsTargetable()
    {
        return true;
    }

    public void SetupTargetIndicator(TargetIndicator indicator)
    {
        indicator.header.text = "Loot: " + item.name ?? "Empty Loot";
    }

    public void SetParticleColors(Gradient gradient)
    {
        var main = baseSystem.main;
        main.startColor = gradient;
        var sub = subSystem.main;
        sub.startColor = gradient;
        var trail = subSystem.trails;
        trail.colorOverLifetime = gradient;
    }
}
