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
    /// </summar>
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        var x = Random.Range(-bounds.extents.x, bounds.extents.x);
        var y = Random.Range(-bounds.extents.y, bounds.extents.y);
        var z = Random.Range(-bounds.extents.z, bounds.extents.z);

        return new Vector3(x, y, z);
    } 
}