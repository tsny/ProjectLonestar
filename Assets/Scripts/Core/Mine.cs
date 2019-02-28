using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour 
{
    public float explosionForce = 100;    
    public float pullForce = 10;
    public float detonationRange = 100;
    public float activationDelay = 1;
    public float lifetime = 10;

    private float elapsedTime;

    private List<GameObject> nearbyObjects;

    private Transform _target;
    public Transform Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
        }
    }

    void Awake()
    {
        enabled = false;
        Invoke("Enable", activationDelay);        
    }

    private void OnTriggerStay(Collider other) 
    {
        if (!enabled || Target != null) return;
        Target = other.transform;
        //if (!nearbyObjects.Contains(other.gameObject)) nearbyObjects.Add(other.gameObject);     
    }

    void Enable()
    {
        enabled = true;
    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, pullForce);
        var shouldExplode = elapsedTime > lifetime;

        if (Vector3.Distance(transform.position, Target.position) < detonationRange || shouldExplode)
        {
            enabled = false;
            Destroy(gameObject);
        }
    }

}