using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/ToggleAfterburner")]
public class ToggleAfterburner : FLAction
{
    public bool activate = true;

    public override void Init() {}

    public override void Act(StateController controller)
    {
        controller.ship.ToggleAfterburner(activate);
    }
}
