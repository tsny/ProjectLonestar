using UnityEngine;
using UnityEngine.UI;

public class HUDManager : ShipUIElement
{
    public Text mouseFlightText;
    public Text cruiseText;

    public MouseState mouseState;
    public EngineState engineState;

    private void Start()
    {
        mouseFlightText.text = "";
        cruiseText.text = "";
        cruiseText.text = "Engines Nominal";
    }

    protected override void HandlePossession(PlayerController sender, PossessionEventArgs e)
    {
        base.HandlePossession(sender, e);
    }

    private void Update()
    {
        if (playerController == null) return;

        SetMouseFlightText();
        SetCruiseText();
    }

    private void SetCruiseText()
    {
        if (playerController.controlledShip.shipMovement.engineState != engineState)
        {
            engineState = playerController.controlledShip.shipMovement.engineState;

            switch (engineState)
            {
                case EngineState.Normal:
                    cruiseText.text = "Engines Nominal";
                    break;
                case EngineState.Charging:
                    cruiseText.text = "Charging Cruise...";
                    break;
                case EngineState.Cruise:
                    cruiseText.text = "Cruising";
                    break;
                case EngineState.Drifting:
                    cruiseText.text = "Drifting";
                    break;
                case EngineState.Reversing:
                    cruiseText.text = "Reversing";
                    break;
            }
        }
    }

    private void SetMouseFlightText()
    {
        if (playerController.mouseState != mouseState)
        {
            mouseState = playerController.mouseState;

            switch (mouseState)
            {
                case MouseState.Held:
                    mouseFlightText.text = "MANUAL MOUSE FLIGHT";
                    break;

                case MouseState.Off:
                    mouseFlightText.text = "";
                    break;

                case MouseState.Toggled:
                    mouseFlightText.text = "MOUSE FLIGHT TOGGLED";
                    break;
            }
        }
    }
}
