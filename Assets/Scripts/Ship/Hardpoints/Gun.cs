using System;
using UnityEngine;

public class Gun : ShipComponent
{
    public Vector3 target;
    public Rigidbody rbTarget;
    public WeaponStats stats;
    public Projectile projectile;
    public Transform spawn;
    public AudioSource audioSource;
    public AudioClip clip;

    [Range(0,1)]
    public float volume = .5f;

    public bool CanFire
    {
        get
        {
            if (!ignoreCooldown && cooldown.IsDecrementing) return false;
            else if (projectile == null) return false;
            else return true;
        }
    }

    public Vector3 SpawnPoint
    {
        get { return spawn ? spawn.position : transform.position; }
    }

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
    
    public Cooldown cooldown;
    public bool ignoreCooldown = false;
    public bool useMaxTargetAngle = true;
    public float maxTargetAngle = 180;

    public event EventHandler<EventArgs> Fired;
    public event EventHandler<EventArgs> Activated;
    public event EventHandler<EventArgs> Deactivated;

    private void OnFired() { if (Fired != null) Fired(this, EventArgs.Empty); }
    private void OnActivated() { if (Activated != null) Activated(this, EventArgs.Empty); }
    private void OnDeactivated() { if (Deactivated != null) Deactivated(this, EventArgs.Empty); }

    public void Toggle()
    {
        IsActive = !IsActive;
    }

    private void Awake()
    {
        stats = Utilities.CheckScriptableObject<WeaponStats>(stats);
        cooldown = Utilities.CheckScriptableObject<Cooldown>(cooldown);
        cooldown.duration = stats.cooldownDuration;
        SetTargetToNeutral();
    }

    public void SetTargetToNeutral()
    {
        target = transform.forward + transform.position;
    }

    public bool Fire(AimPosition aim, Collider[] colliders)
    {
        if (aim.HitAnything && aim.HitTargetReticle)
        {
            return FireAtMovingTarget(aim.TargetIndicator.Ship.rb, colliders);

            // else
            // {
            //     return Fire(aim.hit.transform.position, colliders);
            // }
        }

        else if (aim.rb != null)
        {
            return FireAtMovingTarget(aim.rb, colliders);
        }

        else
        {
            return Fire(aim.pos, colliders);
        }
    }

    public bool Fire(Collider[] colliders)
    {
        return Fire(target, colliders);
    }

    public bool Fire(Vector3 target, Collider[] colliders)
    {
        if (!CanFire) return false;

        var proj = Instantiate(projectile, SpawnPoint, Quaternion.identity);
        proj.Initialize(target, stats, colliders);
        if (clip && audioSource) audioSource.PlayOneShot(clip, volume);

        cooldown.Begin(this);
        return true;
    }

    public bool FireAtMovingTarget(Rigidbody rbt, Collider[] colliders)
    {
        rbTarget = rbt;
        if (rbTarget == null) return false;
        var pos = Utilities.CalculateAimPosition(SpawnPoint, rbt, stats.thrust);
        return Fire(pos, colliders);
    }
}
