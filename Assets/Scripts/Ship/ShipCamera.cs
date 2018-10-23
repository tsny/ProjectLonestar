using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShipCamera : ShipComponent
{
    public Transform transformToFollow;
    public bool isFollowingShip = true;

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
    public float speed;

    private void FixedUpdate()
    {
        CalculateOffsets();

        if (isFollowingShip) FollowShip();

        else
        {
            Vector3 newPosition = transformToFollow.position - (transformToFollow.forward * distanceOffset);
            transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
        } 
    }

    public static Vector3 GetMousePositionInWorld(Camera camera = null, bool drawRay = false, float aimRaycastDistance = 10000)
    {
        if (camera == null)
            camera = Camera.main;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (drawRay)
        {
            Debug.DrawRay(ray.origin, ray.direction * aimRaycastDistance);
        }

        Physics.Raycast(ray, out hitInfo, aimRaycastDistance, ~LayerMask.GetMask("Player"));

        if (hitInfo.collider != null)
        {
            return hitInfo.point;
        }

        else
        {
            return ray.GetPoint(aimRaycastDistance);
        }
    }

    public void CalculateOffsets()
    {
        var mouseCoords = PlayerController.GetMousePositionOnScreen();

        distanceOffset = Mathf.Clamp(speed / speedDivisor, 0, maxDistance);

        pitchOffset = Mathf.Clamp(mouseCoords.y * pitchModifier, maxLowerPitch, maxUpperPitch);
        yawOffset = Mathf.Clamp(mouseCoords.x * yawModifier, -maxYaw, maxYaw);
    }

    public void FollowShip()
    {
        Vector3 newPosition;

        newPosition = transformToFollow.position - (transformToFollow.forward * distanceOffset);
        newPosition = newPosition + (transformToFollow.up * pitchOffset);
        newPosition = newPosition + (transformToFollow.right * yawOffset);

        transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
        transform.rotation = transformToFollow.rotation;
    }
}
