using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/RotateTowards")]
public class RotateTowardsAction : FLAction
{
    public override void Act(StateController controller)
    {
        if (controller.targetTrans == null) return;

        Quaternion newRot = Quaternion.LookRotation(controller.targetTrans.position - controller.ship.transform.position);
        controller.ship.transform.rotation = Quaternion.Slerp(controller.ship.transform.rotation, newRot, controller.ship.engineStats.turnSpeed * Time.deltaTime);
    }
}
