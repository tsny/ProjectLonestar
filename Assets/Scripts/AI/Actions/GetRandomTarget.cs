using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Actions/FindRandomTarget")]
public class GetRandomTarget : FLAction
{
    public bool findOnlyShips = true;
    public bool includeAllies;
    // Select only allies?

    public override void Init() {}

    public override void Act(StateController controller)
    {
        var targets = controller.enemies;
        if (targets.Count < 1) return;

        var rnd = new System.Random();
        int r = rnd.Next(targets.Count); 
        controller.Target = targets[r].gameObject;
    }
}