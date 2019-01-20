using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/LookAt")]
public class LookAtDecision : FLDecision
{
    public override void Init()
    {

    }

    public override bool Decide(StateController controller)
    {
        bool targetVisible = LookAt(controller);
        return targetVisible;
    }

    private bool LookAt(StateController controller)
    {
        return false;
    }
}
