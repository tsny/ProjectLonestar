using System.Collections;
using UnityEngine;

public class Tradelane : MonoBehaviour
{
    public TargetingInfo info;
    public Transform transformToMove;
    public Health health;
    public Collider coll;
    public Transform endPoint;

    public float thrust = 30;
    public float accelDuration = 3;
    public float turnSpeed = 1;
    public float desiredAngle = 1;
    public float totalDuration = 5;

    private void Awake() 
    {
        health = Utilities.CheckComponent<Health>(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Ship") || other.CompareTag("Player")) 
        {
            StartCoroutine(WarpRoutine(other.attachedRigidbody.GetComponent<Ship>()));
        }
    }

    private IEnumerator WarpRoutine(Ship ship)
    {
        if (!endPoint) yield break;
        float elapsed = 0;
        float elapsedAccel = 0;

        ship.engine.Strafe = 0;
        ship.engine.Throttle = 0;

        while (true)
        {
            Quaternion newRot = Quaternion.LookRotation(endPoint.position - ship.transform.position);
            ship.transform.rotation = Quaternion.Slerp(ship.transform.rotation, newRot, turnSpeed);

            if (Vector3.Angle(ship.transform.forward, endPoint.position - ship.transform.position) < desiredAngle)
            {
                ship.rb.velocity = new Vector3(0,0, thrust * (elapsedAccel / accelDuration));
                elapsedAccel += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            if (elapsed > totalDuration) yield break;
            elapsed += Time.deltaTime;

            yield return new WaitForFixedUpdate();
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