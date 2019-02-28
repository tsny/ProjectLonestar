using UnityEngine;

public class JobGenerator : MonoBehaviour
{
    public static string[] adjectives = { "Fiery", "Destitute", "Loner", "Aggressive", "Insane", "Huge", "Powerful", "Weak", "Wimpy", "Overpowered" };
    public ShipSpawner[] missionZones;

    public ShipSpawner currentSpawner;
    public GameObject currentMission;

    public static string GenerateTitle()
    {
        int numOfEnemies = UnityEngine.Random.Range(1, 7);
        string adjective = adjectives[new System.Random().Next(adjectives.Length)];
        return numOfEnemies + " " + adjective + " enemy ships need to be dealt with";
    }    

    private void HandleSpawnTriggered()
    {
        currentSpawner.Triggered -= HandleSpawnTriggered;
    }

    void Start()
    {
        GenerateMissionInZone();
    }

    public void GenerateMissionInZone()
    {
        Reset();

        var zone = GetRandomZone();
        var mission = new GameObject("PLAYERMISSION");
        mission.AddComponent<TargetingInfo>().Init("MISSION", null, false);

        currentMission = mission;
        currentSpawner = zone;
        currentSpawner.Triggered += HandleSpawnTriggered;
    }

    private void Reset()
    {
        if (currentMission) Destroy(currentMission);
        if (currentSpawner) currentSpawner.Triggered -= HandleSpawnTriggered;
    }
    
    private ShipSpawner GetRandomZone()
    {
        if (missionZones == null || missionZones.Length < 1)
        {
            Debug.LogError("MissionZones cannot be null or empty!");
            return null;
        }

        return missionZones[new System.Random().Next(missionZones.Length)];
    }
}