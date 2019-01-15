using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/EngageCruise")]
public class EngageCruiseAction : FLAction
{
    public override void Act(StateController controller)
    {
        controller.ship.cruiseEngine.TryChargeCruise();
    }
}
