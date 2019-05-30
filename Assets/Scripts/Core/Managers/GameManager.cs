using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public HUDManager hud;
    public PlayerController pc;
    public FadeCamera fade;

    public bool spawnPlayer = true;

    private void Start()
    {
        if (spawnPlayer)
        {
            var ship = Instantiate(GameSettings.Instance.defaultShip, pc.transform.position, Quaternion.identity);
            pc.Possess(ship);
        }

        Invoke("StartFadeIn", 2);

        hud.SetPlayerController(pc); 
    }

    private void Awake() 
    {
        fade.updateElapsed = false;

        if (hud == null || pc == null || fade == null) 
        {
            Debug.LogError("GameManager missing important reference");
            Debug.LogError("Could not setup player objects");
            return;
        }
    }

    public void StartFadeIn()
    {
        fade.updateElapsed = true;
    }
}