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

        cruiseText.text = "Engines Nominal";
        mouseFlightText.text = "";
    }

    public void SetPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;

        playerController.PossessedNewShip += HandlePossessedNewShip;
        playerController.MouseStateChanged += HandleMouseStateChange;
        //playerController.ship.cruiseEngine.CruiseStateChanged += HandleCruiseChanged;

        if (playerController.ship != null)
        {
            uiElements.ForEach(x => x.SetShip(playerController.ship));
            SetCruiseText(playerController.ship.cruiseEngine.State);
        }
    }

    private void HandleCruiseChanged(CruiseEngine sender, CruiseState newState)
    {
        SetCruiseText(sender.State);
    }

    private void HandlePossessedNewShip(PlayerController sender, PossessionEventArgs args)
    {
        uiElements.ForEach(x => x.SetShip(playerController.ship));
    }

    private void HandleGamePaused(bool paused)
    {
        pauseMenu.SetActive(paused);
    }

    private void HandleMouseStateChange(MouseState state)
    {
        SetMouseFlightText(state);
    }

    private void SetCruiseText(CruiseState cruiseState)
    {
        switch (cruiseState)
        {
            case CruiseState.Off:
                cruiseText.text = "Engines Nominal";
                break;

            case CruiseState.Charging:
                cruiseText.text = "Charging Cruise...";
                break;

            case CruiseState.On:
                cruiseText.text = "Cruising";
                break;

            case CruiseState.Disrupted:
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

