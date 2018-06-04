using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PluggableAI/Actions/GotoNextWaypoint")]
public class GotoNextWaypointAction : FLAction
{
    public override void Act(StateController controller)
    {
        GotoNextWaypoint(controller);
    }

    private void GotoNextWaypoint(StateController controller)
    {

    }
}
