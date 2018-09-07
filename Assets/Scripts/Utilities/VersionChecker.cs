using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class VersionChecker : MonoBehaviour
{
    private string cloudVersion;
    private string localVersion;

    public bool localVersionIsLive
    {
        get
        {
            return cloudVersion == localVersion;
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

            if (www.isNetworkError || www.isHttpError) print("error");

            else
            {
                var json = JsonUtility.FromJson<ButlerInfo>(www.downloadHandler.text);
                cloudVersion = json.latest;
                localVersion = GetLocalVersion();
                print("Note: Local version " + (localVersionIsLive ? "is live" : "isn't live"));
            }
        }
    }

    private string GetLocalVersion()
    {
        var path = @"buildversion.txt";
        string versionString = "";

        try 
        {
            var file = new StreamReader(path);
            versionString = file.ReadLine();
        }

        catch (FileNotFoundException)
        {
            print("Couldn't find buildversion");
        }

        return versionString;
    }

    public class ButlerInfo
    {
        public string latest;
    }
}
