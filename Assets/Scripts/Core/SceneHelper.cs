using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour 
{
    public FadeCamera fade;
    public PlayerController pc;
    public bool spawnPlayer;

    protected virtual void Awake()
    {
        fade.updateElapsed = false;
    }

    protected virtual void Start()
    {
        if (spawnPlayer)
        {
            var ship = Instantiate(GameSettings.Instance.defaultShip, pc.transform.position, Quaternion.identity);
            pc.Possess(ship);
        }


        Invoke("StartFadeIn", 2);
    }

    [ContextMenu("test")]
    public void Test()
    {
        var test = SaveManager.QuickSave();
        print(test.name);
        print(test.Filepath);
    }

    public void StartFadeIn()
    {
        fade.updateElapsed = true;
    }
}