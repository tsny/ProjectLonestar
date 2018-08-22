using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AsteroidField : MonoBehaviour
{
    public GameObject asteroidGO;
    public float innerRadius = 30;
    public float outerRadius = 100;

    public int desiredAsteroids = 1000;

    public float asteroidScale = 1;

    public bool hideAsteroidInHierarchy = true;

    public void GenerateField()
    {
        if (innerRadius > outerRadius)
        {
            print("Invalid raidus, inner must be smaller than outer radius");
            return;
        }

        ClearField();
        CreateAsteroids();
    }

    private void CreateAsteroids()
    {
        Vector3 newPos;
        Quaternion newRot;
        Vector3 newScale = Vector3.one * asteroidScale;

        if (asteroidGO == null)
        {
            print("Asteroid Gameobject is null, creating cubes by default...");
            asteroidGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        // Create new asteroids.
        for (int i = 0; i < desiredAsteroids; i++)
        {
            GameObject newAsteroid = Instantiate(asteroidGO);

            if (hideAsteroidInHierarchy) newAsteroid.hideFlags = HideFlags.HideInHierarchy;

            newAsteroid.transform.parent = gameObject.transform;
            newAsteroid.transform.localScale = newScale;

            newPos = transform.position + Random.onUnitSphere * Random.Range(innerRadius, outerRadius);
            newRot = Random.rotation;

            newAsteroid.transform.SetPositionAndRotation(newPos, newRot);
        }
    }

    public void ClearField()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (Transform transform in tempList) DestroyImmediate(transform.gameObject);
    }
}
