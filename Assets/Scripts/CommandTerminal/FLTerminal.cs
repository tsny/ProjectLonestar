using UnityEngine;
using UnityEngine.SceneManagement;
using CommandTerminal;

public class FLTerminal : Terminal
{
    [RegisterCommand(Name = "version.check", Help = "Checks the live version on itch.io against the local version", MinArgCount = 0, MaxArgCount = 0)]
    static void VersionCheck(CommandArg[] args)
    {
        FindObjectOfType<MonoBehaviour>().StartCoroutine(VersionChecker.GetVersions());
    }

    [RegisterCommand(Help = "Toggle GodMode on Current Ship", MinArgCount = 0, MaxArgCount = 0)]
    static void God(CommandArg[] args)
    {
        if (PlayerControllerExistsInScene() == false) return;

        var pc = FindObjectOfType<PlayerController>();

        if (pc.controlledShip == null) return;

        pc.controlledShip.invulnerable = !pc.controlledShip.invulnerable;

        print("Godmode : " + pc.controlledShip.invulnerable);
    }

    [RegisterCommand(Help = "Spawns an empty ship at level origin (0,0,0)", MinArgCount = 0, MaxArgCount = 0)]
    static void SpawnDefault(CommandArg[] args)
    {
        ShipSpawner.SpawnDefaultShip();
        print("Spawned default ship");
    }

    [RegisterCommand(Help = "Toggles the game's time scale between 1 and 0", MinArgCount = 0, MaxArgCount = 0)]
    static void Pause(CommandArg[] args)
    {
        GameStateUtils.TogglePause();
    }

    [RegisterCommand(Help = "Gives current ship unlimited energy", MinArgCount = 0, MaxArgCount = 0)]
    static void Impulse101(CommandArg[] args)
    {
        if (PlayerControllerExistsInScene() == false) return;

        var playerController = FindObjectOfType<PlayerController>();
        playerController.controlledShip.hardpointSystem.EnableInfiniteEnergy();
    }

    [RegisterCommand(Help = "Gives current ship unlimited afterburner energy", MinArgCount = 0, MaxArgCount = 0)]
    static void Impulse102(CommandArg[] args)
    {
        if (PlayerControllerExistsInScene() == false) return;

        var abHardpoint = FindObjectOfType<PlayerController>().controlledShip.hardpointSystem.afterburnerHardpoint;
        abHardpoint.drain = abHardpoint.drain == 0 ? 100 : 0;

        print("Toggled infinite afterburner...");
    }

    [RegisterCommand(Help = "Unpossesses the current ship", MinArgCount = 0, MaxArgCount = 0)]
    static void UnPossess(CommandArg[] args)
    {
        if (PlayerControllerExistsInScene() == false) return;

        FindObjectOfType<PlayerController>().Possess(null);

        print("Ship unpossessed");
    }


    [RegisterCommand(Name = "SpawnNew", Help = "Spawns a new ship and possesses it", MinArgCount = 0, MaxArgCount = 0)]
    static void SpawnNewPlayerShip(CommandArg[] args)
    {
        DebugSettings.Instance.SpawnPrefabs();
    }

    [RegisterCommand(Help = "Restarts the current scene", MinArgCount = 0, MaxArgCount = 0)]
    static void Restart(CommandArg[] args)
    {
        SceneManager.LoadScene(0);
    }

    // Change this to apply to all speeds
    [RegisterCommand(Name = "throttle.power", Help = "Change the current ship's throttle power", MinArgCount = 1, MaxArgCount = 1)]
    static void SetThrottlePower(CommandArg[] args)
    {
        if (PlayerControllerExistsInScene() == false) return;

        FindObjectOfType<PlayerController>().controlledShip.engine.throttlePower = args[0].Int;
    }

    [RegisterCommand(Name = "cruise.power", Help = "Change the current ship's cruise power", MinArgCount = 1, MaxArgCount = 1)]
    static void SetCruisePower(CommandArg[] args)
    {
        if (PlayerControllerExistsInScene() == false) return;

        FindObjectOfType<PlayerController>().controlledShip.cruiseEngine.cruisePower = args[0].Int;
    }

    private static bool PlayerControllerExistsInScene()
    {
        var pc = FindObjectOfType<PlayerController>();
        if (pc == null)
        {
            print("ERROR: Couldn't find Player Controller in scene...");
            return false;
        }

        return true;
    }
}
