using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/TimeHasPassed")]
public class TimeHasPassed : FLDecision
{
    public bool useRange;

    public float duration = 3;

    public float lowerDuration = 3;
    public float upperDuration = 5;

    public override void Init()
    {
        if (useRange)
        {
            duration = Random.Range(lowerDuration, upperDuration);
        }
    }

    public override bool Decide(StateController controller)
    {
        return controller.timeInCurrentState > duration;
    }
}