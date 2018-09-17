using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SingletonScriptableObject<T>: ScriptableObject where T : ScriptableObject
{
    protected static T  _inst;

    public static T Instance
    {
        get
        {
            if (!_inst)
                _inst = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

            if (!_inst)
            {
                _inst = CreateInstance<T>();
            }

            //_inst.hideFlags = HideFlags.DontUnloadUnusedAsset;
            return _inst;
        }
    }
}

