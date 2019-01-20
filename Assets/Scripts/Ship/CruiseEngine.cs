using UnityEngine;
using System.Collections;

public enum CruiseState { Off, Charging, On, Disrupted };

public class CruiseEngine : ShipComponent
{
    public Rigidbody rb;

    private CruiseState state;
    public CruiseState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
            OnCruiseChange(value);
        }
    }

    public CruiseEngineStats stats;

    public delegate void CruiseChangeEventHandler(CruiseEngine sender, CruiseState newState);
    public event CruiseChangeEventHandler CruiseStateChanged;

    private IEnumerator chargeCoroutine;

    private void Awake()
    {
        stats = Utilities.CheckScriptableObject<CruiseEngineStats>(stats);
    }

    private void OnCruiseChange(CruiseState newState)
    {
        if (CruiseStateChanged != null) CruiseStateChanged(this, newState);
    }

    public void StopAnyCruise()
    {
        if (State == CruiseState.Off) return;

        if (State == CruiseState.Charging)
        {
            StopCoroutine(chargeCoroutine);
        }

        State = CruiseState.Off;
    }

    public bool TryChargeCruise(bool skipChargeDuration = false)
    {
        if (State != CruiseState.On)
        {
            return false;
        }

        if (skipChargeDuration)
        {
            State = CruiseState.On;
            return true;
        }

        else
        {
            chargeCoroutine = ChargeCruiseCoroutine();
            StartCoroutine(chargeCoroutine);
            return true;
        }
    }

    private void FixedUpdate()
    {
        if (State == CruiseState.On)
        {
            rb.AddForce(rb.transform.forward * stats.thrust * stats.thrustMultiplier);
        }
    }

    public void ToggleCruiseEngines()
    {
        switch (State)
        {
            case CruiseState.Off:
                chargeCoroutine = ChargeCruiseCoroutine();
                StartCoroutine(chargeCoroutine);
                break;

            case CruiseState.Charging:
                StopCoroutine(chargeCoroutine);
                State = CruiseState.Off;
                break;

            case CruiseState.On:
                State = CruiseState.Off;
                break;

            default:
                break;
        }
    }

    private IEnumerator ChargeCruiseCoroutine()
    {
        State = CruiseState.Charging;

        yield return new WaitForSeconds(stats.chargeDuration);

        chargeCoroutine = null;
        State = CruiseState.On;
    }
}
