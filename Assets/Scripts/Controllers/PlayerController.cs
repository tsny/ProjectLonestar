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

    public InputManager inputManager;

    [Header("--- Input ---")]
    public bool canPause = true;
    public bool inputAllowed = true;

    public MouseState mouseState;
    public float mouseX;
    public float mouseY;
    public float distanceFromCenter;

    // The amount of time that the controller waits until it 
    // determines the player is trying to switch to manual mouse flight
    public float mouseHoldDelay = .1f;

    private ShipCamera playerCamera;
    private ShipMovement shipMovement;

    public Ship controlledShip;

    public delegate void PossessionEventHandler(PossessionEventArgs args);
    public event PossessionEventHandler Possession;

    private void Awake()
    {
        enabled = false;
    }

    public void Possess(Ship newShip)
    {
        if (controlledShip != null) UnPossess();

        var oldShip = controlledShip;
        controlledShip = newShip;

        // TODO: Moves these responsibilities somewhere else
        playerCamera = controlledShip.GetComponentInChildren<ShipCamera>(true);
        shipMovement = controlledShip.GetComponent<ShipMovement>();

        playerCamera.enabled = true;
        playerCamera.pController = this;

        enabled = true;

        if (Possession != null) Possession(new PossessionEventArgs(newShip, oldShip, this));
    }

    public void UnPossess()
    {
        if (controlledShip == null) return;

        var oldShip = controlledShip;
        playerCamera.enabled = false;
        controlledShip = null;

        mouseState = MouseState.Off;

        enabled = false;

        if (Possession != null) Possession(new PossessionEventArgs(this, oldShip));
    }

    // TODO: Moves these first checks within Updates to some kind of on enabled check
    private void FixedUpdate()
    {
        switch (mouseState)
        {
            case MouseState.Off:
                shipMovement.LerpYawToNeutral();
                break;

            case MouseState.Toggled:
            case MouseState.Held:
                shipMovement.Pitch(mouseY);
                shipMovement.Yaw(mouseX);
                break;
        }
    }

    private void Update()
    {
        if (PossessingPawn == false)
        {
            enabled = false;
            return;
        }

        GetMousePosition();

        controlledShip.AimPosition = playerCamera.GetAimPosition();

        if(inputAllowed)
        {
            #region movement
            if(Input.GetKey(InputManager.instance.ThrottleUp))
            {
                shipMovement.ThrottleUp();
            }

            if(Input.GetKey(InputManager.instance.ThrottleDown))
            {
                shipMovement.ThrottleDown();
            }

            if(Input.GetKey(InputManager.instance.StrafeLeft))
            {
                shipMovement.ChangeStrafe(-1);
            }

            if(Input.GetKey(InputManager.instance.StrafeRight))
            {
                shipMovement.ChangeStrafe(1);
            }

            // If neither strafe key is pressed, reset the ship's strafing
            if(!Input.GetKey(InputManager.instance.StrafeRight) && !Input.GetKey(InputManager.instance.StrafeLeft))
            {
                shipMovement.ChangeStrafe(0);
            }         

            if(Input.GetKeyDown(InputManager.instance.ToggleMouseFlight))
            {
                ToggleMouseFlight();
            }

            if(Input.GetKeyDown(InputManager.instance.Afterburner))
            {
                controlledShip.hardpointSystem.ToggleAfterburner(true);
            }
            
            if(Input.GetKeyUp(InputManager.instance.Afterburner))
            {
                controlledShip.hardpointSystem.ToggleAfterburner(false);
            }

            if(Input.GetKeyDown(InputManager.instance.ManualMouseFlight))
            {
                if(mouseState == MouseState.Off) StartCoroutine("ManualMouseFlightCoroutine");
            }

            if(Input.GetKeyUp(InputManager.instance.ManualMouseFlight))
            {
                StopAllCoroutines();

                if (mouseState == MouseState.Held) mouseState = MouseState.Off;
            }

            if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
            {
                shipMovement.ToggleCruiseEngines();
            }

            if(Input.GetKeyDown(InputManager.instance.KillEngines))
            {
                shipMovement.Drift();
            }

            #endregion

            #region hardpoints
            if(Input.GetKey(InputManager.instance.Hardpoint1))
            {
                controlledShip.hardpointSystem.FireHardpoint(1);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint2))
            {
                controlledShip.hardpointSystem.FireHardpoint(2);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint3))
            {
                controlledShip.hardpointSystem.FireHardpoint(3);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint4))
            {
                controlledShip.hardpointSystem.FireHardpoint(4);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint5))
            {
                controlledShip.hardpointSystem.FireHardpoint(5);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint6))
            {
                controlledShip.hardpointSystem.FireHardpoint(6);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint7))
            {
                controlledShip.hardpointSystem.FireHardpoint(7);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint8))
            {
                controlledShip.hardpointSystem.FireHardpoint(8);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint9))
            {
                controlledShip.hardpointSystem.FireHardpoint(9);
            }

            if(Input.GetKey(InputManager.instance.Hardpoint10))
            {
                controlledShip.hardpointSystem.FireHardpoint(10);
            }
            #endregion

            if(Input.GetKey(InputManager.instance.Fire))
            {
                controlledShip.hardpointSystem.FireActiveWeapons();
            }

            if (Input.GetKeyDown(InputManager.instance.LootAll))
            {
                controlledShip.hardpointSystem.tractorHardpoint.TractorAllLoot();
            }
        }

        if(Input.GetKeyDown(InputManager.instance.PauseGame) && canPause)
        {
            GameManager.instance.TogglePause();
        }
    }

    public void ToggleMouseFlight()
    {
        switch(mouseState)
        {
            case MouseState.Off:
                mouseState = MouseState.Toggled;
                break;

            case MouseState.Toggled:
                mouseState = MouseState.Off;
                break;

            case MouseState.Held:
                mouseState = MouseState.Toggled;
                break;
        }
    }

    public void GetMousePosition()
    {
        int width = Screen.width;
        int height = Screen.height;

        Vector2 center = new Vector2(width / 2, height / 2);
        Vector3 mousePosition = Input.mousePosition;

        // Shifts the origin of the screen to be in the middle instead of the bottom left corner

        mouseX = mousePosition.x - center.x;
        mouseY = mousePosition.y - center.y;

        mouseX = Mathf.Clamp(mouseX / center.x, -1, 1);
        mouseY = Mathf.Clamp(mouseY / center.y, -1, 1);

        distanceFromCenter = Vector2.Distance(Vector2.zero, new Vector2(mouseX, mouseY));
    }

    IEnumerator ManualMouseFlightCoroutine()
    {
        yield return new WaitForSeconds(mouseHoldDelay);

        mouseState = MouseState.Held;
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




