using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class UISettingsPanel : MonoBehaviour
{
    public void Toggle() { gameObject.SetActive(!gameObject.activeSelf); }
}
