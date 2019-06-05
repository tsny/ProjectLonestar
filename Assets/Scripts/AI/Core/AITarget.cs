using UnityEngine;

public class AITarget
{
    public bool hasBeenSet;
    public GameObject targetObject;
    public Vector3 Position 
    {
        get 
        {
            if (targetObject != null) 
                return targetObject.transform.position;
            else 
                return targetPoint;
        }
    }
    public Ship ship;

    private Vector3 targetPoint;

    public void Pick(Ship ship)
    {
        this.ship = ship;
        hasBeenSet = true;
    }

    public void Pick(GameObject go)
    {
        targetObject = go;
        hasBeenSet = true;
    }

    public void Pick(Vector3 point)
    {
        targetPoint = point;
        hasBeenSet = true;
    }

    public void Clear()
    {
        hasBeenSet = false;
        targetObject = null;
        ship = null;
        targetPoint = Vector3.zero;
    }
}