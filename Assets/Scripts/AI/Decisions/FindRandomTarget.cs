using UnityEngine;

[CreateAssetMenu(fileName = "FindRandomTarget", menuName = "AI/Decisions/FindRandomTarget", order = 0)]
public class FindRandomTarget : FLDecision
{
    public bool onlyShips;

    public override void Init()
    {

    }

    public override bool Decide(StateController controller)
    {
        Component[] components;

        if (onlyShips)
        {
            // TODO: use the game manager's current list of alive ships?
            components = FindObjectsOfType<Ship>();
        }

        else
        {
            components = FindObjectsOfType<MeshRenderer>();
        }

        if (components.Length > 1)
        {
            controller.Target = components[Random.Range(0, components.Length)].gameObject;
        }

        return controller.HasTarget;
    }
}