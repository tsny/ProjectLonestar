using System;
using UnityEngine;

public class Gun : Hardpoint
{
    public AudioSource audioSource;
    public Vector3 aimPosition;
    public Transform projectileSpawnPoint;
    public bool hideProjectileInHierarchy;

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

    public bool Fire(Vector3 target = new Vector3(), Collider[] collidersToIgnore = null)
    {
        if (projectilePrefab == null || IsOnCooldown) return false;

        // If no target, aim forward
        if (target == Vector3.zero)
        {
            target = transform.forward + transform.position;
        }

        var spawnPoint = projectileSpawnPoint == null ? transform : projectileSpawnPoint;
        var newProjectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        newProjectile.Initialize(target, collidersToIgnore);
        if (hideProjectileInHierarchy)
        {
            newProjectile.hideFlags = HideFlags.HideInHierarchy;
        }

        StartCooldown(projectilePrefab.weaponStats.cooldownDuration);

        return true;
    }
}
