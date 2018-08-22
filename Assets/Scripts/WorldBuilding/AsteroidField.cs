using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AsteroidField : MonoBehaviour
{
    public struct ScaleInfo
    {
        public float scale;
        public float lowerRange;
        public float upperRange;

        public ScaleInfo(float lowerRange, float upperRange)
        {
            this.lowerRange = lowerRange;
            this.upperRange = upperRange;
            scale = 0;
        }

        public ScaleInfo(float scale)
        {
            lowerRange = 0;
            upperRange = 0;
            this.scale = Mathf.Abs(scale);
        }
    }

    public GameObject asteroidGO;

    public float innerRadius = 30;
    public float outerRadius = 100;

    public HideFlags asteroidHideFlags = HideFlags.HideInHierarchy;

    public int desiredAsteroids = 100;

    public bool scaleUsesRange;

    public void GenerateField(ScaleInfo scaleInfo)
    {
        if (!IsRadiusValid()) return;

        ClearField();
        ValidateAsteroid();

        for (int i = 0; i < desiredAsteroids; i++)
        {
            CreateAsteroid(scaleInfo);
        }
    }

    private bool IsRadiusValid()
    {
        if (innerRadius > outerRadius)
        {
            print("Invalid raidus, inner must be smaller than outer radius");
            return false;
        }

        return true;
    }

    private void ValidateAsteroid()
    {
        if (asteroidGO == null)
        {
            print("Asteroid Gameobject is null, creating cubes by default...");
            asteroidGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }

    private void CreateAsteroid(ScaleInfo scaleInfo)
    {
        Vector3 position;
        Quaternion rotation;
        Vector3 scale = SetScale(scaleInfo);

        GameObject newAsteroid = Instantiate(asteroidGO);

        newAsteroid.hideFlags = asteroidHideFlags;

        newAsteroid.transform.parent = gameObject.transform;
        newAsteroid.transform.localScale = scale;

        position = transform.position + Random.onUnitSphere * Random.Range(innerRadius, outerRadius);
        rotation = Random.rotation;

        newAsteroid.transform.SetPositionAndRotation(position, rotation);
    }

    private Vector3 SetScale(ScaleInfo scaleInfo)
    {
        if (scaleInfo.scale == 0)
        {
            return Vector3.one * Random.Range(scaleInfo.lowerRange, scaleInfo.upperRange);
        }

        return Vector3.one * scaleInfo.scale;
    }

    public void ClearField()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (Transform transform in tempList) DestroyImmediate(transform.gameObject);
    }
}
