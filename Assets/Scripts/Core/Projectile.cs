using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour
{
    public float distanceTraveled = 0f;
    //public float distanceTillColliderEnable = 2;

    public WeaponStats weaponStats;

    public ParticleSystem mainEffect;
    public ParticleSystem impactEffect;

    private new Collider collider;
    private Rigidbody rb;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        if (weaponStats == null)
            weaponStats = Instantiate(ScriptableObject.CreateInstance<WeaponStats>());

        distanceTraveled = 0f;
        startPosition = transform.position;
    }

    public void Initialize(Vector3 target, Collider[] collidersToIgnore = null)
    {
        transform.LookAt(target);

        rb.AddForce(transform.forward * weaponStats.thrust, ForceMode.Impulse);

        if (collidersToIgnore != null)
        {
            foreach (var collider in collidersToIgnore)
            {
                Physics.IgnoreCollision(collider, this.collider);
            }
        }

        mainEffect.Play();
    }

    private void Update()
    {
        distanceTraveled = Vector3.Distance(transform.position, startPosition);
        if (distanceTraveled > weaponStats.range) Destroy(gameObject);
    }

    // Maybe change this so that it spawns the particle systems and destroys immediately
    // Right now we just disable the script
    private void OnCollisionEnter(Collision collision)
    {
        enabled = false;

        Destroy(rb);
        Destroy(collider);

        transform.position = collision.contacts[0].point;

        IDamageable damageableObject = collision.collider.transform.root.GetComponentInChildren<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(weaponStats);
        }

        mainEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        impactEffect.Play();

        Destroy(gameObject, 2);
    }
} 