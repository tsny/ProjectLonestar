using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool PossessingPawn
    {
        get
        {
            return controlledShip != null;
        }
    }

    [Header("--- Input ---")]
    public bool canPause = true;
    public bool inputAllowed = true;

    private MouseState mouseState;
    public MouseState MouseState
    {
        get
        {
            return mouseState;
        }

        set
        {
            mouseState = value;
            if (MouseStateChanged != null) MouseStateChanged(MouseState);
        }
    }

    // The amount of time that the controller waits until it 
    // determines the player is trying to switch to manual mouse flight
    public float mouseHoldDelay = .1f;
    public Ship controlledShip;
    public GameObject hudPrefab;

    public delegate void PossessionEventHandler(PossessionEventArgs args);
    public event PossessionEventHandler Possession;

    public delegate void MouseStateEventHandler(MouseState state);
    public event MouseStateEventHandler MouseStateChanged;

    private void Awake()
    {
        name = "PLAYER CONTROLLER";
    }

    public Ship SpawnPlayer(GameObject shipPrefab, Loadout loadout)
    {
        controlledShip = ShipSpawner.SpawnShip(shipPrefab, Vector3.zero, loadout);
        return controlledShip;
    }

    public HUDManager SpawnHUD()
    {
        var hud = FindObjectOfType<HUDManager>();

        if (hud == null)
        {
            hud = Instantiate(GameSettings.Instance.HUDPrefab).GetComponent<HUDManager>();
        }

        hud.InitializeHUD(this);

        return hud;
    }

    public void Possess(Ship newShip)
    {
        if (newShip == null)
        {
            Eject();
            return;
        }

        var oldShip = controlledShip;

        if (oldShip != null)
        {
            oldShip.SetPossessed(this, false);
        }

        controlledShip = newShip;

        newShip.SetPossessed(this, true);

        if (Possession != null) Possession(new PossessionEventArgs(newShip, oldShip, this));

        enabled = true;
    }

    public void Eject()
    {
        controlledShip.SetPossessed(this, false);
        controlledShip = null;
        MouseState = MouseState.Off;
        enabled = false;
        Instantiate(GameSettings.Instance.flycamPrefab);
    }

    private void Update()
    {
        GetMousePosition();

        if(inputAllowed)
        {
            #region movement
            if(Input.GetKey(InputManager.ThrottleUpKey) || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                controlledShip.engine.ThrottleUp();
            }

            if(Input.GetKey(InputManager.ThrottleDownKey) || Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                controlledShip.engine.ThrottleDown();
            }

            if(Input.GetKey(InputManager.StrafeLeftKey))
            {
                controlledShip.engine.Strafe = -1; 
            }

            if(Input.GetKey(InputManager.StrafeRightKey))
            {
                controlledShip.engine.Strafe = 1; 
            }

            // If neither strafe key is pressed, reset the ship's strafing
            if(!Input.GetKey(InputManager.StrafeRightKey) && !Input.GetKey(InputManager.StrafeLeftKey))
            {
                controlledShip.engine.Strafe = 0; 
            }         

            if(Input.GetKeyDown(InputManager.ToggleMouseFlightKey))
            {
                ToggleMouseFlight();
            }

            if(Input.GetKeyDown(InputManager.AfterburnerKey))
            {
                controlledShip.hardpointSystem.ToggleAfterburner(true);
            }
            
            else if(Input.GetKeyUp(InputManager.AfterburnerKey))
            {
                controlledShip.hardpointSystem.ToggleAfterburner(false);
            }

            if(Input.GetKeyDown(InputManager.ManualMouseFlightKey))
            {
                if(MouseState == MouseState.Off) StartCoroutine("ManualMouseFlightCoroutine");
            }

            if(Input.GetKeyUp(InputManager.ManualMouseFlightKey))
            {
                StopAllCoroutines();

                if (MouseState == MouseState.Held) MouseState = MouseState.Off;
            }

            if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(InputManager.ThrottleUpKey))
            {
                controlledShip.cruiseEngine.ToggleCruiseEngines();
            }

            if(Input.GetKeyDown(InputManager.KillEnginesKey))
            {
                controlledShip.engine.ToggleDrifting();
            }

            #endregion

            #region hardpoints
            if(Input.GetKey(InputManager.Hardpoint1Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(1);
            }

            if(Input.GetKey(InputManager.Hardpoint2Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(2);
            }

            if(Input.GetKey(InputManager.Hardpoint3Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(3);
            }

            if(Input.GetKey(InputManager.Hardpoint4Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(4);
            }

            if(Input.GetKey(InputManager.Hardpoint5Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(5);
            }

            if(Input.GetKey(InputManager.Hardpoint6Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(6);
            }

            if(Input.GetKey(InputManager.Hardpoint7Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(7);
            }

            if(Input.GetKey(InputManager.Hardpoint8Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(8);
            }

            if(Input.GetKey(InputManager.Hardpoint9Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(9);
            }

            if(Input.GetKey(InputManager.Hardpoint10Key))
            {
                controlledShip.hardpointSystem.FireWeaponHardpoint(10);
            }

            if(Input.GetKey(InputManager.FireKey))
            {
                controlledShip.hardpointSystem.FireActiveWeapons();
            }

            if (Input.GetKeyDown(InputManager.LootAllKey))
            {
                controlledShip.hardpointSystem.tractorHardpoint.TractorAllLoot();
            }

            #endregion
        }

        if(Input.GetKeyDown(InputManager.PauseGameKey) && canPause)
        {
            GameStateUtils.TogglePause();
        }
    }

    private void FixedUpdate()
    {
        var engine = controlledShip.engine;

        switch (mouseState)
        {
            case MouseState.Off:
                engine.LerpYawToNeutral();
                break;

            case MouseState.Toggled:
            case MouseState.Held:
                engine.Pitch(GetMousePosition().y);
                engine.Yaw(GetMousePosition().x);
                break;

            default:
                break;
        }
    }

    public void ToggleMouseFlight()
    {
        switch(MouseState)
        {
            case MouseState.Off:
                MouseState = MouseState.Toggled;
                break;

            case MouseState.Toggled:
                MouseState = MouseState.Off;
                break;

            case MouseState.Held:
                MouseState = MouseState.Toggled;
                break;
        }
    }

    public static Vector2 GetMousePosition()
    {
        int width = Screen.width;
        int height = Screen.height;

        Vector2 center = new Vector2(width / 2, height / 2);
        Vector3 mousePosition = Input.mousePosition;

        // Shifts the origin of the screen to be in the middle instead of the bottom left corner

        var mouseX = mousePosition.x - center.x;
        var mouseY = mousePosition.y - center.y;

        mouseX = Mathf.Clamp(mouseX / center.x, -1, 1);
        mouseY = Mathf.Clamp(mouseY / center.y, -1, 1);

        return new Vector2(mouseX, mouseY);
    }

    IEnumerator ManualMouseFlightCoroutine()
    {
        yield return new WaitForSeconds(mouseHoldDelay);

        MouseState = MouseState.Held;
    }

    public void SpawnFlyCam(Vector3 pos)
    {
        RemoveFlycamFromScene();
        var flyCam = Instantiate(GameSettings.Instance.flycamPrefab);
        flyCam.transform.position = pos + new Vector3(0, 10, 0);
    }

    // Can only be one fly cam in the scene

    public void RemoveFlycamFromScene()
    {
        var flyCam = FindObjectOfType<Flycam>();
        if (flyCam != null) Destroy(flyCam.gameObject);
    }
}

public class PossessionEventArgs : EventArgs
{
    public PlayerController playerController;
    public Ship newShip;
    public Ship oldShip;

    public bool PossessingNewShip
    {
        get
        {
            return newShip != null;
        }
    }

    public PossessionEventArgs(Ship newShip, Ship oldShip, PlayerController playerController)
    {
        this.newShip = newShip;
        this.oldShip = oldShip;
        this.playerController = playerController;
    }

    public PossessionEventArgs(PlayerController playerController, Ship oldShip)
    {
        this.playerController = playerController;
        this.oldShip = oldShip;
    }
}




