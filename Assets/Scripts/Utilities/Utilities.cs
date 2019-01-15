using UnityEngine;

public static class Utilities
{
    /// <summary>
    /// Checks whether a gameObject has a specific component and adds it if it doesn't
    /// </summary>
    public static T CheckComponent<T>(GameObject obj) where T : Component
    {
        var comp = obj.GetComponent<T>();

        if (comp == null)
        {
            comp = obj.AddComponent<T>();
        }
        
        return comp;
    }
}