using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Actions/RotateOverCurve")]
public class RotateOverCurve : FLAction
{
    public FlightAxis axis;
    public AnimationCurve curve;

    public bool pickRandomAxis;

    public float turnAmount = .5f;

    public override void Init() 
    {
       if (pickRandomAxis)
       {
           axis = Utilities.RandomEnumValue<FlightAxis>();
       } 
    }

    public override void Act(StateController controller)
    {
        switch (axis)
        {
            case FlightAxis.Pitch:
                controller.ship.engine.AddPitch(turnAmount);
                break;
                
            case FlightAxis.Roll:
                controller.ship.engine.AddRoll(turnAmount);
                break;

            case FlightAxis.Yaw:
                controller.ship.engine.AddYaw(turnAmount);
                break;
        }
    }
}