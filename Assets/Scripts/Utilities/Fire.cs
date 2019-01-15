using UnityEngine;

public class Fire : MonoBehaviour 
{
    public Projectile projectile;
    public Transform spawn;
    public WeaponStats stats;

    public new AudioSource audio;
    public AudioClip clip;

    public Fire[] fires;

    [Range(0,1)]
    public float volume;

    private void Awake() 
    {
        fires = FindObjectsOfType<Fire>();
    }

    public void FireThis()
    {
        var proj = Instantiate(projectile, spawn.position, Quaternion.identity);
        proj.Initialize(Vector3.forward + spawn.position, stats, null);
        audio.PlayOneShot(clip, volume);
    }

    public void FireAll()
    {
        foreach (var fire in fires)
        {
            fire.FireThis();
        }
    }

}