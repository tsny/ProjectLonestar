using UnityEngine;
using System.Collections;
using System;

public class Engine : ShipComponent 
{
    public Rigidbody rb;

    // Move to Ship.cs
    public CruiseEngine cruiseEngine;
    public float Speed
    {
        get
        {
            return Vector3.Dot(rb.velocity, transform.forward);
        }
    }

    public float turnSpeed = 1;
    public float throttlePower = 1;
    public float strafePower = 1;

    private float throttle;
    public float Throttle
    {
        get
        {
            return throttle;
        }

        set
        {
            var oldThrottle = throttle;
            throttle = Mathf.Clamp(value, 0, 1);
            if (ThrottleChanged != null) ThrottleChanged(this, throttle, oldThrottle);
        }
    }

    private float strafe;
    public float Strafe
    {
        get
        {
            return strafe;
        }

        set
        {
            var oldStrafe = strafe;
            strafe = Mathf.Clamp(value, -1, 1);
            if (StrafeChanged != null) StrafeChanged(this, strafe, oldStrafe);
        }
    }
    public float throttleChangeIncrement = .1f;

    public bool IsStrafing { get { return Strafe != 0; } }

    public bool drifting;
    public bool Drifting
    {
        get
        {
            return drifting;
        }

        set
        {
            drifting = value;
            if (DriftingChange != null) DriftingChange(value);
        }
    }

    public float mass = 5;
    public float drag = 5;
    public float driftMass = 5;
    public float driftDrag = .05f;

    public delegate void ThrottleChangedEventHandler(Engine sender, float newThrottle, float oldThrottle);
    public event ThrottleChangedEventHandler ThrottleChanged;

    public delegate void StrafeChangedEventHandler(Engine sender, float newStrafe, float oldStrafe);
    public event StrafeChangedEventHandler StrafeChanged;

    public event EventHandler DriftingChange;
    public delegate void EventHandler(bool drifting);

    public override void InitShipComponent(Ship sender, ShipStats stats)
    {
        base.InitShipComponent(sender, stats);
        turnSpeed = stats.turnSpeed;
        cruiseEngine = sender.cruiseEngine;
        rb = sender.rb;
        mass = rb.mass;
        drag = rb.drag;
        sender.Possession += HandleOwnerPossession;
    }

    private void HandleOwnerPossession(PlayerController pc, Ship sender, bool possessed)
    {
        enabled = possessed;
    }

    private void FixedUpdate()
    {
        ApplyStrafeForces();
        ApplyThrottleForces();
    }

    private void ApplyStrafeForces()
    {
        if (Strafe != 0)
        {
            rb.AddForce(rb.transform.right * Strafe * strafePower);
        }
    }

    private void ApplyThrottleForces()
    {
        if (Throttle > 0)
        {
            rb.AddForce(rb.transform.forward * Throttle * throttlePower);
        }
    }

    public void ThrottleUp()
    {
        Throttle = Mathf.MoveTowards(Throttle, 1, throttleChangeIncrement);
    }

    public void ThrottleDown()
    {
        Throttle = Mathf.MoveTowards(Throttle, 0, throttleChangeIncrement);
    }

    public void LerpYawToNeutral()
    {
        Quaternion neutralRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        neutralRotation = Quaternion.Lerp(transform.rotation, neutralRotation, Time.deltaTime);
        transform.rotation = neutralRotation;
    }

    public void Pitch(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(new Vector3(turnSpeed * -amount, 0, 0));
    }

    public void Yaw(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(new Vector3(0, turnSpeed * amount, 0));
    }

    public void Roll(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(new Vector3(0, 0, turnSpeed * amount));
    }

    public void ToggleDrifting()
    {
        Drifting = !Drifting;

        if (Drifting)
        {
            rb.drag = driftDrag;
            rb.mass = driftMass;
        }

        else
        {
            rb.drag = drag;
            rb.mass = mass;
        }
    }
}
