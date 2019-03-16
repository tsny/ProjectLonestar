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
    public float scaleModifier = 2;

    public float margin;
    public static Dictionary<Type, Sprite> typesDict;

    [ContextMenu("Refresh UI")]
    public void Initialize()
    {
        Utilities.ClearChildren(mapImage.transform);

        targets = FindObjectsOfType<MapTarget>();
        if (targets == null || targets.Length < 2 || mapImage == null) 
        {
            Debug.LogError("Could not calculate bounds. Not enough MapTargets in scene or mapImage is null");
            return; 
        }

        var bounds = CalculateMapBounds(targets); 
        var mapSize = mapImage.rectTransform.sizeDelta;
        var worldSize = new Vector2(bounds.size.x, bounds.size.z);
        var mapWorldScale = mapSize / worldSize;

        //CreateVisualizedBounds(bounds);

        foreach (var target in targets)
        {
            var pos = target.transform.position - bounds.center; 
            var mapPos = new Vector2(pos.x, pos.z);
            //var posScale = mapPos / worldSize;
            mapPos.Scale(mapWorldScale);
            var mapIcon = new GameObject("Map_" + target.name).AddComponent<Image>();

            mapIcon.transform.SetParent(mapImage.transform, false);
            mapIcon.rectTransform.localPosition = new Vector3(mapPos.x, mapPos.y);
            mapIcon.sprite = target.mapSprite;
            mapIcon.rectTransform.sizeDelta = target.spriteSize;
        }
    }

    public static GameObject CreateVisualizedBounds(Bounds bounds)
    {
        var mapObject = new GameObject("Map Bounds");
        mapObject.transform.position = bounds.center;
        var box = Utilities.CheckComponent<BoxCollider>(mapObject);
        box.size = bounds.size;
        return mapObject;
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