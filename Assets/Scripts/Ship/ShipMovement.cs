using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Ship))]
public class ShipMovement : ShipComponent
{
    public EngineState engineState;

    public GameObject cruiseEffect;

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
    private Timer cruiseChargeTimer;
    public bool cruiseDisrupted;
    public float cruiseChargeDuration = 5f;
    public AudioSource cruiseChargeSource;

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
            engineState = EngineState.Normal;
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
        engineState = EngineState.Normal;
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

    public void StartChargingCruise(bool skipCharge)
    {
        if (skipCharge)
        {
            BeginCruising();
            return;
        }

        if (cruiseEffect != null)
        {
            cruiseEffect.SetActive(true);
        }

        owningShip.hardpointSystem.afterburnerHardpoint.Disable();

        throttle = 1;
        engineState = EngineState.Charging;

        cruiseChargeTimer = gameObject.AddComponent<Timer>();
        cruiseChargeTimer.Initialize(cruiseChargeDuration, this, "BeginCruising");
    }

    public void StopChargingCruise()
    {
        engineState = EngineState.Normal;

        if (cruiseChargeTimer != null)
        {
            Destroy(cruiseChargeTimer);
        }

        if (cruiseEffect != null)
        {
            cruiseEffect.SetActive(false);
        }
    }

    public void BeginCruising()
    {
        engineState = EngineState.Cruise;

        if (cruiseEffect != null)
        {
            cruiseEffect.SetActive(false);
        }
    }

    public void StopCruising()
    {
        engineState = EngineState.Normal;
    }

    public void StopAnyCruise()
    {
        if (IsCruising)
        {
            StopCruising();
            return;
        }

        if (IsChargingCruise)
        {
            StopChargingCruise();
            return;
        }
    }

    public void Drift()
    {
        StopAnyCruise();
        engineState = EngineState.Drifting;
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
        Quaternion newRot = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, turnSpeed * Time.deltaTime);
    }
}
