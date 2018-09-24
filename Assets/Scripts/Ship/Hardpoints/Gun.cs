using System;
using UnityEngine;

public class Gun : Hardpoint
{
    public AudioSource audioSource;
    public Vector3 aimPosition;
    public Transform projectileSpawnPoint;

    public Projectile projectilePrefab;

    private bool _isActive = true;
    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
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
        IsActive = !IsActive;
    }

    public bool Fire(Vector3 target, Collider[] collidersToIgnore = null)
    {
        if (projectilePrefab == null || IsOnCooldown) return false;

        var spawnPoint = projectileSpawnPoint == null ? transform : projectileSpawnPoint;

        var newProjectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        newProjectile.Initialize(target, collidersToIgnore);

        //audioSource.PlayOneShot(weapon.clip);

        StartCooldown(projectilePrefab.weaponStats.cooldownDuration);

        return true;
    }
}
