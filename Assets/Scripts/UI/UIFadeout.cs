using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayerController playerController;

    public float fadeDuration = 1;
    public float fadeInAlpha = 1;
    public float fadeOutAlpha = .2f;
    public float mouseExitDelay = .5f;

    private CanvasGroup canvasGroup;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeElement(true));

        if (playerController != null)
        {
            playerController.inputAllowed = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(FadeElement(false));

        if (playerController != null)
        {
            playerController.inputAllowed = true;
        }
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
        playerController = FindObjectOfType<PlayerController>();
    }
}
