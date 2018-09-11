using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Ship))]
public class ShipEngine : ShipComponent
{
    public EngineState engineState;

    [Range(0.01f, 0.05f)]
    public float throttleChangeIncrement = .1f;
    public float turnSpeed = 1f;

    public float throttle;
    public int strafe;

    public bool IsStrafing
    {
        get { return (strafe == 0) ? false : true; }
    }

    [Header("Cruise")]
    [Space(5)]
    public bool cruiseDisrupted;
    public float cruiseChargeDuration = 5f;
    public AudioSource cruiseChargeSource;

    private IEnumerator cruiseChargeCoroutine;
    private IEnumerator rotateCoroutine;

    public bool CanAfterburn
    {
        get
        {
            return !(engineState == EngineState.Charging || engineState == EngineState.Cruise);
        }
    }

    public bool CanChargeCruise
    {
        get
        {
            if (cruiseDisrupted) return false;

            switch (engineState)
            {
                case EngineState.Charging:
                case EngineState.Cruise:

                    return false;

                case EngineState.Normal:
                case EngineState.Drifting:
                case EngineState.Reversing:

                    return true;

                default:

                    return false;
            }
        }
    }

    public bool IsChargingCruise
    {
        get
        {
            return (engineState == EngineState.Charging);
        }
    }

    public bool IsCruising
    {
        get
        {
            return (engineState == EngineState.Cruise);
        }
    }

    public bool HasThrottleControl
    {
        get
        {
            switch (engineState)
            {
                case EngineState.Normal:
                case EngineState.Drifting:
                case EngineState.Reversing:
                    return true;

                case EngineState.Charging:
                case EngineState.Cruise:
                    return false;

                default:
                    return false;
            }
        }
    }

    public event EngineEventHandler EngineStateChanged;
    public event EngineEventHandler CruiseChanged;

    public delegate void EngineEventHandler(ShipEngine sender);

    private void OnEngineUpdate()
    {
        if (EngineStateChanged != null) EngineStateChanged(this);
    }

    private void OnCruiseChange(EngineState newState)
    {
        engineState = newState;
        if (CruiseChanged != null) CruiseChanged(this);
    }

    public void ToggleCruiseEngines()
    {
        if (IsCruising) StopCruising();

        else if (IsChargingCruise) StopChargingCruise();

        else if (CanChargeCruise) StartChargingCruise(false);
    }

    public void ThrottleUp(float value = 0)
    {
        if (engineState == EngineState.Drifting)
        {
            OnCruiseChange(EngineState.Normal);
        }

        if (value != 0)
        {
            throttle = value;
            return;
        }

        throttle = Mathf.MoveTowards(throttle, 1, throttleChangeIncrement);
    }

    public void ThrottleDown()
    {
        StopAnyCruise();

        throttle = Mathf.MoveTowards(throttle, 0, throttleChangeIncrement);
        OnCruiseChange(EngineState.Normal);
    }

    public void Rotate(float yaw, float pitch)
    {
        transform.Rotate(pitch * turnSpeed, yaw * turnSpeed, 0);
    }

    public void ChangeStrafe(int newStrafe)
    {
        strafe = Mathf.Clamp(newStrafe, -1, 1);
    }
    
    public void ResetStrafe()
    {
        strafe = 0;
    }

    public void StartChargingCruise(bool skipCharge = false)
    {
        if (skipCharge)
        {
            OnCruiseChange(EngineState.Cruise);
            return;
        }

        cruiseChargeCoroutine = CruiseChargeCoroutine();
        StartCoroutine(cruiseChargeCoroutine);
    }

    public void StopChargingCruise()
    {
        OnCruiseChange(EngineState.Normal);
        StopCoroutine(cruiseChargeCoroutine);
    }

    public void StopCruising()
    {
        OnCruiseChange(EngineState.Normal);
    }

    public void StopAnyCruise()
    {
        if (IsCruising) StopCruising();

        else if (IsChargingCruise) StopChargingCruise();
    }

    public void Drift()
    {
        StopAnyCruise();
        OnCruiseChange(EngineState.Drifting);
    }

    public void LerpYawToNeutral()
    {
        Quaternion neutralRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        neutralRotation = Quaternion.Lerp(transform.rotation, neutralRotation, Time.deltaTime);
        transform.rotation = neutralRotation;
    }

    public void TurnToTarget(Vector3 target)
    {
        Vector3 targetDir = target - transform.position;
        float step = turnSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void Pitch(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(new Vector3(turnSpeed * -amount, 0, 0));
    }

    public void Yaw(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(new Vector3(0, turnSpeed * amount, 0));
    }

    public void Roll(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(new Vector3(0, 0, turnSpeed * amount));
    }

    public void RotateTowardsTarget(Transform target)
    {
        rotateCoroutine = RotateTowardsTargetCoroutine(target);
        StartCoroutine(rotateCoroutine);
    }

    public void StopRotating()
    {
        if (rotateCoroutine == null) return;
        StopCoroutine(rotateCoroutine);
    }

    private IEnumerator RotateTowardsTargetCoroutine(Transform target)
    {
        for (; ;)
        {
            Quaternion newRot = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, turnSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator CruiseChargeCoroutine(bool skipCharge = false)
    {
        OnCruiseChange(EngineState.Charging);
        throttle = 1;

        var chargeTime = 0f;

        for (;;)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime > cruiseChargeDuration) break;
            yield return null;
        }

        OnCruiseChange(EngineState.Cruise);
    }

}
