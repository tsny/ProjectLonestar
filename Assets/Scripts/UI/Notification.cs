using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour 
{
    // So this thing can probably destroy itself after its animation plays so there shouldn't be a lot of code in here?
    public TextMeshProUGUI text;

    public void Init(string body)
    {
        text.text = body;
    }
}