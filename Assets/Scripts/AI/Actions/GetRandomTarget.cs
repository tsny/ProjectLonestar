using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Actions/FindRandomTarget")]
public class GetRandomTarget : FLAction
{
    public override void Init() {}

    public override void Act(StateController controller)
    {
        controller.TargetRandomEnemy();
    }
}