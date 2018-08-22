using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AsteroidField : MonoBehaviour
{
    public GameObject AsteroidGameObject { get; set; }

    public new HideFlags hideFlags;

    public int desiredAsteroids = 1000;

    public float InnerRadius { get; set; }
    public float OuterRadius { get; set; }
    public float AsteroidScale { get; set; }

    public void GenerateField()
    {
        ClearField();
        CreateAsteroids();
    }

    private void CreateAsteroids()
    {
        Vector3 newPos;
        Quaternion newRot;
        Vector3 newScale = Vector3.one * AsteroidScale;


        // Create new asteroids.
        for (int i = 0; i < desiredAsteroids; i++)
        {
            GameObject newAsteroid = Instantiate(AsteroidGameObject);

            newAsteroid.hideFlags = hideFlags;

            newAsteroid.transform.parent = gameObject.transform;
            newAsteroid.transform.localScale = newScale;

            newPos = transform.position + Random.onUnitSphere * Random.Range(InnerRadius, OuterRadius);
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
