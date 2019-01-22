using System.Collections;
using UnityEngine;

public class Loot : MonoBehaviour, ITargetable
{
    public Item item;
    public Transform target;

    public ParticleSystem deathFX;

    public bool isBeingLooted;
    public float pickupRange = 5;
    public float outOfBoundsRange = 100;

    public float DistanceToTarget
    {
        get { return target ? Vector3.Distance(transform.position, target.transform.position) : 0; }
    }

    [Range(0, 10)]
    public float pullForce = 5;

    private Health health;
    private Rigidbody rb;
    private SphereCollider coll;

    public event TargetEventHandler BecameTargetable;
    public event TargetEventHandler BecameUntargetable;

    public static event LootEventHandler Spawned;
    public static event LootEventHandler Looted;

    public delegate void LootEventHandler(Loot sender);
    //public delegate void LootedEventHandler(Loot sender, Ship looter);

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
        coll = Utilities.CheckComponent<SphereCollider>(gameObject);
        coll.isTrigger = true;
    }

    private void Start() 
    {
        if (Spawned != null) Spawned(this);
        StartCoroutine(FinishSpawn());
    }

    public void Init(Vector3 impulse)
    {
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    private IEnumerator FinishSpawn(float waitDuration = 3)
    {
        yield return new WaitForSeconds(waitDuration);
        Destroy(rb);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            SetTarget(other.transform);
        }
    }

    private void HandleHealthDepleted()
    {
        if (deathFX != null)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void SetTarget(Transform newTarget)
    {
        ClearTarget();
        target = newTarget;
        isBeingLooted = true;
    }

    public void ClearTarget()
    {
        target = null;
        isBeingLooted = false;
    }

    private void FixedUpdate()
    {
        GravitateTowardsLooter();
    }

    public void GravitateTowardsLooter()
    {
        if (!target || !isBeingLooted) return;

        if (DistanceToTarget > outOfBoundsRange)
        {
            ClearTarget();
            return;
        }

        if (DistanceToTarget < pickupRange)
        {
            Inventory targetInventory = GameSettings.Instance.playerInventory;

            if (targetInventory == null)
            {
                ClearTarget();
                Debug.LogError("ERROR: No inventory");
                return;
            }

            targetInventory.AddItem(item);

            OnBecameUntargetable();
            if (Looted != null) Looted(this);

            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, pullForce);
    }

    [ContextMenu("test")]
    public void Test()
    {
        if (Looted != null) Looted(this);
    }

    public bool IsTargetable()
    {
        return true;
    }

    public void SetupTargetIndicator(TargetIndicator indicator)
    {
        indicator.header.text = "Loot: " + item.name ?? "Empty Loot";
        indicator.showHealthOnSelect = false;
        indicator.reticle.gameObject.SetActive(false);
    }

    public void SetParticleColors(Gradient gradient)
    {
        grad = gradient;
        var main = baseSystem.main;
        main.startColor = gradient;
        var sub = subSystem.main;
        sub.startColor = gradient;
        var trail = subSystem.trails;
        trail.colorOverLifetime = gradient;
    }
}
