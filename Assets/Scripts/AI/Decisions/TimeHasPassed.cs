using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/TimeHasPassed")]
public class TimeHasPassed : FLDecision
{
    public float duration = 3;

    public override bool Decide(StateController controller)
    {
        return controller.timeInCurrentState > duration;
    }
}