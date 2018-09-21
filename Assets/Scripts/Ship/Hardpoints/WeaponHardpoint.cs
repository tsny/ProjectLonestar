using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WeaponHardpoint : MonoBehaviour 
{
    public AudioSource audioSource;
    public Weapon weapon;
    public Vector3 target;

    public bool Active { get; set; }
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

    private void Awake()
    {
        if (weapon == null)
            weapon = ScriptableObject.CreateInstance<Weapon>();

        weapon = Instantiate(weapon);
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

    public bool Fire()
    {
        if (weapon == null || IsOnCooldown) return false;

        GameObject newProjectile = Instantiate(weapon.projectile, transform.position, Quaternion.identity);

        newProjectile.GetComponent<ProjectileController>().Initialize(weapon, target);

        //audioSource.PlayOneShot(weapon.clip);

        BeginCooldown();

        return true;
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(weapon.cooldownDuration);

        cooldownCoroutine = null;
    }
}
