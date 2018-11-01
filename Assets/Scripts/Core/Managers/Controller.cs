using UnityEngine;

public class Controller<T> : MonoBehaviour where T : Component
{
    private static bool _applicationIsQuitting = false;
    
    private static T _instance = null;
    public static T instance
    {
        get
        {
            if (_instance == null && !_applicationIsQuitting)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                    _instance = new GameObject("_" + typeof(T).Name).AddComponent<T>();

                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _instance = null;
        Destroy(gameObject);
        _applicationIsQuitting = true;
    }
}