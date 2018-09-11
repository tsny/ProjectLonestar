using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponHardpoint : Hardpoint
{
    public int slot;
    public int rank;
    public bool active;

    private Weapon weapon;
    public AudioSource audioSource;

    public bool CanFire
    {
        get
        {
            return hardpointSystem.energy > weapon.energyDraw && !OnCooldown;
        }
    }

    public WeaponHardpoint()
    {
        associatedEquipmentType = typeof(Weapon);
    }

    public override void Mount(Equipment newEquipment)
    {
        base.Mount(newEquipment);

        weapon = newEquipment as Weapon;
        active = true;
    }

    public void Toggle()
    {
        active = !active;
    }

    public void Fire()
    {
        if (!owningShip.CanFire || weapon == null || !CanFire) return;

        hardpointSystem.energy -= weapon.energyDraw;

        GameObject newProjectile = Instantiate(weapon.projectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<ProjectileController>().Initialize(owningShip, weapon);

        audioSource.PlayOneShot(weapon.clip);

        StartCooldown();
    }
}
