using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/AimAtTarget")]
public class AimAtTarget : FLAction
{
    public override void Init() {}

    public override void Act(StateController controller)
    {
        if (!controller.HasTarget) return;

        controller.ship.aimPosition = controller.TargetTransform.position;
    }
}
