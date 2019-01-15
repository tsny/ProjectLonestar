using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/FireActiveAction")]
public class FireActiveAction : FLAction
{
    [Range(1, 100)]
    public int missChance = 5;
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

        if (Random.Range(1,100) <= missChance)
            target += missOffset;

        controller.ship.FireActiveWeapons(target);

        //Fire needs to return bool
        //timesFired++;
    }
}
