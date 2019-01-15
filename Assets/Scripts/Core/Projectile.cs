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

    public Projectile Initialize(Vector3 target, WeaponStats stats, Collider[] collidersToIgnore = null)
    {
        if (stats != null)
            this.stats = stats;
        else
            Debug.LogWarning("Firing projectile from gun without passing stats...");

        transform.LookAt(target);

        if (collidersToIgnore != null)
        {
            foreach (var collider in collidersToIgnore)
            {
                Physics.IgnoreCollision(collider, this.coll);
            }
        }

        Accelerate();
        StartCoroutine(RangeChecker());

        return this;
    }

    public void Accelerate()
    {
        startPosition = transform.position;
        rb.velocity = transform.forward * stats.thrust;
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

            yield return new WaitForSeconds(1);
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