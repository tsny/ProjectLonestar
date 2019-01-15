using System.Collections;
using UnityEngine;

public class Cooldown : ScriptableObject
{
    public bool logElapsed = false;
    public bool destroyOnCompletion = true;
    public bool decrementing = true;
    public float duration = 2;
    public IEnumerator cr;

    public static Cooldown Instantiate(MonoBehaviour caller, float dur, bool logElapsed = false)
    {
        var cd = CreateInstance<Cooldown>();

        cd.duration = dur;
        cd.logElapsed = logElapsed;

        caller.StartCoroutine(cd.CooldownCR());

        return cd;
    }

    private IEnumerator CooldownCR()
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            if (decrementing)
            {
                elapsed += Time.deltaTime;
                if (logElapsed) Debug.Log(elapsed);
            }

            yield return new WaitForEndOfFrame();
        }

        if (destroyOnCompletion) Destroy(this);
    }
}