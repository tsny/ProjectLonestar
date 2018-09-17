using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Actions/ThrottleDown")]
public class ThrottleDownAction : FLAction
{
    public override void Act(StateController controller)
    {
        //if (controller.ship.shipEngine.throttle > 0)
        //{
        //    ThrottleDown(controller);
        //}
    }

    private void ThrottleDown(StateController controller)
    {
        //controller.ship.shipEngine.ThrottleDown();
    }
}
