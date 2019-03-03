using System.Collections;
using UnityEngine;

[System.Serializable]
public class Cooldown
{
    public bool logElapsed = false;
    public bool IsDecrementing { get { return cr != null; } }
    public float duration = 2;
    public float elapsed = 0;

    public MonoBehaviour owner;
    public IEnumerator cr;

    public void Begin(MonoBehaviour caller)
    {
        if (IsDecrementing)
        {
            elapsed = 0;
            return;
        } 

        owner = caller;
        cr = CooldownCR();
        caller.StartCoroutine(cr);
    }

    public void Stop()
    {
        if (!IsDecrementing) return;
        owner.StopCoroutine(cr);
        cr = null;
    }

    private IEnumerator CooldownCR()
    {
        elapsed = 0;

        while (elapsed < duration)
        {
            if (IsDecrementing)
            {
                elapsed += Time.deltaTime;
                if (logElapsed) Debug.Log(elapsed);
            }

            yield return new WaitForEndOfFrame();
        }

        elapsed = 0;
        cr = null;
    }
}