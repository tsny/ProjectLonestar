using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/FireActiveAction")]
public class FullStop : FLAction
{
    public override void Init() {}

    public override void Act(StateController controller)
    {
        controller.ship.engine.Throttle = 0;
        controller.ship.engine.Strafe = 0;
        controller.ship.cruiseEngine.StopAnyCruise();
    }
}