using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Actions/EngageCruise")]
public class EngageCruiseAction : FLAction
{
    public override void Act(StateController controller)
    {
        if (!(controller.ship.shipMovement.IsChargingCruise || controller.ship.shipMovement.IsCruising))
        {
            EngageCruise(controller);
        }
    }

    private void EngageCruise(StateController controller)
    {
        controller.ship.shipMovement.StartChargingCruise(false);
    }
}
