using UnityEngine;
using UnityEngine.UI;

public class HUDManager : ShipUIElement
{
    public Text mouseFlightText;
    public Text cruiseText;

    public MouseState mouseState;
    public EngineState engineState;

    private ShipEngine engine;

    protected override void Awake()
    {
        base.Awake();
        name = "SHIP HUD";
    }

    protected override void HandlePossessed(PlayerController sender, Ship newShip)
    {
        base.HandlePossessed(sender, newShip);

        engine = newShip.shipEngine;
        engine.CruiseChanged += HandleCruiseChange;

        playerController.MouseStateChanged += HandleMouseStateChange;

        SetCruiseText(engine.engineState);
        SetMouseFlightText(playerController.mouseState);

        mouseFlightText.text = "";
    }

    private void HandleMouseStateChange(MouseState state)
    {
        SetMouseFlightText(state);
    }

    protected override void HandleUnpossessed(PlayerController sender, Ship oldShip)
    {
        base.HandleUnpossessed(sender, oldShip);

        engine.CruiseChanged -= HandleCruiseChange;
        engine = null;

        mouseFlightText.text = "";
        cruiseText.text = "";
    }

    private void HandleCruiseChange(ShipEngine sender)
    {
        SetCruiseText(sender.engineState);
    }

    private void SetCruiseText(EngineState engineState)
    {
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

    private void SetMouseFlightText(MouseState mouseState)
    {
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

