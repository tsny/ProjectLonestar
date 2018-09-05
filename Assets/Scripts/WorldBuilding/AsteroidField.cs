using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AsteroidField : MonoBehaviour
{
    public GameObject AsteroidGameObject { get; set; }

    [Space(10)]
    [Tooltip("Select 'Hide In Hierarchy' to ensure a cleaner hierarchy")]
    public new HideFlags hideFlags;

    public int desiredAsteroids = 1000;

    public float InnerRadius { get; set; }
    public float OuterRadius { get; set; }
    public float AsteroidScale { get; set; }
    public float AsteroidLowerScale { get; set; }
    public float AsteroidUpperScale { get; set; }

    public void GenerateField(bool scaleUsesRange)
    {
        ClearField();
        CreateAsteroids(scaleUsesRange);
    }

    private void CreateAsteroids(bool scaleUsesRange)
    {
        if (scaleUsesRange)
        {
            for (int i = 0; i < desiredAsteroids; i++)
            {
                CreateAsteroid(AsteroidLowerScale, AsteroidUpperScale);
            }
        }

        else
        {
            for (int i = 0; i < desiredAsteroids; i++)
            {
                CreateAsteroid(AsteroidScale);
            }
        }
    }

    private GameObject CreateAsteroid(float scale = 1)
    {
        Vector3 newPos;
        Quaternion newRot;
        Vector3 newScale = Vector3.one * scale;

        GameObject asteroid = Instantiate(AsteroidGameObject);

        asteroid.hideFlags = hideFlags;

        asteroid.transform.parent = gameObject.transform;
        asteroid.transform.localScale = newScale;

        newPos = transform.position + Random.onUnitSphere * Random.Range(InnerRadius, OuterRadius);
        newRot = Random.rotation;

        asteroid.transform.SetPositionAndRotation(newPos, newRot);

        return asteroid;
    }

    private GameObject CreateAsteroid(float lowerScale = 1, float upperScale = 2)
    {
        float randomScale = Random.Range(lowerScale, upperScale);

        return CreateAsteroid(randomScale);
    }

    public void ClearField()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (Transform transform in tempList) DestroyImmediate(transform.gameObject);
    }
}
