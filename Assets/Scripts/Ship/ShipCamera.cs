using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShipCamera : ShipComponent
{
    public Transform transformToFollow;

    public bool calculateRotationOffsets;
    private Rigidbody rb;

    [Header("Offsets")] [Space(5)]
    public float yawOffset;
    public float pitchOffset;
    public float distanceOffset;

    public LayerMask targetableLayers;

    // Use these to increase the possible range of yaw/pitch offsets.
    public float pitchModifier = 5;
    public float yawModifier = 5;

    [Header("Lerp")] [Space(5)]

    public float speedDivisor = 20;
    public float lerpSpeed = .2f;

    [Header("Maxes")] [Space(5)]
    public float maxYaw = 10;
    public float maxUpperPitch = 1;
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
        if (transformToFollow == null)
        {
            //Debug.LogWarning("ShipCam has no transform to follow... Disabling...");
            //enabled = false;
            return;
        }

        CalculateOffsets();
        FollowTarget();
    }

    [Obsolete]
    public static AimPosition GetMousePositionInWorld(Camera camera = null, bool drawRay = false, float castDistance = 10000)
    {
        // TODO: Don't use camera.main at all?
        if (camera == null)
        {
            camera = Camera.main;
            Debug.LogWarning("Using Camera.main in ShipCamera!");
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (drawRay) Debug.DrawRay(ray.origin, ray.direction * castDistance);

        //Physics.Raycast(ray, out hitInfo, aimRaycastDistance, ~LayerMask.GetMask("Player"));
        // TODO: Change that layer to "Targetable"?
        Physics.Raycast(ray, out hit, castDistance, 1 << LayerMask.NameToLayer("Default"));

        var pos = (hit.collider != null) ? hit.point : ray.GetPoint(castDistance);

        return new AimPosition(pos, hit);
    }

    private void CalculateOffsets()
    {
        var mouseCoords = GameStateUtils.GetMousePositionOnScreen();

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

// TODO: Move this to it's own file
public class AimPosition
{
    public Vector3 pos;
    public Rigidbody rb;

    public RaycastHit hit;
    public bool HitAnything { get { return hit.transform != null; } }
    public bool HitTargetReticle { get { return hit.transform.CompareTag("TargetReticle"); } }
    public TargetIndicator TargetIndicator { get { return hit.transform.GetComponent<TargetIndicator>(); } }
    public Rigidbody TargetRb { get { return hit.rigidbody; } }

    public AimPosition() {}
    public AimPosition(Vector3 pos) { this.pos = pos; }
    public AimPosition(Rigidbody rb) { this.rb = rb; }

    public AimPosition(Vector3 pos, RaycastHit hit)
    {
        this.pos = pos;
        this.hit = hit;
    }
    
    public override string ToString()
    {
        var str = "Hit Anything: " + HitAnything;
        if (HitAnything)
        {
            str += "\nHit Target Reticle: " + HitTargetReticle;
            str += "\nTarget Has Rigidbody: " + TargetRb;
        }

        str += "\nPosition: " + pos;
        str += "\nRb: " + rb;

        return str;
    } 

    public static AimPosition FromMouse(Camera camera = null, bool drawRay = false, float castDistance = 10000)
    {
        // TODO: Don't use camera.main at all?
        if (camera == null)
        {
            camera = Camera.main;
            Debug.LogWarning("Using Camera.main!");
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (drawRay) Debug.DrawRay(ray.origin, ray.direction * castDistance);

        //Physics.Raycast(ray, out hitInfo, aimRaycastDistance, ~LayerMask.GetMask("Player"));
        // TODO: Change that layer to "Targetable"?
        Physics.Raycast(ray, out hit, castDistance, 1 << LayerMask.NameToLayer("Targetable"));

        var pos = (hit.collider != null) ? hit.point : ray.GetPoint(castDistance);

        return new AimPosition(pos, hit);
    }
}
