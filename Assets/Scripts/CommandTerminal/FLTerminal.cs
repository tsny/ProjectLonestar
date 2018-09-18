using UnityEngine;
using UnityEngine.SceneManagement;
using CommandTerminal;

public class FLTerminal : Terminal
{
    [RegisterCommand(Name = "version.check", Help = "Checks the live version on itch.io against the local version", MinArgCount = 0, MaxArgCount = 0)]
    static void VersionCheck(CommandArg[] args)
    {
        FindObjectOfType<MonoBehaviour>().StartCoroutine(VersionChecker.GetVersions(null, true));
    }

    [RegisterCommand(Help = "Mounts a default loadout onto the current ship", MinArgCount = 0, MaxArgCount = 0)]
    static void MountLoadout(CommandArg[] args)
    {
        if (PlayerControllerExistsInScene() == false) return;

        var pc = FindObjectOfType<PlayerController>();

        if (pc.controlledShip == null) return;

        pc.controlledShip.hardpointSystem.MountLoadout(GameSettings.Instance.defaultLoadout);
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

    [RegisterCommand(Help = "Spawns a ship, args: player, empty", MinArgCount = 1, MaxArgCount = 1)]
    static void Spawn(CommandArg[] args)
    {
        switch (args[0].String)
        {
            case "player":
                ShipSpawner.SpawnShip(GameSettings.Instance.shipPrefab, Vector3.zero, GameSettings.Instance.defaultLoadout);
                print("Spawning player ship...");
                break;

            case "empty":
                ShipSpawner.SpawnShip(GameSettings.Instance.shipPrefab, Vector3.zero);
                print("Spawned default ship...");
                break;
        }
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

    [RegisterCommand(Help = "Reloads the current scene", MinArgCount = 0, MaxArgCount = 0)]
    static void Restart(CommandArg[] args)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        print("Reloading current scene...");
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
