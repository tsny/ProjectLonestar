using UnityEngine;
using System.Linq;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private float distanceTraveled = 0f;
    private Vector3 startPosition;

    public WeaponStats stats;

    public ParticleSystem mainEffect;
    public ParticleSystem impactEffect;

    public Rigidbody rb;
    public Collider coll;

    public float DistanceTraveled
    {
        get
        {
            return distanceTraveled;
        }
    }

    public Projectile Initialize(Vector3 target, WeaponStats stats, Collider[] collidersToIgnore = null)
    {
        if (stats != null)
            this.stats = stats;
        else
            stats = Utilities.CheckScriptableObject<WeaponStats>(stats);

        transform.LookAt(target);

        if (collidersToIgnore != null)
        {
            foreach (var collider in collidersToIgnore)
            {
                Physics.IgnoreCollision(collider, this.coll);
            }
        }

        Accelerate();
        return this;
    }

    public void Accelerate()
    {
        stats = Utilities.CheckScriptableObject<WeaponStats>(stats);

        startPosition = transform.position;
        rb.velocity = transform.forward * stats.thrust;
        StartCoroutine(RangeChecker());
    }

    private IEnumerator RangeChecker()
    {
        while (true)
        {
            distanceTraveled = Vector3.Distance(transform.position, startPosition);

            if (distanceTraveled > stats.range) 
            {
                Destroy(gameObject);
                yield break;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(rb);
        Destroy(coll);

        if (collision.rigidbody != null)
        {
            collision.rigidbody.SendMessage("TakeDamage", stats, SendMessageOptions.DontRequireReceiver);
        }

        mainEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        impactEffect.Play();

        Destroy(gameObject, 2f);
    }
} 