using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour 
{
    public Vector3 mapCenter;
    public Vector3 defaultMapSize;
    public MapTarget[] targets;
    public Image mapImage;

    public float margin;
    public static Dictionary<Type, Sprite> typesDict;

    [ContextMenu("CalculateBounds")]
    public void Main()
    {
        targets = FindObjectsOfType<MapTarget>();
        if (targets == null || targets.Length < 2) 
        {
            Debug.LogError("Could not calculate bounds. Not enough MapTargets in scene");
            return; 
        }

        var bounds = CalculateMapBounds(targets); 

        var mapObject = new GameObject("Map Bounds");
        mapObject.transform.position = bounds.center;
        var box = Utilities.CheckComponent<BoxCollider>(mapObject);
        box.size = bounds.size;

        var map = mapImage.rectTransform.sizeDelta;
        var overworld = new Vector2(bounds.size.x, bounds.size.z);
        var scale = overworld / map;

        foreach (var target in targets)
        {
            var loc = target.transform.position - bounds.center;
            var temp = new Vector2(loc.x, loc.z) / scale;
            print(temp);
        }
    }

    public Bounds CalculateMapBounds(MapTarget[] gameObjects)
    {
        Bounds bounds = new Bounds(mapCenter, defaultMapSize);

        String[] containees = new String[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            containees[i] = gameObjects[i].name;
            bounds.Encapsulate(gameObjects[i].transform.position);
        }

        var longestSide = bounds.size.x > bounds.size.y ? bounds.size.x : bounds.size.y;
        bounds = new Bounds(bounds.center, new Vector3(longestSide, 0, longestSide));

        bounds.Expand(margin);

        print("bounds contains: " + string.Join(", ", containees));
        print("making bounds with " + bounds);

        return bounds;
    }
}