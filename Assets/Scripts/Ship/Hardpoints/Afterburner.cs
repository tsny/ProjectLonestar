using System;
using System.Collections;
using UnityEngine;

public class Afterburner : Hardpoint
{
    public AfterburnerStats stats;
    public float charge;
    public bool Burning {get; private set;}
    public bool ignoreDrain;
    public Cooldown outOfEnergyCooldown;
    public ParticleSystem ps;
    public new AudioSource audio;

    public event ToggledEventHandler Toggled;
    public delegate void ToggledEventHandler(bool toggle);

    private void OnActivated() { if (Toggled != null) Toggled(true); }
    private void OnDeactivated() { if (Toggled != null) Toggled(false); }

    private void Awake()
    {
        outOfEnergyCooldown = Utilities.CheckScriptableObject<Cooldown>(outOfEnergyCooldown);
        stats = Utilities.CheckScriptableObject<AfterburnerStats>(stats);
        charge = stats.capacity;
    }

    public void Activate()
    {
        if (Burning || outOfEnergyCooldown.IsDecrementing) return;
        else
        {
            Burning = true;
            ps.Play();
            audio.Play();
        }
    }

    public void Deactivate()
    {
        if (!Burning) return;
        else
        {
            Burning = false;
            ps.Stop();
            audio.Stop();
        }
    }

    void FixedUpdate()
    {
        if (Burning && !ignoreDrain)
        {
            charge -= stats.drainRate;
            if (charge <= 0)
            {
                outOfEnergyCooldown.Begin(this);
                Deactivate();
            }
        }
        else if (!outOfEnergyCooldown.IsDecrementing)
        {
            charge = Mathf.MoveTowards(charge, 100, stats.regenRate);
        }
    }
}