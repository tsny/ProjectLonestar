using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFadeout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerController playerController;
    public CanvasGroup canvasGroup;
    public Image image;

    public float fadeDuration = 1;
    public float fadeInAlpha = 1;
    public float fadeOutAlpha = .2f;
    public float mouseExitDelay = .5f;

    public bool fadingIn; 
    public float elapsed = 0;

    public void OnPointerEnter(PointerEventData eventData)
    {
        fadingIn = true;
        elapsed = 0;
        print("pointer enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        fadingIn = false;
        print("pointer exit");
    }

    private void OnMouseEnter() {
        fadingIn = true;
        elapsed = 0;
        print("mouse enter");
    }

    private void OnMouseExit() {
        fadingIn = false;
        print("mouse exit");
    }

    private void Update()
    {
        if (fadingIn && elapsed < fadeDuration)
        {
            elapsed += Time.fixedDeltaTime;
        } 
        else if (!fadingIn && elapsed > 0)
        {
            elapsed -= Time.fixedDeltaTime;  
        }

        //canvasGroup.alpha = elapsed / fadeDuration;
        var temp = image.color;
        temp.a = (elapsed / fadeDuration) * fadeInAlpha;
        image.color = temp;
    }

    public static void DisplayRollingText(Text text, string str)
    {
        StaticCoroutine.StartCoroutine(RollingTextRoutine(text, str));
    }

    private static IEnumerator RollingTextRoutine(Text text, string str)
    {
        text.text = "";

        foreach (char ch in str)
        {
            if (ch == '\\') text.text += "\n";
            else text.text += ch;
            yield return new WaitForFixedUpdate(); 
        }
    }
}
