using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/ThrottleUp")]
public class ThrottleUpAction : FLAction
{
    [Range(0,1)]
    public float threshold = 1;
    public float raycastOffset = 5;
    public float raycastDistance = 25;

    public override void Init() {}

    public override void Act(StateController controller)
    {
        if (controller.ship.engine.Throttle < threshold)
            controller.ship.engine.ThrottleUp();

        //CollisionAvoidance.CheckForObstacle(controller.ship, raycastOffset, raycastDistance);
    }
}
