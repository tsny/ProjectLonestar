using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/State")]
public class State : ScriptableObject
{
    public Color sceneGizmoColor = Color.gray;

    public FLAction[] actions;
    public Transition[] transitions;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(StateController controller)
    {
        foreach (Transition transition in transitions)
        {
            if (transition.decision.Decide(controller))
                controller.TransitionToState(transition.trueState);
            else
                controller.TransitionToState(transition.falseState);
        }
    }
}
