using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponHardpoint : Hardpoint
{
    public int slot;
    public int rank;
    public bool active;

    public Weapon Weapon
    {
        get
        {
            return CurrentEquipment as Weapon;
        }
    }

    public AudioSource audioSource;

    protected override bool EquipmentMatchesHardpoint(Equipment equipment)
    {
        return equipment is Weapon;
    }

    public void Toggle()
    {
        active = !active;
    }

    /// <summary>
    /// Fires the equipped weapon
    /// </summary>
    /// <returns>
    /// A bool respresenting if the hardpoint fired 
    /// </returns>
    public bool Fire()
    {
        if (Weapon == null || OnCooldown) return false;

        GameObject newProjectile = Instantiate(Weapon.projectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<ProjectileController>().Initialize(owningShip, Weapon);

        audioSource.PlayOneShot(Weapon.clip);

        StartCooldown();
        return true;
    }
}
