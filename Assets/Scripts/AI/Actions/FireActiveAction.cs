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

    public override void Act(StateController controller)
    {
        FireActive(controller);
    }

    private void FireActive(StateController controller)
    {
        Vector3 target = controller.ship.aimPosition;
        var offsetModifier = Random.Range(1,2);

        if (useRange)
        {
            var chance = Random.Range(lowerRangeMissChance, upperRangeMissChance);

            if (Random.Range(1, 100) <= chance)
                target += missOffset * offsetModifier;
        }
        else
        {
            if (Random.Range(1,100) <= missChance)
                target += missOffset * offsetModifier;
        }

        controller.ship.FireActiveWeapons(target);

        //Fire needs to return bool
        //timesFired++;
    }
}
