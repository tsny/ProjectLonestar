using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Reflection;
using System.Linq;

// This probably does not have to be a ShipUIElement

[RequireComponent(typeof(VersionChecker))]
public class BuildVersionUI : MonoBehaviour
{
    private Text text;
    private VersionChecker versionChecker;

    protected void Awake()
    {
        text = GetComponent<Text>();
        StartCoroutine(FindVersionChecker());
    }

    private IEnumerator FindVersionChecker()
    {
        for (; ;)
        {
            versionChecker = FindObjectOfType<VersionChecker>();

            if (versionChecker == null) yield return null;

            else break;
        }

        versionChecker.UpdateFound += HandleUpdateChecked;
        if (versionChecker.VersionChecked) SetText();
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