using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Reflection;
using System.Linq;

[RequireComponent(typeof(VersionChecker))]
public class BuildVersionUI : ShipUIElement
{
    public string LiveVersion { get; set; }
    public string LocalVersion { get; set; }

    private Text text;
    private VersionChecker versionChecker;

    protected override void Awake()
    {
        base.Awake();
        versionChecker = FindObjectOfType<VersionChecker>();
        versionChecker.UpdateFound += HandleUpdateChecked;
        text = GetComponent<Text>();
    }

    protected void Start()
    {
        text.text = "Fetching build info...";
    }

    private void HandleUpdateChecked(VersionChecker sender)
    {
        SetText();
    }

    private void SetText()
    {
        text.text = "Local version: " + versionChecker.LocalVersion;
        text.text += "\nLive version: " + versionChecker.LiveVersion;
        text.text += "\nLonestar " + DateTime.Today.ToShortDateString();
        text.text += "\nTaylor Snyder";
    }
}