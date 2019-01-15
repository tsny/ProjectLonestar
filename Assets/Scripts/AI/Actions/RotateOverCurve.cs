using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/RotateOverCurve")]
public class RotateOverCurve : FLAction
{
    public FlightAxis axis;
    public AnimationCurve curve;

    public override void Act(StateController controller)
    {
        switch (axis)
        {
            case FlightAxis.Pitch:
                controller.ship.engine.Pitch(curve.Evaluate(controller.timeInCurrentState));
                break;
                
            case FlightAxis.Roll:
                controller.ship.engine.Roll(curve.Evaluate(controller.timeInCurrentState));
                break;

            case FlightAxis.Yaw:
                controller.ship.engine.Yaw(curve.Evaluate(controller.timeInCurrentState));
                break;

            default:
                return;
        }
    }
}