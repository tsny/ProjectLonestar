using UnityEngine;

public class SceneHelper : MonoBehaviour 
{
    public FadeCamera fade;
    public PlayerController pc;

    void Awake()
    {
        fade.updateElapsed = false;
    }

    void Start()
    {
        var ship = Instantiate(GameSettings.Instance.defaultShip, pc.transform.position, Quaternion.identity);
        pc.Possess(ship);
        Invoke("Test", 2);
    }

    void Test()
    {
        fade.updateElapsed = true;
    }
}