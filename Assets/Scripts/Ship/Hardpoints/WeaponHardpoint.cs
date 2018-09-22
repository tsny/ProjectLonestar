using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WeaponHardpoint : MonoBehaviour
{
    public AudioSource audioSource;
    public Vector3 aimPosition;

    public Projectile projectilePrefab;

    private bool _active = true;
    public bool Active
    {
        get
        {
            return _active;
        }
        set
        {
            _active = value;
            if (value)
            {
                OnActivated();
            }
            else
            {
                OnDeactivated();
            }
        }
    }

    public bool IsOnCooldown
    {
        get
        {
            return cooldownCoroutine != null;
        }
    }

    private IEnumerator cooldownCoroutine;

    public event EventHandler<EventArgs> Fired;
    public event EventHandler<EventArgs> Activated;
    public event EventHandler<EventArgs> Deactivated;

    private void OnFired()
    {
        if (Fired != null)
            Fired(this, EventArgs.Empty);
    }

    private void OnActivated()
    {
        if (Activated != null)
            Activated(this, EventArgs.Empty);
    }

    private void OnDeactivated()
    {
        if (Deactivated != null)
            Deactivated(this, EventArgs.Empty);
    }

    public void Toggle()
    {
        Active = !Active;
    }

    public void BeginCooldown()
    {
        cooldownCoroutine = Cooldown();
        StartCoroutine(cooldownCoroutine);
    }

    public void EndCooldown()
    {
        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        cooldownCoroutine = null;
    }

    public bool Fire(Vector3 target, Collider[] collidersToIgnore = null)
    {
        if (projectilePrefab == null || IsOnCooldown) return false;

        var newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        newProjectile.Initialize(target, collidersToIgnore);

        //audioSource.PlayOneShot(weapon.clip);

        BeginCooldown();

        return true;
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(projectilePrefab.projectileStats.cooldownDuration);

        yield return null;
        cooldownCoroutine = null;
    }
}
