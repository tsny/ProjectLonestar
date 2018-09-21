using UnityEngine;
using System.Linq;

public class ProjectileController : MonoBehaviour
{
    public float distanceTraveled = 0f;
    public float distanceTillColliderEnable = 2;

    public Weapon weapon;

    public ParticleSystem mainEffect;
    public ParticleSystem impactEffect;

    private new Collider collider;
    private Rigidbody rb;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        //gameObject.hideFlags = HideFlags.HideInHierarchy;
        distanceTraveled = 0f;
        startPosition = transform.position;

        collider.enabled = false;
    }

    // Possible params: Collider[] colidersToIgnore
    public void Initialize(Weapon weapon, Vector3 target)
    {
        this.weapon = weapon;
        transform.LookAt(target);
        rb.AddForce(transform.forward * weapon.thrust, ForceMode.Impulse);
        mainEffect.Play();
    }

    private void Update()
    {
        if (distanceTraveled > distanceTillColliderEnable)
        {
            collider.enabled = true;
        }

        distanceTraveled = Vector3.Distance(transform.position, startPosition);

        if (distanceTraveled > weapon.range) Destroy(gameObject);
    }

    // Maybe change this so that it spawns the particle systems and destroys immediately
    // Right now we just disable the script
    private void OnCollisionEnter(Collision collision)
    {
        enabled = false;

        Destroy(rb);
        Destroy(collider);

        IDamageable damageableObject = collision.collider.transform.root.GetComponentInChildren<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(weapon);
        }

        mainEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        impactEffect.Play();

        Destroy(gameObject, 2);
    }
} 