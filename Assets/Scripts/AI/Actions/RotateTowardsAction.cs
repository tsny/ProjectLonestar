using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/RotateTowards")]
public class RotateTowardsAction : FLAction
{
    public override void Init() {}

    public override void Act(StateController controller)
    {
        if (!controller.HasTarget) return;

    // TOOD: Redo this so not always rotating?
        Quaternion newRot = Quaternion.LookRotation(controller.TargetTransform.position - controller.ship.transform.position);
        controller.ship.transform.rotation = Quaternion.Slerp(controller.ship.transform.rotation, newRot, controller.ship.engineStats.turnSpeed * Time.deltaTime);
    }
}
