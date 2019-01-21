using UnityEngine;

public class Tradelane : MonoBehaviour, ITargetable
{
    public Transform transformToMove;
    public Health health;
    public Collider coll;
    public Transform endPoint;
    public float thrust = 30;

    public event TargetEventHandler BecameTargetable;
    public event TargetEventHandler BecameUntargetable;

    private void Awake() 
    {
        health = Utilities.CheckComponent<Health>(gameObject);
    }

    public bool IsTargetable()
    {
        //throw new System.NotImplementedException();
        return true;
    }

    public void SetupTargetIndicator(TargetIndicator indicator)
    {
        //throw new System.NotImplementedException();
        indicator.header.text = "Trade Lane";
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