using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup canvasGroup;

    public float fadeDuration = 1;
    public float fadeInAlpha = 1;
    public float fadeOutAlpha = .2f;
    public float mouseExitDelay = .5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeElement(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeElement(false));
    }

    IEnumerator FadeElement(bool fadeIn)
    {
        float desiredAlpha = fadeIn ? fadeInAlpha : fadeOutAlpha;
        float alpha = canvasGroup.alpha;

        for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, desiredAlpha, t);
            yield return null;
        }
    }

    // Use this for initialization
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeElement(false));
    }
}
