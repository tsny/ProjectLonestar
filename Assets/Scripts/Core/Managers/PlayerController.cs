using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera), typeof(AudioListener), typeof(ShipCamera))]
public class PlayerController : MonoBehaviour
{
    [Header("--- Input ---")]
    public bool canPause = true;
    public bool inputAllowed = true;
    public float doubleTapDuration = 1;

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
            UpdateShipCameraState();
            if (MouseStateChanged != null) MouseStateChanged(MouseState);
        }
    }

    [Tooltip("How long until manual flight is engaged after hold")]
    public float mouseHoldDelay = .1f;

    public Ship ship;
    public Ship lastShip;
    public ShipCamera shipCamera;
    public Flycam flycam;
    public Camera cam;
    public PostProcessLayer ppl;
    public AudioListener listener;
    public float mouseRaycastDistance = 1000;

    public AimPosition CurrentAimPosition {get; set;}

    public delegate void PossessionEventHandler(PlayerController sender, PossessionEventArgs args);
    public event PossessionEventHandler PossessedNewShip;
    public event PossessionEventHandler ReleasedShip;

    public delegate void MouseStateEventHandler(MouseState state);
    public event MouseStateEventHandler MouseStateChanged;

    protected void Awake()
    {
        if (FindObjectsOfType<PlayerController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        name = "PLAYER CONTROLLER";

        cam = Utilities.CheckComponent<Camera>(gameObject);
        shipCamera = Utilities.CheckComponent<ShipCamera>(gameObject);
        flycam = Utilities.CheckComponent<Flycam>(gameObject);
        listener = Utilities.CheckComponent<AudioListener>(gameObject);

        enabled = false;
    }

    protected virtual void OnPossessedNewShip(PossessionEventArgs args)
    {
        if (PossessedNewShip != null) PossessedNewShip(this, args);
    }

    protected virtual void OnReleasedShip(Ship releasedShip)
    {
        if (ReleasedShip != null) ReleasedShip(this, new PossessionEventArgs(null, releasedShip));
    }

    public void Possess(Ship newShip)
    {
        if (newShip == null)
        {
            Release();
            return;
        }

        var oldShip = ship;

        if (oldShip != null)
        {
            oldShip.SetPossessed(this, false);
        }

        newShip.SetPossessed(this, true);

        flycam.enabled = false;
        shipCamera.SetTarget(newShip.cameraPosition);

        newShip.GetComponent<StateController>().currentState = null;
        newShip.Died += HandleShipDied;

        ship = newShip;
        lastShip = null;
        enabled = true;
        OnPossessedNewShip(new PossessionEventArgs(ship, null));
    }

    private void HandleShipDied(Ship ship)
    {
        Release();
    }

    public void Release()
    {
        ship.SetPossessed(this, false);
        shipCamera.ClearTarget();
        ship.Died -= HandleShipDied;

        MouseState = MouseState.Off;
        flycam.enabled = true;
        enabled = false;

        lastShip = ship;
        ship = null;
        OnReleasedShip(ship);
    }

    public void Repossess()
    {
        if (lastShip != null)
        {
            Possess(lastShip);
        }
    }

    private void Update()
    {
        CurrentAimPosition = GetCurrentAimPosition();

        if(Input.GetKeyDown(InputManager.PauseGameKey) && canPause)
        {
            GameStateUtils.TogglePause();
        }

        if(!inputAllowed || ship == null) return;

        #region movement
        if(Input.GetKey(InputManager.ThrottleUpKey) || Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ship.engine.ThrottleUp();
        }

        else if(Input.GetKeyUp(InputManager.ThrottleUpKey))
        {
            Action action = ship.engine.Blink;
            StartCoroutine(DoubleTap(InputManager.ThrottleUpKey, action));
        }

        if(Input.GetKey(InputManager.ThrottleDownKey) || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ship.engine.ThrottleDown();
        }

        if(Input.GetKey(InputManager.StrafeLeftKey))
        {
            ship.engine.Strafe = -1; 
        }
        else if (Input.GetKeyUp(InputManager.StrafeLeftKey))
        {
            Action action = ship.engine.SidestepLeft;
            StartCoroutine(DoubleTap(InputManager.StrafeLeftKey, action));
        }

        if(Input.GetKey(InputManager.StrafeRightKey))
        {
            ship.engine.Strafe = 1; 
        }
        else if (Input.GetKeyUp(InputManager.StrafeRightKey))
        {
            Action action = ship.engine.SidestepRight;
            StartCoroutine(DoubleTap(InputManager.StrafeRightKey, action));
        }

        // If neither strafe key is pressed, reset the ship's strafing
        if(!Input.GetKey(InputManager.StrafeRightKey) && !Input.GetKey(InputManager.StrafeLeftKey))
        {
            ship.engine.Strafe = 0; 
        }         

        if(Input.GetKeyDown(InputManager.ToggleMouseFlightKey))
        {
            ToggleMouseFlight();
        }

        if(Input.GetKeyDown(InputManager.AfterburnerKey))
        {
            ship.hpSys.ToggleAfterburner(true);
        }
        
        else if(Input.GetKeyUp(InputManager.AfterburnerKey))
        {
            ship.hpSys.ToggleAfterburner(false);
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
            ship.cruiseEngine.ToggleCruiseEngines();
        }

        if(Input.GetKeyDown(InputManager.KillEnginesKey))
        {
            ship.engine.Drifting = !ship.engine.Drifting;
        }

        #endregion

        #region hardpoints
        if(Input.GetKey(InputManager.Hardpoint1Key))
        {
            ship.hpSys.FireWeaponHardpoint(1, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint2Key))
        {
            ship.hpSys.FireWeaponHardpoint(2, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint3Key))
        {
            ship.hpSys.FireWeaponHardpoint(3, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint4Key))
        {
            ship.hpSys.FireWeaponHardpoint(4, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint5Key))
        {
            ship.hpSys.FireWeaponHardpoint(5, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint6Key))
        {
            ship.hpSys.FireWeaponHardpoint(6, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint7Key))
        {
            ship.hpSys.FireWeaponHardpoint(7, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint8Key))
        {
            ship.hpSys.FireWeaponHardpoint(8, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint9Key))
        {
            ship.hpSys.FireWeaponHardpoint(9, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.Hardpoint10Key))
        {
            ship.hpSys.FireWeaponHardpoint(10, CurrentAimPosition);
        }

        if(Input.GetKey(InputManager.FireKey))
        {
            ship.hpSys.FireActiveWeapons(CurrentAimPosition);
        }

        if (Input.GetKeyDown(InputManager.LootAllKey))
        {
            ship.hpSys.tractorBeam.TractorAllLoot();
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (ship == null || ship.engine == null) return;

        var engine = ship.engine;

        switch (mouseState)
        {
            case MouseState.Off:
                engine.LerpYawToNeutral();
                break;

            case MouseState.Toggled:
            case MouseState.Held:
                engine.AddPitch(GameStateUtils.GetMousePositionOnScreen().y);
                engine.AddYaw(GameStateUtils.GetMousePositionOnScreen().x);
                break;

            default:
                Debug.LogWarning("MouseState has incorrect value...");
                break;
        }
    }

    // Maybe make those ship cam changes events
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

    private void UpdateShipCameraState()
    {
        if (shipCamera == null) return;

        switch (MouseState)
        {
            case MouseState.Off:
                shipCamera.calculateRotationOffsets = false;
                break;

            case MouseState.Toggled:
            case MouseState.Held:
                shipCamera.calculateRotationOffsets = true;
                break;
        }
    }

    IEnumerator ManualMouseFlightCoroutine()
    {
        yield return new WaitForSeconds(mouseHoldDelay);

        MouseState = MouseState.Held;
    }

    IEnumerator DoubleTap(KeyCode pressedKey, Action action)
    {
        float elapsed = 0;

        while (elapsed < doubleTapDuration)
        {
            if (Input.GetKey(pressedKey))
            {
                action();
                yield break;
            }
        
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private AimPosition GetCurrentAimPosition()
    {
        if (Time.time < 3) return null;

        var sim = (CustomStandaloneInputModule) EventSystem.current.currentInputModule;
        var go = sim.GetPointerData().pointerCurrentRaycast.gameObject;

        if (go != null)
        {
            if (go.CompareTag("TargetReticle"))
            {
                var indicator = go.GetComponent<TargetReticle>().indicator;
                if (indicator.targetRb != null)
                {
                    return new AimPosition(indicator.targetRb);
                }
            }
        }

        return AimPosition.FromMouse(cam, false, mouseRaycastDistance);
    }
}

public class PossessionEventArgs : EventArgs
{
    public Ship newShip;
    public Ship oldShip;

    public bool PossessingNewShip
    {
        get
        {
            return newShip != null;
        }
    }

    public PossessionEventArgs(Ship newShip, Ship oldShip)
    {
        this.newShip = newShip;
        this.oldShip = oldShip;
    }
}