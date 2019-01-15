using UnityEngine;

public class PLRandom
{
    public static bool RollTheDice(float chance)
    {
        chance = Mathf.Clamp(chance, 0, 100);
        return chance <= Random.Range(0, 100);
    }    
}