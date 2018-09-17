using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Actions/EngageCruise")]
public class EngageCruiseAction : FLAction
{
    public override void Act(StateController controller)
    {
        //if (!(controller.ship.shipEngine.IsChargingCruise || controller.ship.shipEngine.IsCruising))
        //{
        //    EngageCruise(controller);
        //}
    }

    private void EngageCruise(StateController controller)
    {
        //controller.ship.shipEngine.StartChargingCruise(false);
    }
}
