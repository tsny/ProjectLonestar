using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Reflection;

public class VersionChecker : MonoBehaviour
{
    public delegate void EventHandler(VersionChecker sender);
    public event EventHandler UpdateFound;

    public bool VersionChecked { get; private set; }

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
        StartCoroutine(GetVersions());
    }

    private void OnVersionsChecked(string liveVersion, string localVersion)
    {
        LiveVersion = liveVersion;
        LocalVersion = localVersion;
        print("Note: Local version " + (LocalVersionIsLive ? "is live" : "isn't live"));
        VersionChecked = true;
        if (UpdateFound != null) UpdateFound(this);
    }

    private IEnumerator GetVersions()
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
                OnVersionsChecked(json.latest, Application.version);
            }
        }
    }

    public class ButlerInfo
    {
        public string latest;
    }
}
