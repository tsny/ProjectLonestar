using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Actions/GetTargetPointAction")]
public class GetTargetPointAction : FLAction
{
    public override void Act(StateController controller)
    {
        GetTargetPoint(controller);
    }

    private void GetTargetPoint(StateController controller)
    {
        controller.ship.aimPosition = controller.targetShip.transform.position;
    }
}
