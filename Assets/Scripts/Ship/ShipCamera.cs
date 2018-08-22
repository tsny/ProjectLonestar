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

    private ShipPhysics shipPhysics;
    public ShipController shipController;
    public Camera shipCam;
    public AudioListener listener;

    private void FixedUpdate()
    {
        if (shipController == null) return;

        CalculateOffsets();

        if (shipController.mouseState == MouseState.Held || shipController.mouseState == MouseState.Toggled)
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

        shipCam = GetComponentInChildren<Camera>(true);
        listener = GetComponent<AudioListener>();

        shipPhysics = owningShip.GetComponentInParent<ShipPhysics>();

        enabled = false;
    }

    public void Init(ShipController controller)
    {
        enabled = true;
        shipController = controller;
    }

    private void OnEnable()
    {
        shipCam.enabled = true;
        listener.enabled = true;
    }

    private void OnDisable()
    {
        shipController = null;
        shipCam.enabled = false;
        listener.enabled = false;
    }

    public Vector3 GetAimPosition()
    {
        Ray ray = shipCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.direction * 100);

        Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~LayerMask.GetMask("Player"));

        if (hitInfo.collider != null)
        {
            return hitInfo.collider.transform.position;
        }

        return ray.GetPoint(10000);
    }

    public void CalculateOffsets()
    {
        distanceOffset = Mathf.Clamp(shipPhysics.speed / speedDivisor, 0, maxDistance);
        pitchOffset = Mathf.Clamp(shipController.mouseY * pitchModifier, maxLowerPitch, maxUpperPitch);
        yawOffset = Mathf.Clamp(shipController.mouseX * yawModifier, -maxYaw, maxYaw);
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
