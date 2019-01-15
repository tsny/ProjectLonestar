using UnityEngine;
using System.Collections.Generic;
using System;

public static class InputManager 
{
    // ATM, keycodes must be the EXACT same name as their pref in playerprefs
    #region properties

    public static KeyCode ThrottleUpKey { get; set; }
    public static KeyCode ThrottleDownKey { get; set; }
    public static KeyCode ReverseKey { get; set; }

    public static KeyCode StrafeRightKey { get; set; }
    public static KeyCode StrafeLeftKey { get; set; }

    public static KeyCode ToggleMouseFlightKey { get; set; }
    public static KeyCode ToggleCruiseKey { get; set; }
    public static KeyCode AfterburnerKey { get; set; }
    public static KeyCode KillEnginesKey { get; set; }

    public static KeyCode ManualMouseFlightKey { get; set; }
    public static KeyCode FireKey { get; set; }

    public static KeyCode NanobotsKey { get; set; }
    public static KeyCode ShieldBotsKey { get; set; }

    public static KeyCode Hardpoint1Key { get; set; }
    public static KeyCode Hardpoint2Key { get; set; }
    public static KeyCode Hardpoint3Key { get; set; }
    public static KeyCode Hardpoint4Key { get; set; }
    public static KeyCode Hardpoint5Key { get; set; }
    public static KeyCode Hardpoint6Key { get; set; }
    public static KeyCode Hardpoint7Key { get; set; }
    public static KeyCode Hardpoint8Key { get; set; }
    public static KeyCode Hardpoint9Key { get; set; }
    public static KeyCode Hardpoint10Key { get; set; }

    public static KeyCode LootAllKey { get; set; }

    public static KeyCode FireMineDropperKey { get; set; }
    public static KeyCode FireCountermeasureKey { get; set; }
    public static KeyCode FireMissilesKey { get; set; }

    public static KeyCode NearestEnemyKey { get; set; }
    public static KeyCode NextTargetKey { get; set; }
    public static KeyCode ClearTargetKey { get; set; }
    public static KeyCode PrevTargetKey { get; set; }

    public static KeyCode PauseGameKey { get; set; }

    public static KeyCode RequestDockKey { get; set; }
    public static KeyCode GoToTargetKey { get; set; }
    public static KeyCode JoinFormationKey { get; set; }
    public static KeyCode FreeFlightKey { get; set; }

    public static KeyCode StoryStarKey { get; set; }
    public static KeyCode NavMapKey { get; set; }
    public static KeyCode InventoryKey { get; set; }
    public static KeyCode ReputationKey { get; set; }
    public static KeyCode InformationKey { get; set; }
    public static KeyCode MinimizeHUDKey { get; set; }
    public static KeyCode SwitchCameraKey { get; set; }

    #endregion

    static InputManager()
    {
        SetKeycodes();
    }

    public static void SetKeycodes()
    {
        ThrottleUpKey = GetKeycode("ThrottleUpKey", KeyCode.W.ToString());
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
    }

    public static Dictionary<string, KeyCode> GetKeycodes()
    {
        var keyDictionary = new Dictionary<string, KeyCode>();

        var propInfo = typeof(InputManager).GetProperties();

        foreach (var prop in propInfo)
        {
            if (prop.PropertyType == typeof(KeyCode))
            {
                var currentKey = (KeyCode)prop.GetValue(new object(), null);
                keyDictionary.Add(prop.Name, currentKey);
            }
        }

        return keyDictionary;
    }

    public static KeyCode GetKeycode(string keyName, string defaultName)
    {
        return (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keyName, defaultName));
    }
}
