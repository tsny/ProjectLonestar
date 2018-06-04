using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("--- Input ---")]
    public bool canPause = true;
    public bool inputAllowed = true;

    public MouseState mouseState;
    public float mouseX;
    public float mouseY;
    public float distanceFromCenter;

    private ShipCamera playerCamera;
    private ShipMovement shipMovement;

    public Ship controlledShip;

    public delegate void PossessedShipHandler(PlayerController sender, PossessionEventArgs e);
    public event PossessedShipHandler ShipPossessed;

    public void Possess(Ship newShip)
    {
        if (controlledShip != null) UnPossess();

        Ship oldShip = controlledShip;

        controlledShip = newShip;

        controlledShip.name = "Player Ship";
        controlledShip.tag = "Player";

        controlledShip.dustParticleSystem.gameObject.SetActive(true);

        controlledShip.hardpointSystem.shieldHardpoint.gameObject.layer = LayerMask.NameToLayer("Player");

        playerCamera = controlledShip.GetComponentInChildren<ShipCamera>();
        shipMovement = controlledShip.GetComponent<ShipMovement>();

        playerCamera.enabled = true;
        playerCamera.pController = this;

        if (ShipPossessed != null) ShipPossessed(this, new PossessionEventArgs(newShip, oldShip));
    }

    public void UnPossess()
    {
        playerCamera.enabled = false;

        controlledShip.dustParticleSystem.gameObject.SetActive(false);

        controlledShip.tag = "Untagged";

        controlledShip.hardpointSystem.shieldHardpoint.gameObject.layer = 0;
    }

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
        GetMousePosition();

        controlledShip.AimPosition = playerCamera.GetAimPosition();

        if(inputAllowed)
        {
            #region movement
            if(Input.GetKey(GameManager.gm.ThrottleUp))
            {
                shipMovement.ThrottleUp();
            }

            if(Input.GetKey(GameManager.gm.ThrottleDown))
            {
                shipMovement.ThrottleDown();
            }

            if(Input.GetKey(GameManager.gm.StrafeLeft))
            {
                shipMovement.ChangeStrafe(-1);
            }

            if(Input.GetKey(GameManager.gm.StrafeRight))
            {
                shipMovement.ChangeStrafe(1);
            }

            // If neither strafe key is pressed, reset the ship's strafing
            if(!Input.GetKey(GameManager.gm.StrafeRight) && !Input.GetKey(GameManager.gm.StrafeLeft))
            {
                shipMovement.ChangeStrafe(0);
            }         

            if(Input.GetKeyDown(GameManager.gm.ToggleMouseFlight))
            {
                ToggleMouseFlight();
            }

            if(Input.GetKeyDown(GameManager.gm.Afterburner))
            {
                controlledShip.hardpointSystem.ToggleAfterburner(true);
            }
            
            if(Input.GetKeyUp(GameManager.gm.Afterburner))
            {
                controlledShip.hardpointSystem.ToggleAfterburner(false);
            }

            if(Input.GetKeyDown(GameManager.gm.PauseGame) && canPause)
            {
                GameManager.gm.TogglePause();
            }

            if(Input.GetKeyDown(GameManager.gm.ManualMouseFlight))
            {
                if(mouseState == MouseState.Off)
                {
                    mouseState = MouseState.Held;
                }
            }

            if(Input.GetKeyUp(GameManager.gm.ManualMouseFlight) && mouseState == MouseState.Held)
            {
                mouseState = MouseState.Off;
            }

            if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
            {
                shipMovement.ToggleCruiseEngines();
            }

            if(Input.GetKeyDown(GameManager.gm.KillEngines))
            {
                shipMovement.Drift();
            }

            #endregion

            #region hardpoints
            if(Input.GetKey(GameManager.gm.Hardpoint1))
            {
                controlledShip.hardpointSystem.FireHardpoint(1);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint2))
            {
                controlledShip.hardpointSystem.FireHardpoint(2);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint3))
            {
                controlledShip.hardpointSystem.FireHardpoint(3);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint4))
            {
                controlledShip.hardpointSystem.FireHardpoint(4);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint5))
            {
                controlledShip.hardpointSystem.FireHardpoint(5);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint6))
            {
                controlledShip.hardpointSystem.FireHardpoint(6);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint7))
            {
                controlledShip.hardpointSystem.FireHardpoint(7);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint8))
            {
                controlledShip.hardpointSystem.FireHardpoint(8);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint9))
            {
                controlledShip.hardpointSystem.FireHardpoint(9);
            }

            if(Input.GetKey(GameManager.gm.Hardpoint10))
            {
                controlledShip.hardpointSystem.FireHardpoint(10);
            }
            #endregion

            if(Input.GetKey(GameManager.gm.Fire))
            {
                controlledShip.hardpointSystem.FireActiveWeapons();
            }

            if (Input.GetKeyDown(GameManager.gm.LootAll))
            {
                controlledShip.hardpointSystem.tractorHardpoint.TractorAllLoot();
            }
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

        // shifts the origin of the screen to be in the middle instead of the bottom left corner

        mouseX = mousePosition.x - center.x;
        mouseY = mousePosition.y - center.y;

        mouseX = Mathf.Clamp(mouseX / center.x, -1, 1);
        mouseY = Mathf.Clamp(mouseY / center.y, -1, 1);

        distanceFromCenter = Vector2.Distance(Vector2.zero, new Vector2(mouseX, mouseY));
    }
}

public class PossessionEventArgs : EventArgs
{
    public Ship newShip;
    public Ship oldShip;

    public PossessionEventArgs(Ship newShip, Ship oldShip)
    {
        this.newShip = newShip;
        this.oldShip = oldShip;
    }
}




