using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using CommandTerminal;

public class FLTerminal : Terminal
{
    // Methods to add:
    // volume {float}
    // mute
    // toggle hud


    [RegisterCommand(Name = "version.check", Help = "Checks the live version on itch.io against the local version", MinArgCount = 0, MaxArgCount = 0)]
    static void VersionCheck(CommandArg[] args)
    {
        FindObjectOfType<MonoBehaviour>().StartCoroutine(VersionChecker.GetVersions("https://itch.io/api/1/x/wharf/latest?target=tsny/project-lonestar&channel_name=win", null, true));
    }

    [RegisterCommand(Help = "Mounts a default loadout onto the current ship", MinArgCount = 0, MaxArgCount = 0)]
    static void MountLoadout(CommandArg[] args)
    {
        if (PlayerController.instance.ship == null) return;

        //PlayerController.instance.ship.hardpointSystem.MountLoadout(GameSettings.instance.defaultLoadout);
    }

    [RegisterCommand(Help = "Toggle GodMode on Current Ship", MinArgCount = 0, MaxArgCount = 0)]
    static void God(CommandArg[] args)
    {
        PlayerController.instance.ship.invulnerable = !PlayerController.instance.ship.invulnerable;
        print("Godmode : " + PlayerController.instance.ship.invulnerable);
    }

    [RegisterCommand(Help = "Spawns a ship. Usage: spawn {entity} {times} {possess?}", MinArgCount = 1, MaxArgCount = 3)]
    static void Spawn(CommandArg[] args)
    {
        string entity = args[0].String;

        if (Terminal.IssuedError) return;

        Ship shipToSpawn = null;
        var ships = FindObjectsOfType<Ship>();

        switch (entity)
        {
            case "self":
                shipToSpawn = PlayerController.instance.ship;
                break;

            case "random":
                shipToSpawn = ships[Random.Range(0, ships.Length)];
                break;

            case "nearest":
                ships.ToList().Remove(PlayerController.instance.ship);

                Ship closestShip = null;
                float closestShipDistance = 0;

                closestShip = ships[0];
                closestShipDistance = Vector3.Distance(PlayerController.instance.ship.transform.position, closestShip.transform.position);

                foreach (var ship in ships)
                {
                    Vector3.Distance(PlayerController.instance.ship.transform.position, ship.transform.position);
                }

                shipToSpawn = null;
                break;

            // Case for specific ship name 

            default:
                print("Could not spawn entity of name " + entity);
                return;
        }

        Ship spawnedShip = null;

        if (args.Length > 1)
        {
            for (int i = 0; i < args[1].Int; i++)
            {
                spawnedShip = ShipSpawner.SpawnShip(shipToSpawn.gameObject, Vector3.zero);
            }
        }

        else
        {
            spawnedShip = ShipSpawner.SpawnShip(shipToSpawn.gameObject, Vector3.zero);
        }

        if (args.Length > 2)
        {
            if (args[2].Bool)
            {
                PlayerController.instance.Possess(spawnedShip);
            }
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
        PlayerController.instance.ship.hardpointSystem.EnableInfiniteEnergy();
    }

    [RegisterCommand(Help = "Gives current ship unlimited afterburner energy", MinArgCount = 0, MaxArgCount = 0)]
    static void Impulse102(CommandArg[] args)
    {
        var abHardpoint = PlayerController.instance.ship.hardpointSystem.afterburner;
        abHardpoint.afterburner.drainRate = abHardpoint.afterburner.drainRate == 0 ? 100 : 0;

        print("Toggled infinite afterburner...");
    }

    [RegisterCommand(Help = "Unpossesses the current ship", MinArgCount = 0, MaxArgCount = 0)]
    static void Release(CommandArg[] args)
    {
        PlayerController.instance.Release();
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
        var newPower = Mathf.Clamp(args[0].Int, 0, 99999);
        PlayerController.instance.ship.engine.engineStats.enginePower = newPower;
    }

    [RegisterCommand(Name = "cruise.power", Help = "Change the current ship's cruise power", MinArgCount = 1, MaxArgCount = 1)]
    static void SetCruisePower(CommandArg[] args)
    {
        var newPower = Mathf.Clamp(args[0].Int, 0, 99999);
        PlayerController.instance.ship.cruiseEngine.stats.thrust = newPower;
    }
}
