using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/Stopped")]
public class Stopped : FLDecision
{
    public bool includeCruise = true;
    public override bool Decide(StateController controller)
    {
        bool stopped;

        if (includeCruise)
        {
            switch (controller.ship.cruiseEngine.State)
            {
                case CruiseState.Charging:
                case CruiseState.On:
                    return false;
            }
        }

        stopped = controller.ship.engine.Throttle == 0;

        return stopped;
    }
}