using UnityEngine;
using System.Collections;

public class ShipNavigation : ShipComponent
{
    public Transform target;
    public float turnSpeed = 1;
    public float turnAngleThreshold = 5;

    [Tooltip("The smallest angle the ship must be from the target in order to fire at it")]
    public float fireAngleThreshold = 10;
    public float delayBetweenShots = .1f;

    [Tooltip("The distance away a target must be in order to consider using cruise engines")]
    public float cruiseDistanceThreshold = 1000;

    private IEnumerator rotateCoroutine;
    private IEnumerator fireCoroutine;

    // For testing
    public void FireAtPlayer()
    {
        fireCoroutine = FireAtTargetCoroutine(FindObjectOfType<PlayerController>().ship.transform);
        StartCoroutine(fireCoroutine);
    }

    public void GotoTarget(Transform target)
    {
        //RotateTowardsTarget(target);
        // Throttle Up
        // Check for faster routes, i.e trade rails
        // Decide if go to cruise
    }

    private IEnumerator RotateTowardsTargetCoroutine(Transform target, float angleThreshold)
    {
        for (; ;)
        {
            Quaternion newRot = Quaternion.LookRotation(target.position - owningShip.transform.position);
            transform.rotation = Quaternion.Slerp(owningShip.transform.rotation, newRot, turnSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.Slerp(owningShip.transform.rotation, newRot, turnSpeed * Time.deltaTime);

            if (Quaternion.Angle(owningShip.transform.rotation, newRot) < angleThreshold) break;
            yield return null;
        }

        rotateCoroutine = null;
    }

    private IEnumerator FireAtTargetCoroutine(Transform target, int amountOfShots = 50, float delayBetweenShots = .05f)
    {
        this.target = target;

        int remainingShots = amountOfShots;

        while (true)
        {
            yield return StartCoroutine(RotateTowardsTargetCoroutine(target, fireAngleThreshold));
            owningShip.aimPosition = target.position;
            //ship.hardpointSystem.FireActiveWeapons();
            remainingShots--;
            print(remainingShots);
            if (remainingShots <= 0) break;
            yield return new WaitForSeconds(delayBetweenShots);
        }

        fireCoroutine = null;
    }
}
