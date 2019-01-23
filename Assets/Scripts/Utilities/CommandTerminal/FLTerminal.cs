using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using CommandTerminal;

public class FLTerminal : Terminal
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            Flycam.TakeScreenshot();
    }

    [RegisterCommand(Name = "map", MinArgCount = 0, MaxArgCount = 1)]
    static void ChangeScene(CommandArg[] args)
    {
        if (args.Length == 0)
        {
            Log("Current scene: " + SceneManager.GetActiveScene().name);
            return;
        }

        var scn = args[0].String;

        switch (scn)
        {
            case "main":
                scn = GameSettings.Instance.menuSceneName;
                break;
            case "test":
                scn = "SCN_Debug";
                break;
        }

        SceneManager.LoadScene(scn);
        Debug.Log("Loading scene: " + scn);
    }

    [RegisterCommand(Name = "version.check", Help = "Checks the live version on itch.io against the local version", MinArgCount = 0, MaxArgCount = 0)]
    static void VersionCheck(CommandArg[] args)
    {
        FindObjectOfType<MonoBehaviour>().StartCoroutine(VersionChecker.GetVersions("https://itch.io/api/1/x/wharf/latest?target=tsny/project-lonestar&channel_name=win", null, true));
    }

    [RegisterCommand(Name = "hud", MinArgCount = 0, MaxArgCount = 0)]
    static void ToggleHUD(CommandArg[] args)
    {
        var canvas = FindObjectOfType<HUDManager>().GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;
    }

    [RegisterCommand(Name = "mount", Help = "Mounts a default loadout onto the current ship", MinArgCount = 0, MaxArgCount = 0)]
    static void MountLoadout(CommandArg[] args)
    {
        if (GameSettings.pc.ship == null) return;

        //GameSettings.pc.ship.hardpointSystem.MountLoadout(GameSettings.instance.defaultLoadout);
    }

    [RegisterCommand(Help = "Toggle GodMode on Current Ship", MinArgCount = 0, MaxArgCount = 0)]
    static void God(CommandArg[] args)
    {
        GameSettings.pc.ship.health.Invulnerable = !GameSettings.pc.ship.health.Invulnerable;
        Log("Godmode : " + GameSettings.pc.ship.health.Invulnerable);
    }

    [RegisterCommand(Help = "Spawns a ship. Usage: spawn 'entity' 'times' 'possess?'", MinArgCount = 1, MaxArgCount = 3)]
    static void Spawn(CommandArg[] args)
    {
        string entity = args[0].String;

        if (Terminal.IssuedError) return;

        var spawnInfo = new ShipSpawnInfo();

        Ship spawnedShip = null;
        int timesToSpawn = 1;

        var ships = FindObjectsOfType<Ship>();

        switch (entity)
        {
            // case crazy
            // case nomad
            // case bship
            case "self":
                spawnInfo.ship = GameSettings.pc.ship;
                break;

            case "random":
                spawnInfo.ship = ships[Random.Range(0, ships.Length)];
                break;

            case "nearest":
                // FindNearestShip(ships)
                spawnInfo.ship = null;
                break;

            default:
                Log("Could not spawn entity of name " + entity);
                return;
        }

        if (args.Length > 1)
        {
            timesToSpawn = args[1].Int;
        }

        for (int i = 0; i < timesToSpawn; i++)
        {
            spawnedShip = ShipSpawner.SpawnShip(spawnInfo, GameSettings.pc.transform.position + GameSettings.pc.transform.forward * (10 + i));
        }

        if (args.Length > 2)
        {
            if (args[2].Bool)
            {
                GameSettings.pc.Possess(spawnedShip);
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
        var hp = GameSettings.pc.ship.hpSys;

        hp.energyCapacity = 1000000;
        hp.energy = 1000000;
        hp.chargeRate = 1000;

        Log("With great power...");
    }

    [RegisterCommand(Help = "Gives current ship unlimited afterburner energy", MinArgCount = 0, MaxArgCount = 0)]
    static void Impulse102(CommandArg[] args)
    {
        var abHardpoint = GameSettings.pc.ship.hpSys.afterburner;
        abHardpoint.stats.drainRate = abHardpoint.stats.drainRate == 0 ? 100 : 0;

        Log("Toggled infinite afterburner...");
    }

    [RegisterCommand(Help = "Unpossesses the current ship", MinArgCount = 0, MaxArgCount = 0)]
    static void Release(CommandArg[] args)
    {
        GameSettings.pc.Release();
        Log("Ship unpossessed");
    }

    [RegisterCommand(Name = "last", Help = "Repossesses the last ship", MinArgCount = 0, MaxArgCount = 0)]
    static void Repossesses(CommandArg[] args)
    {
        GameSettings.pc.Repossess();
        Log("Repossessed last ship");
    }

    [RegisterCommand(Help = "Reloads the current scene", MinArgCount = 0, MaxArgCount = 0)]
    static void Restart(CommandArg[] args)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Log("Reloading current scene...");
    }

    // Change this to apply to all speeds
    [RegisterCommand(Name = "throttle.power", Help = "Change the current ship's throttle power", MinArgCount = 1, MaxArgCount = 1)]
    static void SetThrottlePower(CommandArg[] args)
    {
        var newPower = Mathf.Clamp(args[0].Int, 0, 99999);
        GameSettings.pc.ship.engine.engineStats.enginePower = newPower;
    }

    [RegisterCommand(Name = "cruise.power", Help = "Change the current ship's cruise power", MinArgCount = 1, MaxArgCount = 1)]
    static void SetCruisePower(CommandArg[] args)
    {
        var newPower = Mathf.Clamp(args[0].Int, 0, 99999);
        GameSettings.pc.ship.cruiseEngine.stats.thrust = newPower;
    }

    [RegisterCommand(Name = "noti", Help = "Tests the notifaction system, spawns a test notification", MinArgCount = 0, MaxArgCount = 1)]
    static void NotificationTest(CommandArg[] args)
    {
        string text = "hello";

        if (args.Length > 0)
            text = args[0].String;

        FindObjectOfType<HUDManager>().SpawnNotification(text);
    }

    [RegisterCommand(MinArgCount = 0, MaxArgCount = 0)]
    static void Test()
    {
        //GameSettings.pc.GetComponent<StateController>().test
    }

    private Ship FindClosestShip(Ship[] ships)
    {
        ships.ToList().Remove(GameSettings.pc.ship);

        Ship closestShip = null;
        float closestShipDistance = 0;

        closestShip = ships[0];
        closestShipDistance = Vector3.Distance(GameSettings.pc.ship.transform.position, closestShip.transform.position);

        foreach (var ship in ships)
        {
            var dist = Vector3.Distance(GameSettings.pc.ship.transform.position, ship.transform.position);
            if (dist < closestShipDistance)
            {
                closestShip = ship;
                closestShipDistance = dist;
            }
        }

        return null;
    }
}
