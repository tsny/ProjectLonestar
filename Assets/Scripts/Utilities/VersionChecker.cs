using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine.UI;

public class VersionChecker : ScriptableObject
{
    public static IEnumerator GetVersions(MonoBehaviour caller = null)
    {
        var url = "https://itch.io/api/1/x/wharf/latest?target=tsny/project-lonestar&channel_name=win";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Couldn't find Version at specified URL...");
                yield break;
            }

            else
            {
                var json = JsonUtility.FromJson<ButlerInfo>(www.downloadHandler.text);
                Debug.Log("Local version: " + Application.version);
                Debug.Log("Live version: " + json.latest);

                if (caller != null)
                {
                    var buildUI = caller.GetComponent<BuildVersionUI>();

                    if (buildUI != null)
                        buildUI.SetText(json.latest);
                }
            }
        }
    }

    public class ButlerInfo
    {
        public string latest;
    }
}
