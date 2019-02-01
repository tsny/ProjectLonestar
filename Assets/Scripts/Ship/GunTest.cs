using UnityEngine;

public class GunTest : MonoBehaviour 
{
    public ParticleSystem ps;

    public void Fire()
    {
        if (!ps)
        {
            Debug.LogWarning("GunTest on " + name + " has no particle system assigned...");
            return;
        }

        ps.Emit(1);
    }
}