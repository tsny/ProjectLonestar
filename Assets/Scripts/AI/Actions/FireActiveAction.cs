using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Actions/FireActiveAction")]
public class FireActiveAction : FLAction
{
    public override void Act(StateController controller)
    {
        FireActive(controller);
    }

    private void FireActive(StateController controller)
    {
        controller.ship.hardpointSystem.FireActiveWeapons();
    }
}
