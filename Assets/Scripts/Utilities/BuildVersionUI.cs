using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Reflection;
using System.Linq;

public class BuildVersionUI : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        text.text = "Lonestar " + DateTime.Today.ToShortDateString();
        text.text += "\nTaylor Snyder";
    }
}
