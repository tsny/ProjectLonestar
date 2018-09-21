using UnityEngine;
using System.Collections;

public class CruiseEngine : ShipComponent
{
    public enum CruiseState { Off, Charging, On, Disrupted };

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

    public float chargeDuration = 5;
    public float cruisePower = 300;
    public int cruiseMultiplier = 1;

    public delegate void CruiseChangeEventHandler(CruiseEngine sender);
    public event CruiseChangeEventHandler CruiseStateChanged;

    private IEnumerator chargeCoroutine;

    private void OnCruiseChange(CruiseState newState)
    {
        if (CruiseStateChanged != null) CruiseStateChanged(this);
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
            rb.AddForce(rb.transform.forward * cruisePower * cruiseMultiplier);
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

        var timeElapsed = 0f;

        for (;;)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed > chargeDuration)
            {
                break;
            }

            yield return null;
        }

        chargeCoroutine = null;
        State = CruiseState.On;
    }
}
