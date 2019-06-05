using UnityEngine;

// Spawns random actors when the player enters the zone
// Used to uhhhhh
public class ActiveZone : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] spawnGroups;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SpawnRandomWings();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }

    private void SpawnRandomWings()
    {

    }
}