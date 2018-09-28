using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BuildVersionUI : MonoBehaviour
{
    private Text text;

    protected void Awake()
    {
        text = GetComponent<Text>();
    }

    protected void Start()
    {
        text.text = "Fetching build info...";
        StartCoroutine(VersionChecker.GetVersions("https://itch.io/api/1/x/wharf/latest?target=tsny/project-lonestar&channel_name=win", this));
    }

    public void SetText(string liveVersion)
    {
        text.text = "Local version: " + Application.version;
        text.text += "\nLive version: " + liveVersion;
        text.text += "\nLonestar " + DateTime.Today.ToShortDateString();
        text.text += "\nTaylor Snyder";
    }
}