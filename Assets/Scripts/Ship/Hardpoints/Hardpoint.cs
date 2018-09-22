using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Hardpoint : ShipComponent
{
    public bool IsOnCooldown
    {
        get
        {
            return cooldownCoroutine != null;
        }
    }

    private IEnumerator cooldownCoroutine;

    public virtual void StartCooldown(float duration)
    {
        cooldownCoroutine = Cooldown(duration);
        StartCoroutine(cooldownCoroutine);
    }

    public virtual void EndCooldown()
    {
        StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = null;
    }

    protected IEnumerator Cooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        cooldownCoroutine = null;
    }
}


