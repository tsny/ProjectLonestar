using System;
using System.Collections;
using UnityEngine;

public class Afterburner : Hardpoint
{
    public Rigidbody rb;
    public AfterburnerStats stats;
    public float charge;

    public bool IsBurning { get { return burnCoroutine != null; } }

    private IEnumerator rechargeCoroutine;
    private IEnumerator burnCoroutine;
    private IEnumerator cooldownCoroutine;

    public event ToggledEventHandler Toggled;
    public delegate void ToggledEventHandler(bool toggle);

    private void Awake()
    {
        stats = Utilities.CheckScriptableObject<AfterburnerStats>(stats);
        if (rb == null) rb = GetComponentInChildren<Rigidbody>();
        charge = stats.capacity;
    }

    private void OnActivated() { if (Toggled != null) Toggled(true); }
    private void OnDeactivated() { if (Toggled != null) Toggled(false); }

    public override void Initialize(Ship sender)
    {
        base.Initialize(sender);
        rb = sender.rb;
        stats = Utilities.CheckScriptableObject<AfterburnerStats>(stats);
    }

    public void Activate()
    {
        if (rb == null || IsBurning) return;

        burnCoroutine = Burn();
        StartCoroutine(burnCoroutine);

        OnActivated();
    }

    public void Deactivate()
    {
        if (!IsBurning) return;

        if (burnCoroutine != null) StopCoroutine(burnCoroutine);

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

    private IEnumerator Recharge()
    {
        for(; ;)
        {
            charge = Mathf.MoveTowards(charge, 100, stats.regenRate * Time.deltaTime);

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
            charge = Mathf.MoveTowards(charge, 0, stats.drainRate * Time.deltaTime);
            if (charge <= 0) break;

            //rb.AddForce(rb.transform.forward * afterburner.thrust);
            yield return new WaitForFixedUpdate();
        }

        burnCoroutine = null;

        BeginCooldown();
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(stats.cooldownDuration);
        cooldownCoroutine = null;
        BeginRecharge();
    }
}