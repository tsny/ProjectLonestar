using UnityEngine;
using UnityEngine.UI;

public class ProfileTextField : MonoBehaviour 
{
    public Text text;

    private void Awake() 
    {
        text.text = System.Environment.UserName;
    }
}