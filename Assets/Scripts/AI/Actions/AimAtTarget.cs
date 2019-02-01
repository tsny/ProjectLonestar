using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/AimAtTarget")]
public class AimAtTarget : FLAction
{
    public override void Init() {}

    public override void Act(StateController cont)
    {
        if (!cont.HasTarget) return;

        cont.ship.aimPosition = cont.TargetTransform.position;
    }
}
