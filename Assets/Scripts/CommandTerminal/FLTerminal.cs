using UnityEngine;
using UnityEngine.SceneManagement;
using CommandTerminal;

public class FLTerminal : Terminal
{
    private PlayerController playerController;
    private ShipSpawner shipSpawner;

    protected override void Initialize()
    {
        base.Initialize();
        name = "TERMINAL";
        playerController = FindObjectOfType<PlayerController>();
        shipSpawner = FindObjectOfType<ShipSpawner>();

        if (playerController == null)
        {
            print(name + " couldn't find player controller...");
            Destroy(gameObject);
        }

        PopulateDebugConsole();
    }

    private void PopulateDebugConsole()
    {
        Shell.AddCommand("God", GodMode, 0, 0, "Current ship invincible");
        Shell.AddCommand("SpawnDefault", SpawnDefaultShip, 0, 0, "Spawns an empty ship");
        Shell.AddCommand("UnlimitedEnergy", UnlimitedEnergy, 0, 0, "Current ship has infinite energy");
        Shell.AddCommand("UnlimitedAfterburner", UnlimitedAfterburner, 0, 0, "Current ship has infinite afterburner");
        Shell.AddCommand("Unpossess", UnPossess, 0, 0, "Unpossess the current ship and go into flycam mode");
        Shell.AddCommand("Restart", Restart, 0, 0, "Restarts the current scene");
        Shell.AddCommand("NewPlayer", SpawnNewPlayerShip, 0, 0, "Spawns the player again");
    }

    public void GodMode(CommandArg[] args)
    {
        playerController.controlledShip.invulnerable = !playerController.controlledShip.invulnerable;
        print("Godmode : " + playerController.controlledShip.invulnerable);
    }

    public void SpawnDefaultShip(CommandArg[] args)
    {
        shipSpawner.SpawnDefaultShip();
        print("Spawned default ship");
    }

    public void UnlimitedEnergy(CommandArg[] args)
    {
        playerController.controlledShip.hardpointSystem.EnableInfiniteEnergy();
    }

    public void UnlimitedAfterburner(CommandArg[] args)
    {
        var abHardpoint = playerController.controlledShip.hardpointSystem.afterburnerHardpoint;

        abHardpoint.drain = abHardpoint.drain == 0 ? 100 : 0;

        print("Toggled infinite afterburner...");
    }

    public void UnPossess(CommandArg[] args)
    {
        playerController.UnPossess();
        print("Ship unpossessed");
    }

    public void SpawnNewPlayerShip(CommandArg[] args)
    {
        FindObjectOfType<GameManager>().SpawnPlayer();
    }

    public void Restart(CommandArg[] args)
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame(CommandArg[] args)
    {
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.TogglePause();
    }
}
