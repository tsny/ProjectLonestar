using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    //public PlayerController pc;

    public GameObject pausePanel;
    public GameObject weaponsPanel;
    public GameObject settingsPanel;
    public GameObject backgroundPanel;

    public GameObject notificationSpawn;
    public Notification notificationPF;

    public KeyCode weaponWheelKey;
    public KeyCode settingsKey;

    public CooldownIndicator blinkIndicator;
    public CooldownIndicator sidestepIndicator;

    public Text mouseFlightText;
    public Text cruiseText;

    public List<ShipUIElement> uiElements;

    public AudioMixer sfxMixer;
    public AudioMixer musicMixer;

    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public float slowDownScale = .5f;

    private void Update()
    {
        if (Input.GetKeyDown(weaponWheelKey))
        {
            weaponsPanel.SetActive(true);
            backgroundPanel.SetActive(true);
            Time.timeScale = slowDownScale;
        }
        else if (Input.GetKeyUp(weaponWheelKey))
        {
            weaponsPanel.SetActive(false);
            backgroundPanel.SetActive(false);
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(settingsKey))
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    private void Awake()
    {
        InitResolutionPanel();

        GameStateUtils.GamePaused += HandleGamePaused;
        name = "SHIP HUD";
        GetComponentsInChildren(true, uiElements);

        cruiseText.text = "Engines Nominal";
        mouseFlightText.text = "";
    }

    public void SpawnNotification(string text)
    {
        var noti = Instantiate(notificationPF, notificationSpawn.transform);
        noti.Init(text);
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = GameSettings.Instance.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("volume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("volume", volume);
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
        playerController.PossessedNewShip += HandlePossessedNewShip;
        playerController.MouseStateChanged += HandleMouseStateChange;
        playerController.ship.cruiseEngine.CruiseStateChanged += HandleCruiseChanged;

        GetComponentsInChildren(true, uiElements);

        if (playerController.ship != null)
        {
            uiElements.ForEach(x => x.SetShip(playerController.ship));
            SetCruiseText(playerController.ship.cruiseEngine.State);

            if (blinkIndicator != null)
            {
                blinkIndicator.cd = playerController.ship.engine.blinkCD;
            }

            if (sidestepIndicator != null)
            {
                sidestepIndicator.cd = playerController.ship.engine.sidestepCD;
            }
        }
    }

    private void OnDestroy() 
    {
        if (GameSettings.pc == null) return;

        GameSettings.pc.PossessedNewShip -= HandlePossessedNewShip;
        GameSettings.pc.MouseStateChanged -= HandleMouseStateChange;
        GameSettings.pc.ship.cruiseEngine.CruiseStateChanged -= HandleCruiseChanged;
    }

    private void HandleCruiseChanged(CruiseEngine sender, CruiseState newState)
    {
        SetCruiseText(sender.State);
    }

    private void HandlePossessedNewShip(PlayerController sender, PossessionEventArgs args)
    {
        uiElements.ForEach(x => x.SetShip(sender.ship));
        SetCruiseText(sender.ship.cruiseEngine.State);
    }

    private void HandleGamePaused(bool paused)
    {
        pausePanel.SetActive(paused);
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

    private void InitResolutionPanel()
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(GameSettings.Instance.resolutionOptions);

        resolutionDropdown.onValueChanged.AddListener
        ( 
            delegate
            {
                //var res = StringToResolution(resolutionDropdown.options[resolutionDropdown.value].text);
                var res = GameSettings.Instance.resolutions[resolutionDropdown.value];
                // Check to see if the fullscreen mode setting is correct here
                Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRate);
                resolutionDropdown.RefreshShownValue();
            }
        );
        //resolutionDropdown.value = currentResolutionIndex;
    }

    // Remove this if it doesn't end up getting used
    public static Resolution StringToResolution(string input)
    {
        var res = new Resolution();

        return res;
    }
}

