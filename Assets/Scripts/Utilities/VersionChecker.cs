using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class VersionChecker : ScriptableObject
{
    public static IEnumerator GetVersions(MonoBehaviour caller = null, bool printToConsole = false)
    {
        var url = "https://itch.io/api/1/x/wharf/latest?target=tsny/project-lonestar&channel_name=win";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("ERROR: URL is either invalid or the network is down...");
                yield break;
            }

            else
            {
                var json = JsonUtility.FromJson<ButlerInfo>(www.downloadHandler.text);

                if (printToConsole)
                {
                    Debug.Log("Local version: " + Application.version);
                    Debug.Log("Live version: " + json.latest);
                }

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
