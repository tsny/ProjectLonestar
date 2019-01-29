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

    private void Awake() 
    {
        CalculateBounds();     
    }

    [ContextMenu("CalculateBounds")]
    public void CalculateBounds() 
    {
        var targets = FindObjectsOfType<MapTarget>();
        if (targets == null || targets.Length < 2) return; 

        DistObj[] objs = new DistObj[targets.Length];
        int biggestIndex = 0;
        
        for (int i = 0; i < targets.Length; i++)
        {
            objs[i].self = targets[i].gameObject;

            for (int j = 0; j < targets.Length; j++)
            {
                if (targets[i] == targets[j]) continue;

                var distTest = Vector3.Distance(objs[i].self.transform.position, targets[j].transform.position);

                if (distTest > objs[i].dist) 
                {
                    objs[i].target = targets[j].gameObject;
                    objs[i].dist = distTest;
                }
            }

            if (objs[i].dist > objs[biggestIndex].dist)
                biggestIndex = i;
        }

        DistObj furthestPoints = objs[biggestIndex];
    }


    struct DistObj
    {
        public GameObject self;
        public GameObject target;
        public float dist;
    }
}