using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Utilities
{
    /// <summary>
    /// Checks whether a gameObject has a specific component and adds it if it doesn't
    /// </summary>
    public static T CheckComponent<T>(GameObject caller) where T : Component
    {
        var comp = caller.GetComponent<T>();

        if (comp == null)
        {
            comp = caller.AddComponent<T>();
        }
        
        return comp;
    }

    /// <summary>
    /// Checks whether a ScriptableObject reference is null and returns a default instance
    /// If the reference is NOT null, it returns a clone of the original
    /// </summary>
    public static T CheckScriptableObject<T>(ScriptableObject obj) where T : ScriptableObject
    {
        if (obj == null)
        {
            obj = ScriptableObject.CreateInstance<T>();
        }

        else
        {
            obj = ScriptableObject.Instantiate(obj);
        }
        
        return (T) obj;
    }

    /// <summary>
    /// Returns a random vector3 in a bounds
    /// Useful for a random point in a box collider
    /// </summary>
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        var x = Random.Range(-bounds.extents.x, bounds.extents.x);
        var y = Random.Range(-bounds.extents.y, bounds.extents.y);
        var z = Random.Range(-bounds.extents.z, bounds.extents.z);

        return new Vector3(x, y, z);
    } 

    // TODO: Replace projectile ref with float for speed
    //public static Vector3 CalculateAimPosition(Vector3 spawnPoint, Rigidbody target, Float speed)
    public static Vector3 CalculateAimPosition(Vector3 spawnPoint, Rigidbody target, Projectile proj)
    {
        Vector3 aim = Vector3.zero;
        var dist = Vector3.Distance(target.transform.position, spawnPoint);
        var test = dist / proj.stats.thrust;
        //var anotherTest = Time.fixedDeltaTime * test;
        aim = target.transform.position + (target.velocity * test);
        return aim;
    }

     public static bool Chance(float percent, bool logResult = false)
     {
        percent = Mathf.Clamp(percent, 1, 100);
        var roll = Random.Range(1, 100);
        var result = percent >= roll;

        if (logResult)
        {
            Debug.Log(result + ", Chance: " + percent + ", Roll: " + roll + ", Out of: 100");
        }

        return result;
     }
}