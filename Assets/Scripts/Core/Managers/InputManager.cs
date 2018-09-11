using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public MouseState mouseState;

    // ATM, keycodes must be the EXACT same name as their pref in playerprefs
    #region properties

    public KeyCode ThrottleUpKey { get; set; }
    public KeyCode ThrottleDownKey { get; set; }
    public KeyCode ReverseKey { get; set; }

    public KeyCode StrafeRightKey { get; set; }
    public KeyCode StrafeLeftKey { get; set; }

    public KeyCode ToggleMouseFlightKey { get; set; }
    public KeyCode ToggleCruiseKey { get; set; }
    public KeyCode AfterburnerKey { get; set; }
    public KeyCode KillEnginesKey { get; set; }

    public KeyCode ManualMouseFlightKey { get; set; }
    public KeyCode FireKey { get; set; }

    public KeyCode NanobotsKey { get; set; }
    public KeyCode ShieldBotsKey { get; set; }

    public KeyCode Hardpoint1Key { get; set; }
    public KeyCode Hardpoint2Key { get; set; }
    public KeyCode Hardpoint3Key { get; set; }
    public KeyCode Hardpoint4Key { get; set; }
    public KeyCode Hardpoint5Key { get; set; }
    public KeyCode Hardpoint6Key { get; set; }
    public KeyCode Hardpoint7Key { get; set; }
    public KeyCode Hardpoint8Key { get; set; }
    public KeyCode Hardpoint9Key { get; set; }
    public KeyCode Hardpoint10Key { get; set; }

    public KeyCode LootAllKey { get; set; }

    public KeyCode FireMineDropperKey { get; set; }
    public KeyCode FireCountermeasureKey { get; set; }
    public KeyCode FireMissilesKey { get; set; }

    public KeyCode NearestEnemyKey { get; set; }
    public KeyCode NextTargetKey { get; set; }
    public KeyCode ClearTargetKey { get; set; }
    public KeyCode PrevTargetKey { get; set; }

    public KeyCode PauseGameKey { get; set; }

    public KeyCode RequestDockKey { get; set; }
    public KeyCode GoToTargetKey { get; set; }
    public KeyCode JoinFormationKey { get; set; }
    public KeyCode FreeFlightKey { get; set; }

    public KeyCode StoryStarKey { get; set; }
    public KeyCode NavMapKey { get; set; }
    public KeyCode InventoryKey { get; set; }
    public KeyCode ReputationKey { get; set; }
    public KeyCode InformationKey { get; set; }
    public KeyCode MinimizeHUDKey { get; set; }
    public KeyCode SwitchCameraKey { get; set; }

    #endregion

    public Dictionary<string, KeyCode> keyDictionary;

    private void SingletonInit()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        else if (instance != this) Destroy(gameObject);
    }

    private void Awake()
    {
        SingletonInit();
        GetKeycodesFromPrefs();
        name = "INPUT MANAGER";
    }

    private void GetKeycodesFromPrefs()
    {
        keyDictionary = new Dictionary<string, KeyCode>();

        ThrottleUpKey = GetKeycode("ThrottleUpKey", "W");
        ThrottleDownKey = GetKeycode("ThrottleDown", "S");
        StrafeRightKey = GetKeycode("StrafeRightKey", "D");
        StrafeLeftKey = GetKeycode("StrafeLeftKey", "A");
        ReverseKey = GetKeycode("ReverseKey", "X");
        AfterburnerKey = GetKeycode("AfterburnerKey", "Tab");
        KillEnginesKey = GetKeycode("KillEnginesKey", "Z");
        ToggleMouseFlightKey = GetKeycode("ToggleMouseFlightKey", "Space");

        ManualMouseFlightKey = GetKeycode("ManualMouseFlightKey", "Mouse0");
        FireKey = GetKeycode("FireKey", "Mouse1");

        NanobotsKey = GetKeycode("NanobotsKey", "G");
        ShieldBotsKey = GetKeycode("ShieldbotsKey", "F");

        Hardpoint1Key = GetKeycode("Hardpoint1Key", "Alpha1");
        Hardpoint2Key = GetKeycode("Hardpoint2Key", "Alpha2");
        Hardpoint3Key = GetKeycode("Hardpoint3Key", "Alpha3");
        Hardpoint4Key = GetKeycode("Hardpoint4Key", "Alpha4");
        Hardpoint5Key = GetKeycode("Hardpoint5Key", "Alpha5");
        Hardpoint6Key = GetKeycode("Hardpoint6Key", "Alpha6");
        Hardpoint7Key = GetKeycode("Hardpoint7Key", "Alpha7");
        Hardpoint8Key = GetKeycode("Hardpoint8Key", "Alpha8");
        Hardpoint9Key = GetKeycode("Hardpoint9Key", "Alpha9");
        Hardpoint10Key = GetKeycode("Hardpoint10Key", "Alpha0");

        LootAllKey = GetKeycode("LootAllKey", "B");

        FireMineDropperKey = GetKeycode("FireMineDropperKey", "E");
        FireMissilesKey = GetKeycode("FireMissilesKey", "Q");
        FireCountermeasureKey = GetKeycode("FireCountermeasureKey", "C");

        PauseGameKey = GetKeycode("PauseGameKey", "F1");
        RequestDockKey = GetKeycode("RequestDockKey", "F3");
        GoToTargetKey = GetKeycode("GoToTargetKey", "F2");
        JoinFormationKey = GetKeycode("JoinFormationKey", "F4");

        var propInfo = typeof(InputManager).GetProperties();

        foreach (var prop in propInfo)
        {
            if (prop.PropertyType == typeof(KeyCode))
            {
                var currentKey = (KeyCode)prop.GetValue(this, null);
                keyDictionary.Add(prop.Name, currentKey);
                //print(prop.Name + " : " + currentKey);
            }
        }
    }

    public static KeyCode GetKeycode(string keyName, string defaultName)
    {
        return (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyName, defaultName));
    }
}
