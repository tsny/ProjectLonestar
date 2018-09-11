using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class BindingsEditor : MonoBehaviour
{
    public GameObject keyBindingButtonPrefab;
    public VerticalLayoutGroup vlg;

    private void Start()
    {
        var inputManager = FindObjectOfType<InputManager>();
        foreach (var key in inputManager.keyDictionary.Keys)
        {
            KeyCode currKeyCode;
            bool success = inputManager.keyDictionary.TryGetValue(key, out currKeyCode);
            if (success)
            {
                var text = Instantiate(keyBindingButtonPrefab, vlg.transform).GetComponentInChildren<Text>();
                text.text = key + " : " + currKeyCode;
            }
        }
    }
}