using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject pauseMenu;
    public Text mouseFlightText;
    public Text cruiseText;

    public List<ShipUIElement> uiElements;

    private void Awake()
    {
        GameStateUtils.GamePaused += HandleGamePaused;
        name = "SHIP HUD";
        GetComponentsInChildren(true, uiElements);

        cruiseText.text = "";
        mouseFlightText.text = "";
    }

    public void InitializeHUD(PlayerController playerController)
    {
        this.playerController = playerController;
        uiElements.ForEach(x => x.SetShip(playerController.controlledShip));

        playerController.Possession += HandlePossession;
        playerController.MouseStateChanged += HandleMouseStateChange;
        playerController.controlledShip.cruiseEngine.CruiseStateChanged += HandleCruiseChanged;

        if (playerController.controlledShip != null)
        {
            SetCruiseText(playerController.controlledShip.cruiseEngine.State);
        }
    }

    private void HandleCruiseChanged(CruiseEngine sender)
    {
        SetCruiseText(sender.State);
    }

    private void HandlePossession(PossessionEventArgs args)
    {
        uiElements.ForEach(x => x.SetShip(playerController.controlledShip));
    }

    private void HandleGamePaused(bool paused)
    {
        pauseMenu.SetActive(paused);
    }

    private void HandleMouseStateChange(MouseState state)
    {
        SetMouseFlightText(state);
    }

    private void SetCruiseText(CruiseEngine.CruiseState cruiseState)
    {
        switch (cruiseState)
        {
            case CruiseEngine.CruiseState.Off:
                cruiseText.text = "Engines Nominal";
                break;

            case CruiseEngine.CruiseState.Charging:
                cruiseText.text = "Charging Cruise...";
                break;

            case CruiseEngine.CruiseState.On:
                cruiseText.text = "Cruising";
                break;

            case CruiseEngine.CruiseState.Disrupted:
                cruiseText.text = "Disrupted";
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

