using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/Stopped")]
public class Stopped : FLDecision
{
    public bool includeCruise = true;

    public override void Init()
    {

    }

    public override bool Decide(StateController controller)
    {
        if (includeCruise)
        {
            switch (controller.ship.cruiseEngine.State)
            {
                case CruiseState.Charging:
                case CruiseState.On:
                    return false;
            }
        }

        return controller.ship.engine.Throttle == 0;
    }
}