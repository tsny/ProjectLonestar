using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class VersionChecker : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GetVersion());
    }

    IEnumerator GetVersion()
    {
        var url = "https://itch.io/api/1/x/wharf/latest?target=tsny/project-lonestar&channel_name=win";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) print("error");

            else
            {
                var json = JsonUtility.FromJson<ButlerInfo>(www.downloadHandler.text);
                print(json.latest);
            }
        }
    }

    public class ButlerInfo
    {
        public string latest;
    }
}
