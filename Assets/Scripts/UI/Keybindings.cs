using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Keybindings : MonoBehaviour 
{
    public GameObject panel;
    public VerticalLayoutGroup buttonBlg;
    public VerticalLayoutGroup keyVlg;
    public Text keyText;
    public Button changeKeyButton;

    //public IEnumerator waitingForKey;
    private bool waitingForKey; 

    void Update()
    {
        if (waitingForKey)
        {
            // Display "Waiting for key text"

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                waitingForKey = false;
                return;
            }

            if (Input.anyKeyDown)
            {
                KeyCode newKeyCode = GetKeycodeInFrame();
                //PlayerPrefs.SetString()
                waitingForKey = false;
            }
        }
    }

    private void Awake()
    {
        var dict = InputManager.GetKeycodes();

        foreach (var key in dict.Keys)
        {
            var text = Instantiate(keyText, keyVlg.transform);
            var btn = Instantiate(changeKeyButton, buttonBlg.transform);

            // finish this
            //btn.onClick.AddListener() => {}
            // Use the current key as some kind of param in the lambda
        }
    }

    public bool ChangeCommand(string command, KeyCode newKeyCode)
    {

        return true;
    }

    // TODO
    public static KeyCode GetKeycodeInFrame()
    {
        return InputManager.GetKeycode(Input.inputString, null);
    }
}