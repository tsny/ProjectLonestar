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

    public ShipPhysics shipPhysics;
    public PlayerController pController;
    public Camera shipCam;
    public AudioListener audioListener;

    protected override void HandlePossession(Ship sender, bool possessed)
    {
        base.HandlePossession(sender, possessed);

        enabled = possessed;
    }

    private void FixedUpdate()
    {
        CalculateOffsets();
        owningShip.AimPosition = GetAimPosition();

        if (pController.mouseState == MouseState.Held || pController.mouseState == MouseState.Toggled)
        {
            FollowShip();
        }

        else
        {
            Vector3 newPosition = transform.parent.position - (transform.parent.forward * distanceOffset);
            transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
        } 
    }

    protected override void Awake()
    {
        base.Awake();

        shipCam = GetComponent<Camera>();
        audioListener = GetComponent<AudioListener>();
        shipPhysics = owningShip.GetComponentInParent<ShipPhysics>();

        enabled = false;
    }

    private void OnEnable()
    {
        shipCam.enabled = true;
        audioListener.enabled = true;
    }

    private void OnDisable()
    {
        pController = null;
        shipCam.enabled = false;
        audioListener.enabled = false;
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
        distanceOffset = Mathf.Clamp(shipPhysics.speed / speedDivisor, 0, maxDistance);
        pitchOffset = Mathf.Clamp(pController.mouseY * pitchModifier, maxLowerPitch, maxUpperPitch);
        yawOffset = Mathf.Clamp(pController.mouseX * yawModifier, -maxYaw, maxYaw);
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
