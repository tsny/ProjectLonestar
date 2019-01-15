using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public PlayerController playerController;

    public GameObject pause;
    public GameObject weapons;
    public GameObject settings;
    public GameObject bg;

    public KeyCode weaponWheelKey;
    public KeyCode settingsKey;

    public Text mouseFlightText;
    public Text cruiseText;

    public List<ShipUIElement> uiElements;
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;

    public float slowDownScale = .5f;
    Resolution[] resolutions;

    private void Update()
    {
        if (Input.GetKeyDown(weaponWheelKey))
        {
            weapons.SetActive(true);
            bg.SetActive(true);
            Time.timeScale = slowDownScale;
        }
        else if (Input.GetKeyUp(weaponWheelKey))
        {
            weapons.SetActive(false);
            bg.SetActive(false);
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(settingsKey))
        {
            settings.SetActive(!settings.activeSelf);
        }
    }

    private void Awake()
    {
        InitResolutions();

        GameStateUtils.GamePaused += HandleGamePaused;
        name = "SHIP HUD";
        GetComponentsInChildren(true, uiElements);

        cruiseText.text = "Engines Nominal";
        mouseFlightText.text = "";
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void QuitToMainmenu()
    {
        SceneManager.LoadScene(GameSettings.Instance.menuSceneName);
    }

    public void SetPlayerController(PlayerController playerController)
    {
        if (this.playerController == playerController) return;

        if (this.playerController != null)
        {
            this.playerController.PossessedNewShip -= HandlePossessedNewShip;
            this.playerController.MouseStateChanged -= HandleMouseStateChange;
        }

        this.playerController = playerController;

        playerController.PossessedNewShip += HandlePossessedNewShip;
        playerController.MouseStateChanged += HandleMouseStateChange;
        playerController.ship.cruiseEngine.CruiseStateChanged += HandleCruiseChanged;

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
        pause.SetActive(paused);
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

    private void InitResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " : " + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (resolutions[i].Equals(Screen.currentResolution))
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}

