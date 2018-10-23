using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShipCamera : ShipComponent
{
    public Transform transformToFollow;
    public bool calculateRotationOffsets;
    private Rigidbody rb;

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
    public float Speed
    {
        get
        {
            return (rb != null) ? rb.velocity.magnitude : 0;
        }
    }

    public void SetTarget(Transform newTransform)
    {
        transformToFollow = newTransform;
        rb = newTransform.GetComponentInChildren<Rigidbody>();
        enabled = true;
    }

    public void ClearTarget()
    {
        enabled = false;
        transformToFollow = null;
    }

    private void FixedUpdate()
    {
        CalculateOffsets();
        FollowTarget();
    }

    public static Vector3 GetMousePositionInWorld(Camera camera = null, bool drawRay = false, float aimRaycastDistance = 10000)
    {
        if (camera == null)
            camera = Camera.main;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (drawRay) Debug.DrawRay(ray.origin, ray.direction * aimRaycastDistance);

        Physics.Raycast(ray, out hitInfo, aimRaycastDistance, ~LayerMask.GetMask("Player"));

        return (hitInfo.collider != null) ? hitInfo.point : ray.GetPoint(aimRaycastDistance);
    }

    private void CalculateOffsets()
    {
        var mouseCoords = PlayerController.GetMousePositionOnScreen();

        distanceOffset = Mathf.Clamp(Speed / speedDivisor, 0, maxDistance);

        if (calculateRotationOffsets)
        {
            pitchOffset = Mathf.Clamp(mouseCoords.y * pitchModifier, maxLowerPitch, maxUpperPitch);
            yawOffset = Mathf.Clamp(mouseCoords.x * yawModifier, -maxYaw, maxYaw);
        }

        else
        {
            pitchOffset = 0;
            yawOffset = 0;
        }
    }

    private void FollowTarget()
    {
        Vector3 newPosition;

        newPosition = transformToFollow.position - (transformToFollow.forward * distanceOffset);
        newPosition = newPosition + (transformToFollow.up * pitchOffset);
        newPosition = newPosition + (transformToFollow.right * yawOffset);

        transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
        transform.rotation = transformToFollow.rotation;
    }
}
