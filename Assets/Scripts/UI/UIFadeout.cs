using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIFadeout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerController playerController;
    public CanvasGroup canvasGroup;
    public Image image;
    public UnityEvent onFinishedFade;

    public float fadeInDuration = 1;
    public float fadeInAlpha = 1;
    public float fadeOutAlpha = .2f;
    public float mouseExitDelay = .5f;

    public bool fadingIn; 
    public float elapsed = 0;

    public void OnPointerEnter(PointerEventData eventData)
    {
        fadingIn = true;
        elapsed = 0;
        enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        fadingIn = false;
        enabled = true;
    }

    private void Update()
    {
        if (fadingIn && elapsed < fadeInDuration)
        {
            elapsed += Time.fixedDeltaTime;
        } 
        else if (!fadingIn && elapsed > 0)
        {
            elapsed -= Time.fixedDeltaTime;  
        }
        else return;

        var temp = image.color;
        temp.a = (elapsed / fadeInDuration) * fadeInAlpha;
        image.color = temp;

        var finishedFadeIn = fadingIn && elapsed > fadeInDuration;
        var finishedFadeOut = !fadingIn && elapsed <= 0;

        if (finishedFadeIn || finishedFadeOut)
        {
            enabled = false;
            onFinishedFade.Invoke();
        }
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
