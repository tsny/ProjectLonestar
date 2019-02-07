using System.Collections;
using UnityEngine;

public static class StaticCoroutine 
{
    private class CoroutineHolder : MonoBehaviour { }
 
    //lazy singleton pattern. Note that I don't set it to dontdestroyonload - you usually want corotuines to stop when you load a new scene.
    private static CoroutineHolder _runner;
    private static CoroutineHolder Runner 
    {
        get 
        {
            if (_runner == null) 
            {
                _runner = new GameObject("Static Coroutine Runner").AddComponent<CoroutineHolder>();
                _runner.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }
            return _runner;
        }
    }
 
    public static void StartCoroutine(IEnumerator routine) 
    {
        Runner.StartCoroutine(routine);
    }
}