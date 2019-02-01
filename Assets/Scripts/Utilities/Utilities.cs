using System;
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
        var x = UnityEngine.Random.Range(-bounds.extents.x, bounds.extents.x);
        var y = UnityEngine.Random.Range(-bounds.extents.y, bounds.extents.y);
        var z = UnityEngine.Random.Range(-bounds.extents.z, bounds.extents.z);

        return new Vector3(x, y, z);
    } 

    // TODO: Replace projectile ref with float for speed
    //public static Vector3 CalculateAimPosition(Vector3 spawnPoint, Rigidbody target, Float speed)
    public static Vector3 CalculateAimPosition(Vector3 spawnPoint, Rigidbody target, float speed)
    {
        Vector3 aim = Vector3.zero;
        var dist = Vector3.Distance(target.transform.position, spawnPoint);
        var test = dist / speed;
        //var anotherTest = Time.fixedDeltaTime * test;
        aim = target.transform.position + (target.velocity * test);
        return aim;
    }

    /// <summary>
    /// Returns a bool representing a x in 100 chance
    /// </summary>
    public static bool Chance(float percent, bool logResultToConsole = false)
    {
        percent = Mathf.Clamp(percent, 1, 100);
        var roll = UnityEngine.Random.Range(1, 100);
        var result = percent >= roll;

        if (logResultToConsole)
        {
            Debug.Log(result + ", Chance: " + percent + ", Roll: " + roll + ", Out of: 100");
        }

        return result;
    }

    public static T RandomEnumValue<T> ()
    {
        var v = Enum.GetValues (typeof (T));
        return (T) v.GetValue (new System.Random().Next(v.Length));
    }
}