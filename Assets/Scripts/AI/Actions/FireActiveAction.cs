using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/FireActiveAction")]
public class FireActiveAction : FLAction
{
    [Header("Chance")]
    public bool useRange;
    [Range(1, 100)] public int missChance = 5;
    [Range(1, 100)] public int lowerRangeMissChance = 5;
    [Range(1, 100)] public int upperRangeMissChance = 10;

    public int timesToFire = 10;
    public Vector3 missOffset = Vector3.one;

    private int timesFired;

    public override void Init() {}

    public override void Act(StateController controller)
    {
        FireActive(controller);
    }

    private void FireActive(StateController controller)
    {
        if (!controller.HasTarget) return;
        if (controller.DistanceToTarget > controller.weaponsRange) return;

        Vector3 target = controller.ship.aimPosition;
        target += CalculateAimOffset();

        var rb = controller.TargetShip.rb;

        if (rb == null)
            controller.ship.hpSys.FireActiveWeapons(new AimPosition(target));

        else
            controller.ship.hpSys.FireActiveWeapons(new AimPosition(rb));

        //Fire needs to return bool
        //timesFired++;
    }

    private Vector3 CalculateAimOffset()
    {
        var offsetModifier = Random.Range(1,2);

        if (useRange)
        {
            var chance = Random.Range(lowerRangeMissChance, upperRangeMissChance);

            if (Random.Range(1, 100) <= chance)
                return missOffset * offsetModifier;
        }
        else
        {
            if (Random.Range(1,100) <= missChance)
                return missOffset * offsetModifier;
        }

        return Vector3.zero;
    }
}
