using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/AimAtTarget")]
public class AimAtTarget : FLAction
{
    public override void Init() {}

    public override void Act(StateController controller)
    {
        if (controller.targetTrans == null) return;

        controller.ship.aimPosition = controller.targetTrans.position;
    }
}
