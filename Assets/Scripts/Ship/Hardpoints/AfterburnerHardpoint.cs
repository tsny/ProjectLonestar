using System.Collections;
using UnityEngine;

public class AfterburnerHardpoint : Hardpoint
{
    public Rigidbody rb;

    public float charge = 100;
    public float regenRate = 1;
    public float thrust = 1;
    public float drain = 1;

    public bool burning;

    public Afterburner Afterburner
    {
        get
        {
            return CurrentEquipment as Afterburner;
        }
    }

    private IEnumerator chargeCoroutine;
    private IEnumerator burnCoroutine;

    public override void Setup(Ship sender)
    {
        base.Setup(sender);
        rb = sender.rb;
    }

    protected override bool EquipmentMatchesHardpoint(Equipment equipment)
    {
        return equipment is Afterburner;
    }

    public void Activate()
    {
        if (OnCooldown || Afterburner == null || burning) return;

        if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
        burnCoroutine = Burn();
        StartCoroutine(burnCoroutine);
    }

    public void Disable()
    {
        if (!burning) return;

        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        chargeCoroutine = Charge();
        StartCoroutine(chargeCoroutine);
    }

    private IEnumerator Charge()
    {
        burning = false;

        for(; ;)
        {
            charge = Mathf.MoveTowards(charge, 100, regenRate * Time.deltaTime);
            if (charge >= 100) break;

            yield return null;
        }
    }

    private IEnumerator Burn()
    {
        burning = true;

        for(; ;)
        {
            charge = Mathf.MoveTowards(charge, 0, drain * Time.deltaTime);
            if (charge <= 0) break;

            rb.AddForce(rb.transform.forward * Afterburner.thrust);
            yield return new WaitForFixedUpdate();
        }

        StartCooldown();
        Disable();
    }
}
