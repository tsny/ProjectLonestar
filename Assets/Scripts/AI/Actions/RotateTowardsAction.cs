using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/RotateTowards")]
public class RotateTowardsAction : FLAction
{
    public override void Init() {}

    public override void Act(StateController cont)
    {
        if (!cont.HasTarget) return;
        var ship = cont.ship;

    // TOOD: Redo this so not always rotating?
        Quaternion newRot = Quaternion.LookRotation(cont.TargetTransform.position - ship.transform.position);
        cont.ship.transform.rotation = Quaternion.Slerp(ship.transform.rotation, newRot, ship.engine.engineStats.turnSpeed * Time.deltaTime);
    }
}
