using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AsteroidField : MonoBehaviour
{
    public bool scaleUsesRange = false;
    public bool useArrayOfAsteroids = false;
    public bool useRandomRotation = true;
    public bool checkForCollider = true;

    public new HideFlags hideFlags = HideFlags.HideInHierarchy;

    public int desiredAsteroids = 100;

    public float innerRadius = 100;
    public float outerRadius = 300;
    public float asteroidScaleMultiplier = 1;
    public float asteroidLowerScale = 1;
    public float asteroidUpperScale = 3;

    public GameObject asteroid;
    public GameObject[] asteroids;

    public void GenerateField()
    {
        ClearField();

        for (int i = 0; i < desiredAsteroids; i++)
        {
            CreateAsteroid();
        }
    }

    public void ClearField()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (Transform transform in tempList) DestroyImmediate(transform.gameObject);
    }

    private GameObject CreateAsteroid()
    {
        Vector3 newScale = GetScale();

        GameObject newAsteroid = Instantiate(GetAsteroidGameObject());

        newAsteroid.hideFlags = hideFlags;

        newAsteroid.transform.parent = transform;
        newAsteroid.transform.localScale = newScale;

        CheckForCollider(newAsteroid);

        var newPosition = FindRandomPosition(newScale.x);
        var newRotation = GetRotation(); 

        newAsteroid.transform.SetPositionAndRotation(newPosition, newRotation);

        return asteroid;
    }

    private GameObject GetAsteroidGameObject()
    {
        if (useArrayOfAsteroids)
        {
            int randomIndex = Random.Range(0, asteroids.Length);
            return asteroids[randomIndex];
        }

        else
        {
            return asteroid;
        }
    }

    private Vector3 FindRandomPosition(float boundsSize)
    {
        var newPosition = transform.position + Random.onUnitSphere * Random.Range(innerRadius, outerRadius);

        // Check only 50 times in so we don't crash...

        for (int i = 0; i < 50; i++)
        {
            newPosition = transform.position + Random.onUnitSphere * Random.Range(innerRadius, outerRadius);
            if (!Physics.CheckSphere(newPosition, boundsSize)) break;
        }

        return newPosition;
    }

    private Quaternion GetRotation()
    {
        return useRandomRotation ? Random.rotation : Quaternion.identity;
    }

    private Vector3 GetScale()
    {
        Vector3 newScale = Vector3.one;

        if (scaleUsesRange)
        {
            newScale *= Random.Range(asteroidLowerScale, asteroidUpperScale);
        }

        else
        {
            newScale *= asteroidScaleMultiplier;
        }

        return newScale;
    }

    private void CheckForCollider(GameObject asteroid)
    {
        var colliders = asteroid.GetComponentsInChildren<Collider>();

        colliders.ToList().ForEach(x => DestroyImmediate(x));

        asteroid.AddComponent<SphereCollider>();
    }
}
