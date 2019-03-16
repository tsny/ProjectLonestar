using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BuildVersionUI : MonoBehaviour
{
    public Text text;

    protected void Start()
    {
        if (text == null)
        {
            Debug.LogError("BuildVersion UI Piece could not be found!");
            return;
        }

        text.text = "Fetching build info...";
        var url = "https://itch.io/api/1/x/wharf/latest?target=tsny/project-lonestar&channel_name=win";
        StartCoroutine(VersionChecker.GetVersions(url, this));
    }

    public void SetText(string liveVersion, bool shortDesc = true)
    {
        text.text = "Local version: " + Application.version;
        if (!shortDesc)
        {
            text.text += "\nLive version: " + liveVersion;
            text.text += "\nLonestar " + DateTime.Today.ToShortDateString();
            text.text += "\ntsny";
        }
    }
}