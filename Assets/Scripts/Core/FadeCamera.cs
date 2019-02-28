using UnityEngine;

public class FadeCamera : MonoBehaviour 
{
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));

    [Range(0,1)] 
    public float alpha = 1;

    public bool destroyOnFinish;
    public bool fadeIn = true;
    public bool updateElapsed = true;

    public bool IsTransitioning { get { return (alpha <= 0 || alpha >= 1); } }

    private Texture2D texture;
    private float elapsed;
 
    [ContextMenu("Default")]
    private void Default()
    {
        alpha = 1;
        elapsed = 0;
        fadeIn = true;
    }
 
    public void OnGUI()
    {
        if (texture == null) texture = new Texture2D(1, 1);
 
        texture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
        texture.Apply();

        if (updateElapsed) 
        {
            if (fadeIn && alpha > 0) elapsed += Time.deltaTime;
            else if (!fadeIn && alpha < 1) elapsed -= Time.deltaTime;
        }

        alpha = curve.Evaluate(elapsed);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);

        if (!IsTransitioning && destroyOnFinish) Destroy(this);
    }
}