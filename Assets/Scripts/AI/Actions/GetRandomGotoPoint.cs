using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Actions/GetRandomGotoPoint")]
public class GetRandomGotoPoint : FLAction
{
    public override void Init() {}

    public override void Act(StateController controller)
    {
        controller.Target = AIManager.Instance.GetRandomGotoPoint();
    }
}