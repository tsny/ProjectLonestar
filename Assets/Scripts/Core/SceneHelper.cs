using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour 
{
    public FadeCamera fade;
    public PlayerController pc;
    public bool spawnPlayer;

    public bool HasStartedFadeIn { get; private set; }

    void Awake()
    {
        fade.updateElapsed = false;
    }

    void Start()
    {
        if (spawnPlayer)
        {
            var ship = Instantiate(GameSettings.Instance.defaultShip, pc.transform.position, Quaternion.identity);
            pc.Possess(ship);
        }

        Invoke("StartFading", 2);
    }

    public void StartFading()
    {
        fade.updateElapsed = true;
        HasStartedFadeIn = true;
    }
    
    public void FadeToScene(string scnName)
    {
        if (!IsSceneValid(scnName)) return;
        StartCoroutine(FadeRoutine("SCN_Debug"));
    }

    bool IsSceneValid(string scnName)
    {
        Scene scn = SceneManager.GetSceneByName(scnName);
        return scn.buildIndex != 1;
    }

    private IEnumerator FadeRoutine(string scnName)
    {
        fade.fadeIn = false;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scnName);
    }
}