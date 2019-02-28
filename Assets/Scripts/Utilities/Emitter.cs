using UnityEngine;

public class Emitter : MonoBehaviour 
{
    public ParticleSystem ps;

    public void Fire()
    {
        ps.Emit(1);
    }

    private void OnParticleCollision(GameObject other) 
    {
        print("Collided with: " + other);
    }

    private void OnParticleTrigger() 
    {
        
    }

}