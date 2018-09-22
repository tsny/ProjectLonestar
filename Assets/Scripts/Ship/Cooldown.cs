using UnityEngine;
using System.Collections;

public class CooldownUtil 
{
    public static IEnumerator Cooldown(float cooldownDuration, IEnumerator coroutineRef)
    {
        yield return new WaitForSeconds(cooldownDuration);

        coroutineRef = null;
    }
}
