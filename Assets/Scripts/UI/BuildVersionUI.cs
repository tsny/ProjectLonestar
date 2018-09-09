using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Reflection;
using System.Linq;

public class BuildVersionUI : ShipUIElement
{
    private Text text;
    private VersionChecker versionChecker;

    protected override void Awake()
    {
        base.Awake();
        versionChecker = FindObjectOfType<VersionChecker>();
        versionChecker.UpdateFound += HandleUpdateChecked;
        text = GetComponent<Text>();
    }

    private void HandleUpdateChecked(VersionChecker sender)
    {
        SetText();
    }

    protected void Start()
    {
        text.text = "Fetching build info...";
    }

    private void SetText()
    {
        text.text = "Local version: " + versionChecker.LocalVersion;
        text.text += "\nCloud version: " + versionChecker.CloudVersion;
        text.text += "\nLonestar " + DateTime.Today.ToShortDateString();
        text.text += "\nTaylor Snyder";
    }
}
