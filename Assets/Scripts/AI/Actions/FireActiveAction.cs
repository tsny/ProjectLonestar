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

    public override void Init() {}

    public override void Act(StateController cont)
    {
        if (!cont.HasTarget) return;
        if (cont.DistanceToTarget > cont.weaponsRange) return;

        Vector3 target = cont.ship.aimPosition;
        target += CalculateAimOffset();

        var rb = cont.TargetShip.rb;
        var fired = false;

        if (rb == null || Utilities.Chance(50)) 
            fired = cont.ship.FireActiveWeapons(new AimPosition(target));
        else 
            fired = cont.ship.FireActiveWeapons(new AimPosition(rb));

        if (fired) cont.timesFired++;
    }

    private Vector3 CalculateAimOffset()
    {
        var offsetModifier = Random.Range(0,5);

        if (useRange)
        {
            var chance = Random.Range(lowerRangeMissChance, upperRangeMissChance);

            if (Utilities.Chance(chance))
                return missOffset * offsetModifier;
        }
        else
        {
            if (Utilities.Chance(missChance))
                return missOffset * offsetModifier;
        }

        return Vector3.zero;
    }
}
