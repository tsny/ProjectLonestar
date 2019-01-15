using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class BindingsEditor : MonoBehaviour
{
    public GameObject keyBindingButtonPrefab;
    public VerticalLayoutGroup vlg;

    private void Awake() 
    {

    }

    private void Start()
    {
        // var keycodeDict = InputManager.SetKeycodes();

        // foreach (var entry in keycodeDict)
        // {
        //     var text = Instantiate(keyBindingButtonPrefab, vlg.transform).GetComponentInChildren<Text>();
        //     text.text = entry.Key + " : " + entry.Value;
        // }
    }
}