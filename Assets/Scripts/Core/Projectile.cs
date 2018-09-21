using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour
{
    public float distanceTraveled = 0f;
    public float distanceTillColliderEnable = 2;

    public ProjectileStats projectileStats;

    public ParticleSystem mainEffect;
    public ParticleSystem impactEffect;

    private new Collider collider;
    private Rigidbody rb;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        if (projectileStats == null)
            projectileStats = Instantiate(ScriptableObject.CreateInstance<ProjectileStats>());

        //gameObject.hideFlags = HideFlags.HideInHierarchy;

        distanceTraveled = 0f;
        startPosition = transform.position;

        collider.enabled = false;
    }

    // Possible params: Collider[] colidersToIgnore
    public void Initialize(Vector3 target)
    {
        transform.LookAt(target);
        rb.AddForce(transform.forward * projectileStats.thrust, ForceMode.Impulse);
        mainEffect.Play();
    }

    private void Update()
    {
        if (distanceTraveled > distanceTillColliderEnable)
        {
            collider.enabled = true;
        }

        distanceTraveled = Vector3.Distance(transform.position, startPosition);

        if (distanceTraveled > projectileStats.range) Destroy(gameObject);
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
            //damageableObject.TakeDamage(projectileStats);
        }

        mainEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        impactEffect.Play();

        Destroy(gameObject, 2);
    }
} 