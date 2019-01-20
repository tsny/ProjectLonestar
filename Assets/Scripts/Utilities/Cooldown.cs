using System.Collections;
using UnityEngine;

public class Cooldown : ScriptableObject
{
    public bool logElapsed = false;
    public bool isDecrementing = false;
    public float duration = 2;
    public float elapsed = 0;

    public MonoBehaviour owner;
    public IEnumerator cr;

    public void Begin(MonoBehaviour caller)
    {
        owner = caller;
        caller.StartCoroutine(CooldownCR());
    }

    public void Stop()
    {
        owner.StopCoroutine(cr);
        cr = null;
        isDecrementing = false;
    }

    private IEnumerator CooldownCR()
    {
        elapsed = 0;
        isDecrementing = true;

        while (elapsed < duration)
        {
            if (isDecrementing)
            {
                elapsed += Time.deltaTime;
                if (logElapsed) Debug.Log(elapsed);
            }

            yield return new WaitForEndOfFrame();
        }

        elapsed = 0;
        cr = null;
        isDecrementing = false;
    }
}