using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/ThrottleDown")]
public class ThrottleDownAction : FLAction
{
    public float threshold = 0;

    public override void Act(StateController controller)
    {
        if (controller.ship.engine.Throttle > threshold)
            controller.ship.engine.ThrottleDown();
    }
}
