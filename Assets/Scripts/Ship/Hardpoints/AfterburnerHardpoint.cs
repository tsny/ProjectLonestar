using System;
using System.Collections;
using UnityEngine;

public class AfterburnerHardpoint : MonoBehaviour
{
    public Rigidbody rb;
    public Afterburner afterburner;

    public float charge;

    public bool IsActive
    {
        get
        {
            return burnCoroutine != null;
        }
    }
    public bool IsOnCooldown
    {
        get
        {
            return cooldownCoroutine != null;
        }
    }

    private IEnumerator rechargeCoroutine;
    private IEnumerator burnCoroutine;
    private IEnumerator cooldownCoroutine;

    public event EventHandler<AfterburnerEventArgs> Activated;
    public event EventHandler<AfterburnerEventArgs> Deactivated;

    private void Awake()
    {
        if (afterburner == null)
            afterburner = ScriptableObject.CreateInstance<Afterburner>();

        afterburner = Instantiate(afterburner);

        if (rb == null)
            rb = GetComponentInChildren<Rigidbody>();

        charge = afterburner.capacity;
    }

    private void OnActivated()
    {
        if (Activated != null)
            Activated(this, new AfterburnerEventArgs(true));
    }

    private void OnDeactivated()
    {
        if (Deactivated != null)
            Deactivated(this, new AfterburnerEventArgs(false));
    }

    public void Activate()
    {
        if (rb == null) return;

        if (IsActive) return;

        burnCoroutine = Burn();
        StartCoroutine(burnCoroutine);

        OnActivated();
    }

    public void Deactivate()
    {
        if (!IsActive) return;

        if (burnCoroutine != null)
            StopCoroutine(burnCoroutine);

        burnCoroutine = null;

        BeginRecharge();

        OnDeactivated();
    }

    public void BeginRecharge()
    {
        rechargeCoroutine = Recharge();
        StartCoroutine(rechargeCoroutine);
    }

    public void EndRecharge()
    {
        if (rechargeCoroutine != null)
            StopCoroutine(rechargeCoroutine);

        rechargeCoroutine = null;
    }

    public void BeginCooldown()
    {
        cooldownCoroutine = Cooldown();
        StartCoroutine(cooldownCoroutine);
    }

    public void EndCooldown()
    {
        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        cooldownCoroutine = null;
    }

    private IEnumerator Recharge()
    {
        for(; ;)
        {
            charge = Mathf.MoveTowards(charge, 100, afterburner.regenRate * Time.deltaTime);

            if (charge >= 100) break;

            yield return null;
        }

        rechargeCoroutine = null;
    }

    private IEnumerator Burn()
    {
        EndRecharge();

        for(; ;)
        {
            charge = Mathf.MoveTowards(charge, 0, afterburner.drainRate * Time.deltaTime);
            if (charge <= 0) break;

            rb.AddForce(rb.transform.forward * afterburner.thrust);
            yield return new WaitForFixedUpdate();
        }

        burnCoroutine = null;

        BeginCooldown();
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(afterburner.cooldownDuration);

        cooldownCoroutine = null;

        BeginRecharge();
    }
}

public class AfterburnerEventArgs : EventArgs
{
    public bool wasActivated;

    public AfterburnerEventArgs(bool wasActivated)
    {
        this.wasActivated = wasActivated;
    }
}