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
            components = FindObjectsOfType<Ship>();
        }

        else
        {
            components = FindObjectsOfType<MeshRenderer>();
        }

        if (components.Length > 1)
        {
            controller.targetTrans = components[Random.Range(0, components.Length)].transform;
        }

        return components.Length > 1;
    }
}