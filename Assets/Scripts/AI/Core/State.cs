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
            // Use a copy of the scriptable object in order to not update the original scriptable object
            var descCopy = FLDecision.Instantiate(transition.decision);
            // Initialize kind of like a constructor
            descCopy.Init();

            if (descCopy.Decide(controller))
                controller.TransitionToState(transition.trueState);
            else
                controller.TransitionToState(transition.falseState);
        }
    }
}
