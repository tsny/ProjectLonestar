using System.Collections;
using UnityEngine;

public class AfterburnerHardpoint : Hardpoint
{
    public Afterburner afterburner;

    public float charge = 100;
    public float regenRate = 1;
    public float thrust = 1;
    public float drain = 1;

    public bool canAfterburn;
    public bool burning;

    private IEnumerator chargeCoroutine;
    private IEnumerator burnCoroutine;

    public AfterburnerHardpoint()
    {
        associatedEquipmentType = typeof(Afterburner);
    }

    protected override void Awake()
    {
        base.Awake();
        hardpointSystem.afterburnerHardpoint = this;
    }

    protected void Start()
    {
        owningShip.shipEngine.CruiseChanged += HandleCruiseChange;
        HandleCruiseChange(owningShip.shipEngine);
    }

    private void HandleCruiseChange(ShipEngine sender)
    {
        canAfterburn = !(sender.engineState == EngineState.Charging || sender.engineState == EngineState.Cruise);
    }

    public override void Mount(Equipment newEquipment)
    {
        base.Mount(newEquipment);

        afterburner = newEquipment as Afterburner;

        regenRate = afterburner.regenRate;
        thrust = afterburner.thrust;
        drain = afterburner.drain;

        charge = 100;
    }

    public void Activate()
    {
        if (OnCooldown || afterburner == null || !canAfterburn || burning) return;

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

            yield return null;
        }

        StartCooldown();
        Disable();
    }
}
