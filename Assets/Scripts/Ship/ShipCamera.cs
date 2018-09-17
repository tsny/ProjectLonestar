using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShipCamera : ShipComponent
{
    [Header("Offsets")]
    [Space(5)]
    public float yawOffset;
    public float pitchOffset;
    public float distanceOffset;

    // Use these to increase the possible range of yaw/pitch offsets.
    public float pitchModifier = 1;
    public float yawModifier = 1;

    [Header("Lerp")]
    [Space(5)]

    public float speedDivisor = 20;
    public float lerpSpeed = .2f;

    [Header("Maxes")]
    [Space(5)]
    public float maxYaw = 10;
    public float maxUpperPitch = 10;
    public float maxLowerPitch = -10;
    public float maxDistance = 10;

    [Header("Other")]
    [Space(5)]
    public float aimRaycastDistance = 10000;

    public PlayerController playerController;
    public Engine engine;

    private Camera shipCam;
    private AudioListener audioListener;

    private void Awake()
    {
        shipCam = GetComponent<Camera>();
        audioListener = GetComponent<AudioListener>();
        enabled = false;
    }

    private void OnEnable()
    {
        shipCam.enabled = true;
        audioListener.enabled = true;
    }

    private void OnDisable()
    {
        shipCam.enabled = false;
        audioListener.enabled = false;
    }

    public override void InitShipComponent(Ship sender, ShipStats stats)
    {
        base.InitShipComponent(sender, stats);
        sender.Possession += HandleOwnerPossessed;
        engine = sender.engine;
    }

    private void HandleOwnerPossessed(PlayerController pc, Ship sender, bool possessed)
    {
        playerController = pc;
        enabled = possessed;
    }

    private void FixedUpdate()
    {
        CalculateOffsets();
        owningShip.aimPosition = GetAimPosition();

        if (playerController.MouseState != MouseState.Off)
        {
            FollowShip();
        }

        else
        {
            Vector3 newPosition = transform.parent.position - (transform.parent.forward * distanceOffset);
            transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
        } 
    }

    public Vector3 GetAimPosition()
    {
        Ray ray = shipCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.direction * aimRaycastDistance);

        Physics.Raycast(ray, out hitInfo, aimRaycastDistance, ~LayerMask.GetMask("Player"));

        if (hitInfo.collider != null)
        {
            return hitInfo.collider.transform.position;
        }

        else
        {
            return ray.GetPoint(aimRaycastDistance);
        }
    }

    public void CalculateOffsets()
    {
        var mouseCoords = PlayerController.GetMousePosition();

        distanceOffset = Mathf.Clamp(engine.Speed / speedDivisor, 0, maxDistance);

        pitchOffset = Mathf.Clamp(mouseCoords.y * pitchModifier, maxLowerPitch, maxUpperPitch);
        yawOffset = Mathf.Clamp(mouseCoords.x * yawModifier, -maxYaw, maxYaw);
    }

    public void FollowShip()
    {
        Vector3 newPosition;

        newPosition = transform.parent.position - (transform.parent.forward * distanceOffset);
        newPosition = newPosition + (transform.parent.up * pitchOffset);
        newPosition = newPosition + (transform.parent.right * yawOffset);

        transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
    }
}
