using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float distanceTraveled = 0f;

    public GameObject owner;

    public Weapon weapon;
    public Vector3 target;
    public ParticleSystem mainEffect;
    public ParticleSystem impactEffect;

    private new Collider collider;
    private Rigidbody rb;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        distanceTraveled = 0f;
        target = Vector3.zero;
        startPosition = transform.position;
    }

    // Be sure to call this after instantiation
    public void Initialize(Ship owner, Weapon weapon)
    {
        this.weapon = weapon;
        this.owner = owner.gameObject;

        Physics.IgnoreCollision(owner.GetComponentInChildren<Collider>(), collider);

        target = owner.aimPosition;

        transform.LookAt(target);
        rb.AddForce(transform.forward * weapon.thrust, ForceMode.Impulse);
        mainEffect.Play();
    }

    private void LateUpdate()
    {
        distanceTraveled = Vector3.Distance(transform.position, startPosition);

        if (distanceTraveled > weapon.range) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(rb);
        Destroy(collider);

        HealthComponent healthComponent = collision.collider.transform.root.GetComponentInChildren<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(weapon);
        }

        mainEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        impactEffect.Play();

        Destroy(gameObject, 2);
    }
} 