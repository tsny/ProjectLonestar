using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    private bool _inv;
    public bool Invulnerable
    {
        get
        {
            return _inv;
        }

        set
        {
            if (InvulnerableToggled != null) InvulnerableToggled(this);
            _inv = value;
        }
    }

    public float Amount
    {
        get
        {
            return _amount;
        }

        set
        {
            _amount = value;
            if (HealthUpdated != null) HealthUpdated(this);
        }
    }

    public float Shield
    {
        get
        {
            return _shield;
        }

        set
        {
            _shield = value;
        }
    }

    public float Armor
    {
        get
        {
            return armor;
        }

        set
        {
            armor = value;
        }
    }

    public HealthStats stats;

    private float _amount = 100;
    private float _shield = 100;
    private float armor;

    public event HealthEventHandler HealthUpdated;
    public event HealthEventHandler HealthDepleted;
    public event HealthEventHandler InvulnerableToggled;

    public delegate void HealthEventHandler(Health sender);

    private void Awake() 
    {
        Init();
    }

    public void Init()
    {
        stats = Utilities.CheckScriptableObject<HealthStats>(stats);
        Amount = stats.startingHealth;
        Shield = stats.startingShield;
        Armor = stats.startingArmor;
    } 

    private void OnTookDamage(WeaponStats weapon) { if (HealthUpdated != null) HealthUpdated(this); }
    private void OnHealthDepleted() { if (HealthDepleted != null) HealthDepleted(this); }

    public virtual void TakeDamage(WeaponStats weapon)
    {
        if (_inv || Amount <= stats.minHealth) return;

        if (Shield >= stats.minShield)
        {
            Shield -= weapon.shieldDamage;
        }

        else if (Armor >= stats.minArmor)
        {
            // Do some more calculations here
            Armor -= weapon.hullDamage;
        }

        else if (Amount >= stats.minHealth)
        {
            Amount -= weapon.hullDamage;
        }

        OnTookDamage(weapon);

        if (Amount <= stats.minHealth) OnHealthDepleted();
    }

    public void FullHealth()
    {
        Amount = stats.maxHealth;
        Shield = stats.maxShield;
    }

    public void Deplete()
    {
        Amount = 0;        
        Shield = 0;
    }
}

[System.Serializable]
public class HealthObject
{
    public float current = 100;
    public float start = 100;
    public float max = 100;
    public float min = 0;
    //public healthtype;

    public void Init()
    {
        current = start;
    }
}