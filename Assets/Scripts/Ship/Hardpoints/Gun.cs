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
    public Collider[] ignoredColliders;

    [Range(0, 1)]
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

    public override void Initialize(Ship sender)
    {
        base.Initialize(sender);
        ignoredColliders = sender.GetComponentsInChildren<Collider>();
    }

    public void Toggle()
    {
        IsActive = !IsActive;
    }

    private void Awake()
    {
        stats = Utilities.CheckScriptableObject<WeaponStats>(stats);
        cooldown.duration = stats.cooldownDuration;
        SetTargetToNeutral();
    }

    public void SetTargetToNeutral()
    {
        target = transform.forward + transform.position;
    }

    public Projectile Fire(AimPosition aim)
    {
        var proj = SpawnProjectile();
        if (proj == null) return null;
        var pos = aim.rb == null ?
            aim.pos : Utilities.CalculateAimPosition(SpawnPoint, rbTarget, stats.thrust);

        proj.Initialize(pos, stats, ignoredColliders);

        return proj;
    }

    public Projectile SpawnProjectile()
    {
        if (!CanFire) return null;
        if (clip && audioSource) audioSource.PlayOneShot(clip, volume);
        cooldown.Begin(this);
        return Instantiate(projectile, SpawnPoint, Quaternion.identity);
    }
}