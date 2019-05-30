﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject weaponsPanel;
    public GameObject settingsPanel;
    public GameObject backgroundPanel;
    public TextMeshProUGUI objectiveText;
    public Minimap minimap;
    public IndicatorManager indicatorManager;

    public GameObject notificationSpawn;
    public Notification notificationPF;
    public AudioClip pickupClip;
    public AudioSource audioSource;

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

        if (FindObjectsOfType<HUDManager>().Length > 1)
        {
            Debug.LogWarning("Tried to spawn HUDManager with one already in scene...");
            Destroy(gameObject);
            return;
        }

        GameStateUtils.GamePaused += HandleGamePaused;
        Loot.Looted += HandleDropLooted;

        name = "SHIP HUD";
        GetComponentsInChildren(true, uiElements);

        cruiseText.text = "Engines Nominal";
        mouseFlightText.text = "";
    }

    private void HandleDropLooted(Loot loot)
    {
        SpawnNotification("Looted " + loot.item.name);
        audioSource.PlayOneShot(pickupClip);
    }

    private void Start()
    {
        SpawnNotification("HUD initializing...");
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

    public void SetSFXVolume(float volume) { sfxMixer.SetFloat("volume", volume); }
    public void SetMusicVolume(float volume) { musicMixer.SetFloat("volume", volume); }
    public void SetQuality(int qualityIndex) { QualitySettings.SetQualityLevel(qualityIndex); }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void QuitToMainmenu()
    {
        SceneManager.LoadScene(GameSettings.Instance.menuSceneName);
    }

    public void SetPlayerController(PlayerController pc)
    {
        pc.PossessedNewShip += HandlePossessedNewShip;
        pc.ReleasedShip += HandleReleasedShip;
        pc.MouseStateChanged += HandleMouseStateChange;
        GetComponentsInChildren(true, uiElements);
        indicatorManager.cam = pc.cam;
    }

    private void HandleReleasedShip(PlayerController sender, PossessionEventArgs args)
    {
        sender.ship.cruiseEngine.CruiseStateChanged -= HandleCruiseChanged;
    }

    private void HandleCruiseChanged(CruiseEngine sender, CruiseState newState)
    {
        SetCruiseText(sender.State);
    }

    private void HandlePossessedNewShip(PlayerController sender, PossessionEventArgs args)
    {
        uiElements.ForEach(x => x.Init(args.newShip));
        SetCruiseText(args.newShip.cruiseEngine.State);
        args.newShip.cruiseEngine.CruiseStateChanged += HandleCruiseChanged;
        if (minimap != null) minimap.transformToMirror = args.newShip.transform;
        if (blinkIndicator != null) blinkIndicator.cd = args.newShip.engine.blinkCD;
        if (sidestepIndicator != null) sidestepIndicator.cd = args.newShip.engine.sidestepCD;
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
                SpawnNotification(cruiseText.text);
                break;

            case CruiseState.On:
                cruiseText.text = "Cruising";
                SpawnNotification(cruiseText.text);
                break;

            case CruiseState.Disrupted:
                cruiseText.text = "Disrupted";
                SpawnNotification(cruiseText.text);
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

