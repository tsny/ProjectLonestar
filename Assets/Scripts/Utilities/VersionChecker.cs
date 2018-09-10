using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Reflection;

public class VersionChecker : MonoBehaviour
{
    public delegate void EventHandler(VersionChecker sender);
    public event EventHandler UpdateFound;

    public string LiveVersion { get; set; }
    public string LocalVersion { get; set; }

    public bool LocalVersionIsLive
    {
        get
        {
            return LiveVersion == LocalVersion;
        }
    }

    private void Start()
    {
        StartCoroutine(VersionCheck());
    }

    private IEnumerator VersionCheck()
    {
        var url = "https://itch.io/api/1/x/wharf/latest?target=tsny/project-lonestar&channel_name=win";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                print("Couldn't find Version at specified URL...");
                yield break;
            }

            else
            {
                var json = JsonUtility.FromJson<ButlerInfo>(www.downloadHandler.text);
                LiveVersion = json.latest;
                LocalVersion = Application.version;

                if (UpdateFound != null) UpdateFound(this);
                print("Note: Local version " + (LocalVersionIsLive ? "is live" : "isn't live"));
            }
        }
    }

    public class ButlerInfo
    {
        public string latest;
    }
}
