using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/GotoNextWaypoint")]
public class GotoNextWaypointAction : FLAction
{
    public override void Init() {}

    public override void Act(StateController controller)
    {
        GotoNextWaypoint(controller);
    }

    private void GotoNextWaypoint(StateController controller)
    {

    }
}
