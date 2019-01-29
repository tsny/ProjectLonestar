using UnityEngine;

public class Tradelane : MonoBehaviour
{
    public TargetingInfo info;
    public Transform transformToMove;
    public Health health;
    public Collider coll;
    public Transform endPoint;
    public float thrust = 30;

    private void Awake() 
    {
        health = Utilities.CheckComponent<Health>(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Ship") || other.CompareTag("Player")) 
        {
            MoveTarget(other.attachedRigidbody);
        }
    }

    public void MoveTarget(Rigidbody target)
    {
        if (endPoint == null) return;

        target.transform.LookAt(endPoint);
        target.constraints = RigidbodyConstraints.FreezeRotation;
        //target.isKinematic = true;
        target.velocity = target.transform.forward * thrust;
    }
}