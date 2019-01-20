using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AsteroidField : MonoBehaviour
{
    public BoxCollider spawnZone;
    public BoxCollider innerZone;

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

    private bool rotateToggle;

    private RandomRotator[] rotators;

    public GameObject asteroidPrefab;
    public GameObject[] asteroidPrefabs;

    private void Awake() 
    {
        rotators = new RandomRotator[transform.childCount];

        foreach (Transform child in transform)
        {
            rotators[child.GetSiblingIndex()] = Utilities.CheckComponent<RandomRotator>(child.gameObject);
        }
    }

    public void GenerateField()
    {
        ClearField();

        if (spawnZone == null)
        {
            Debug.LogWarning("No box collider on: " + name);
            return;
        }

        for (int i = 0; i < desiredAsteroids; i++)
        {
            var asteroid = CreateAsteroid();
        }
    }

    public void ToggleAsteroidRotation()
    {
        if (rotators.Length < 1) return;

        rotateToggle = !rotateToggle;

        foreach (var rot in rotators)
        {
            rot.enabled = rotateToggle;
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

        //var newPosition = FindRandomPosition(newScale.x);
        var newPosition = Utilities.RandomPointInBounds(spawnZone.bounds) + transform.position;
        var newRotation = GetRotation(); 

        newAsteroid.transform.SetPositionAndRotation(newPosition, newRotation);

        return newAsteroid;
    }

    private GameObject GetAsteroidGameObject()
    {
        if (useArrayOfAsteroids)
        {
            int randomIndex = Random.Range(0, asteroidPrefabs.Length);
            return asteroidPrefabs[randomIndex];
        }

        else
        {
            return asteroidPrefab;
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

            if (i == 49) Debug.LogWarning("Tried to spawn asteroid 50 times and could not find suitable space...");
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

        if (colliders.Length == 0)
        {
            asteroid.AddComponent<SphereCollider>();
        }

        //colliders.ToList().ForEach(x => DestroyImmediate(x));
    }
}
