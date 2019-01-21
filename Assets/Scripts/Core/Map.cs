using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour 
{
    public float width;
    public float height;
    public float margin;
    public Image bg;
    public static Dictionary<Type, Sprite> typesDict;

    public void Toggle() { gameObject.SetActive(!gameObject.activeSelf); }

    public void CalculateBounds() 
    {
        //FindObjectsOfType<MeshRenderer>();
    }

}