public static class Damage
{
    public static float CalculateShieldDamage(WeaponStats weapon, ShieldType shieldtype)
    {
        float calculatedDamage = 0f;
        float damageModifier = 1f;
        
        switch (shieldtype)
        {
            case ShieldType.Graviton:

                switch (weapon.damageType)
                {
                    case DamageType.Pulse:
                        damageModifier = 0.8f; 
                        break;

                    case DamageType.Plasma:
                        damageModifier = 1.2f;
                        break;

                    case DamageType.Particle:
                        damageModifier = 1.2f;
                        break;

                    case DamageType.Tachyon:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Neutron:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Laser:
                        damageModifier = 0.8f;
                        break;

                    case DamageType.Photon:
                        damageModifier = 0.8f;
                        break;

                    case DamageType.Nomad:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Prototype:
                        damageModifier = 1.0f;
                        break;
                }
                break;

            case ShieldType.Positron:

                switch (weapon.damageType)
                {
                    case DamageType.Pulse:
                        damageModifier = 1.2f; 
                        break;

                    case DamageType.Plasma:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Particle:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Tachyon:
                        damageModifier = 0.8f;
                        break;

                    case DamageType.Neutron:
                        damageModifier = 0.8f;
                        break;

                    case DamageType.Laser:
                        damageModifier = 1.2f;
                        break;

                    case DamageType.Photon:
                        damageModifier = 1.2f;
                        break;

                    case DamageType.Nomad:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Prototype:
                        damageModifier = 1.0f;
                        break;
                }
                break;

            case ShieldType.Molecular:

                switch (weapon.damageType)
                {
                    case DamageType.Pulse:
                        damageModifier = 1.0f; 
                        break;

                    case DamageType.Plasma:
                        damageModifier = 0.8f;
                        break;

                    case DamageType.Particle:
                        damageModifier = 0.8f;
                        break;

                    case DamageType.Tachyon:
                        damageModifier = 1.2f;
                        break;

                    case DamageType.Neutron:
                        damageModifier = 1.2f;
                        break;

                    case DamageType.Laser:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Photon:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Nomad:
                        damageModifier = 1.0f;
                        break;

                    case DamageType.Prototype:
                        damageModifier = 1.0f;
                        break;
                }
                break;
        }

        calculatedDamage = weapon.shieldDamage * damageModifier;

        return calculatedDamage;
    }

    //public static float CalculateHullDamage(Projectile projectile, HullType hulltype)
    //{
    //    float calculatedDamage = 0f;
    //    float damageModifier = 1f;

    //    switch (hulltype)
    //    {
    //        case HullType.Light:
    //            break;

    //        case HullType.Medium:
    //            break;

    //        case HullType.Heavy:
    //            break;

    //        case HullType.Capital:
    //            break;

    //        case HullType.Station:
    //            break;
    //    }

    //    return 0f;
    //}
}
