using UnityEngine;
using System.Collections;
using System;

public class ShipNavigation : ShipComponent
{
    public TargetRelation relationTowardsTarget;
    public Transform target;

    public AnimationCurve evadeCurve;

    public float turnSpeed = 1;

    public bool HasTarget
    {
        get
        {
            bool val = target != null;

            if (val == false)
                Debug.LogWarning("ShipNav has no target...");

            return val;
        }
    }

    [Tooltip("The smallest angle the ship must be from the target in order to fire at it")]
    public float fireAngleThreshold = 10;
    public float delayBetweenShots = .1f;
    public float turnAngleThreshold = 5;

    [Tooltip("The distance away a target must be in order to consider using cruise engines")]
    public float cruiseDistanceThreshold = 1000;
    public float gotoDistanceThreshold = 500;
    public float combatTooCloseDistance = 100;

    public bool WeaponsFacingTarget
    {
        get
        {
            if (!HasTarget) return false;
            return Quaternion.Angle(ship.transform.rotation, target.transform.rotation) < fireAngleThreshold;
        }
    }

    public IEnumerator gotoRoutine;
    public IEnumerator rotateRoutine;
    public IEnumerator fireRoutine;
    public IEnumerator attackRoutine;
    public IEnumerator evadeRoutine;

    public void TargetPlayer()
    {
        target = FindObjectOfType<PlayerController>().ship.transform;

        if (target == null) 
            Debug.LogWarning("PlayerController does not possess ship...");
    }

    public void ClearTarget()
    {
        target = null;
        StopAllCoroutines();
    }

    public void StartCoroutine(IEnumerator cr, Func<IEnumerator> method)
    {
        if (cr != null)
            StopCoroutine(cr);

        cr = method();
        StartCoroutine(cr);
    }

    public void FullStop()
    {
        ship.engine.Throttle = 0;
        StopAllCoroutines();
    }

    public IEnumerator GotoTarget()
    {
        if (HasTarget == false) yield break;

        ship.engine.Throttle = 1;

        while (Vector3.Distance(transform.position, target.position) > gotoDistanceThreshold)
        {
            if (rotateRoutine == null)
                StartCoroutine(rotateRoutine, RotateTowardsTarget);

            yield return new WaitForSeconds(.5f);
        }

        ship.engine.Throttle = 0;
        gotoRoutine = null;
    }

    public IEnumerator RotateTowardsTarget()
    {
        if (HasTarget == false) yield break;

        for (; ;)
        {
            Quaternion newRot = Quaternion.LookRotation(target.position - ship.transform.position);
            transform.rotation = Quaternion.Slerp(ship.transform.rotation, newRot, turnSpeed * Time.deltaTime);

            if (Quaternion.Angle(ship.transform.rotation, newRot) < turnAngleThreshold) break;
            yield return null;
        }

        rotateRoutine = null;
    }

    public IEnumerator FireAtTarget()
    {
        if (HasTarget == false) yield break;

        float duration = 10;
        float time = duration;

        while (time <= duration)
        {
            if (WeaponsFacingTarget)
            {
                ship.aimPosition = target.position;
                //owningShip.FireActiveWeapons();
            }

            time -= delayBetweenShots;

            yield return new WaitForSeconds(delayBetweenShots);
        }

        fireRoutine = null;
    }

    public IEnumerator AttackRun()
    {
        if (HasTarget == false) yield break;

        StartCoroutine(GotoTarget());
        StartCoroutine(FireAtTarget());

        if (Vector3.Distance(transform.position, target.transform.position) < combatTooCloseDistance)
        {
            ship.engine.AddYaw(90);
        }
    }

    public IEnumerator PitchEvade()
    {
        float duration = 10;
        float time = duration;

        while (time <= duration)
        {
            ship.engine.AddPitch(evadeCurve.Evaluate(time / duration));

            yield return new WaitForSeconds(delayBetweenShots);
        }
    }
}
