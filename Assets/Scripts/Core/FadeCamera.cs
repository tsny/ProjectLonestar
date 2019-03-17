using UnityEngine;
using UnityEngine.Events;

public class FadeCamera : MonoBehaviour 
{
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));
    public UnityEvent onFadedIn;
    public UnityEvent onFadedOut;
    public AudioSource musicSource;

    [Range(0,1)] 
    public float alpha = 1;
    public bool destroyOnFinish;
    public bool fadeIn = true;
    public bool updateElapsed = true;

    public bool IsMidTransition { get { return (alpha >= 0 && alpha <= 1); } }

    private Texture2D texture;
    private float elapsed = 0;
    private float origMusicVolume;
 
    private void Awake() 
    {
        if (texture == null) texture = new Texture2D(1, 1);
        if (musicSource != null) origMusicVolume = musicSource.volume;
    }

    void Update()
    {
        if (!updateElapsed) return;

        if (fadeIn && alpha > 0) elapsed += Time.deltaTime;
        else if (!fadeIn && alpha < 1) elapsed -= Time.deltaTime;

        alpha = curve.Evaluate(elapsed);

        if (musicSource != null)
        {
            if (!musicSource.isPlaying) musicSource.Play();
            musicSource.volume = 1 - curve.Evaluate(elapsed) - (1 - origMusicVolume);
        }

        CheckTransitionFinished();
    }
 
    private void OnGUI()
    {
        RefreshTexture();
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }

    private void RefreshTexture()
    {
        texture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
        texture.Apply();
    }

    private void CheckTransitionFinished()
    {
        if (fadeIn && alpha <= 0) 
        {
            onFadedIn.Invoke();
            updateElapsed = false;
        }
        else if (!fadeIn && alpha >= 1)
        {
            onFadedOut.Invoke();
            updateElapsed = false;
        }
    }
}