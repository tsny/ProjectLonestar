using UnityEngine;
using System.Collections;
using System;

public class Engine : ShipComponent 
{
    [Header("Main")]
    public Rigidbody rb;
    public new Transform transform;

    [Header("Model")]
    public float shipModelZModifier = 1;
    private Transform _shipBaseTransform;
    public Transform ShipBaseTransform
    {
        get
        {
            return _shipBaseTransform;
        }
        set
        {
            if (!value) return;
            _shipBaseTransform = value;
            shipModelOrigRot = value.localRotation;
        }
    }
    public float lerpModifier = .1f;
    public ConstantForce cf;
    private Quaternion shipModelOrigRot;

    public float Speed
    {
        get
        {
            //return Vector3.Dot(rb.velocity, transform.forward);
            return Mathf.Abs(rb.velocity.z);
        }
    }

    [Header("Stats")]
    public EngineStats engineStats;

    public Cooldown sidestepCD;
    public Cooldown blinkCD;

    public float sidestepForce = 50000;
    public float sidestepDur = .2f;
    public float maxZVelocity = 10;
    public float blinkDistance = 20;

    private float throttle;
    public float Throttle
    {
        get
        {
            return throttle;
        }

        set
        {
            if (value == throttle) return;
            var oldThrottle = throttle;
            throttle = Mathf.Clamp(value, 0, 1);
            OnThrottleChange(value, oldThrottle);
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
            if (value == strafe) return;

            if (value != 0) Drifting = false;

            var oldStrafe = strafe;

            strafe = Mathf.Clamp(value, -1, 1);

            OnStrafeChange(value, oldStrafe);
        }
    }
    public float throttleChangeIncrement = .1f;

    public bool IsStrafing { get { return Strafe != 0; } }

    private bool drifting;
    public bool Drifting
    {
        get
        {
            return drifting;
        }

        set
        {
            if (drifting == value) return;

            if (value) 
            {
                Strafe = 0;
                Throttle = 0;
            }

            drifting = value;
            OnDriftingChange(value);
        }
    }

    public bool clampVelocity = true;
    private IEnumerator sidestepCR;
    private IEnumerator blinkCR;

    public Afterburner aft;

    private float _yaw;
    public float Yaw { get { return _yaw; } }

    private float _roll;
    public float Roll { get { return _roll; } }

    private float _pitch;
    public float Pitch { get { return _pitch; } }


    public delegate void ThrottleChangedEventHandler(Engine sender, ThrottleChangeEventArgs e);
    public event ThrottleChangedEventHandler ThrottleChanged;

    public delegate void StrafeChangedEventHandler(Engine sender, float newStrafe, float oldStrafe);
    public event StrafeChangedEventHandler StrafeChanged;

    public event EventHandler DriftingChange;
    public delegate void EventHandler(bool drifting);

    public override void Initialize(Ship sender)
    {
        base.Initialize(sender);

        ShipBaseTransform = sender.ShipBase.transform;
    }

    public void SidestepRight()
    {
        if (sidestepCR != null || sidestepCD.isDecrementing) return;

        sidestepCR = SidestepRoutine(new Vector3(sidestepForce, 0, 0), ForceMode.Force);
        StartCoroutine(sidestepCR);
    }

    public void SidestepLeft()
    {
        if (sidestepCR != null || sidestepCD.isDecrementing) return;

        sidestepCR = SidestepRoutine(new Vector3(-sidestepForce, 0, 0), ForceMode.Force);
        StartCoroutine(sidestepCR);
    }

    public void Blink(bool forwards = true)
    {
        if (blinkCR != null || blinkCD.isDecrementing) return;
        blinkCR = BlinkCoroutine(.1f, forwards);
        StartCoroutine(blinkCR);
    }

    private IEnumerator BlinkCoroutine(float dur = .1f, bool forwards = true)
    {
        if (blinkCD.isDecrementing) yield break;

        //ship.ShipBase.shipMesh.enabled = false;
        ship.ShipBase.gameObject.SetActive(false);
        // Make mesh invisible and disable colliders

        yield return new WaitForSeconds(dur);

        // reenable mesh/colliders
        //ship.ShipBase.shipMesh.enabled = true;
        ship.ShipBase.gameObject.SetActive(true);

        var forwardsInt = forwards ? 1 : 0;
        transform.position = transform.position + transform.forward * (blinkDistance * forwardsInt);
        blinkCD.Begin(this);
        blinkCR = null;
    }

    public void BlinkBackwards()
    {
        if (blinkCD.isDecrementing) return;

        transform.position = transform.position + transform.forward * (-blinkDistance);
        blinkCD.Begin(this);
    }

    private IEnumerator SidestepRoutine(Vector3 baseForce, ForceMode mode)
    {
        float elapsed = 0;

        while (elapsed < sidestepDur)
        {
            rb.AddRelativeForce(baseForce * (elapsed / sidestepDur), mode);
            elapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        sidestepCD.Begin(this);
        sidestepCR = null;
    }

    public void SidestepHorizontal(bool usePositiveThrust)
    {
        if (sidestepCR != null || sidestepCD != null) return;
        var thrustBool = usePositiveThrust ? 1 : -1;

        sidestepCR = SidestepRoutine(new Vector3(sidestepForce * thrustBool, 0, 0), ForceMode.Force);
        StartCoroutine(sidestepCR);
    }

    private void Awake()
    {
        engineStats = Utilities.CheckScriptableObject<EngineStats>(engineStats);
        blinkCD = Utilities.CheckScriptableObject<Cooldown>(blinkCD);
        sidestepCD = Utilities.CheckScriptableObject<Cooldown>(sidestepCD);
    }

    private void OnDriftingChange(bool isDrifting)
    {
        if (DriftingChange != null) DriftingChange(isDrifting);
    }

    private void OnStrafeChange(float newStrafe, float oldStrafe)
    {
        if (StrafeChanged != null) StrafeChanged(this, newStrafe, oldStrafe);
    }

    private void OnThrottleChange(float newThrottle, float oldThrottle)
    {
        if (ThrottleChanged != null) ThrottleChanged(this, new ThrottleChangeEventArgs(newThrottle, oldThrottle));
    }

    private void HandleCruiseChange(CruiseEngine sender)
    {
        if (sender.State != CruiseState.Off)
            Drifting = false;
    }

    private void FixedUpdate()
    {
        Vector3 forces = Vector3.zero;

        if (Drifting && aft != null && aft.IsBurning)
        {
            ShipPhysicsStats.ClampShipVelocity(rb, ship.physicsStats, CruiseState.Off);
            ShipPhysicsStats.HandleDrifting(rb, ship.physicsStats, false);
            forces = CalcAfterburnerForces();
        }

        else if (Drifting)
        {
            ShipPhysicsStats.HandleDrifting(rb, ship.physicsStats, true);
        }

        else if (!Drifting)
        {
            ShipPhysicsStats.HandleDrifting(rb, ship.physicsStats, false);
            forces = CalcStrafeForces() + CalcThrottleForces() + CalcAfterburnerForces();
        } 

        if (forces != Vector3.zero)
            rb.AddForce(forces);
    }

    private Vector3 GetClampedVelocity(Vector3 velocity)
    {
        var newZ = Mathf.Clamp(velocity.z, -maxZVelocity, maxZVelocity); 
        return new Vector3(rb.velocity.x, rb.velocity.y, newZ);
    }

    private Vector3 CalcStrafeForces()
    {
        return rb.transform.right * Strafe * engineStats.strafePower;
    }

    private Vector3 CalcThrottleForces()
    {
        return rb.transform.forward * Throttle * engineStats.enginePower;
    }

    private Vector3 CalcAfterburnerForces()
    {
        if (!aft || !aft.IsBurning) return Vector3.zero;

        return rb.transform.forward * aft.stats.thrust;
    }

    public void ThrottleUp()
    {
        Drifting = false;
        if (Throttle == 1) return;
        Throttle = Mathf.MoveTowards(Throttle, 1, throttleChangeIncrement);
    }

    public void ThrottleDown()
    {
        Drifting = false;
        Throttle = Mathf.MoveTowards(Throttle, 0, throttleChangeIncrement);
    }

    public void ToggleDrifting()
    {
        Drifting = !Drifting;
    }

    public void LerpYawToNeutral()
    {
        Quaternion neutralRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        neutralRotation = Quaternion.Lerp(transform.rotation, neutralRotation, Time.deltaTime);
        transform.rotation = neutralRotation;

        if (ShipBaseTransform != null)
        {
            neutralRotation = Quaternion.Lerp(ShipBaseTransform.localRotation, shipModelOrigRot, lerpModifier);
            ShipBaseTransform.localRotation = neutralRotation;
        }
    }

    public void AddPitch(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(Vector3.left * amount * engineStats.turnSpeed);
        _pitch = amount;
    }

    public void AddYaw(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(Vector3.up * amount * engineStats.turnSpeed);
        _yaw = amount;
        VisualYawRotation(amount);
    }

    public void AddRoll(float amount)
    {
        amount = Mathf.Clamp(amount, -1, 1);
        transform.Rotate(Vector3.forward * amount * engineStats.turnSpeed);
        _roll = amount;
    }

    private void VisualYawRotation(float amount)
    {
        if (ShipBaseTransform == null) 
        {
            Debug.LogWarning(gameObject + " has no ship model in the inspector");
            return;
        }

        // Rotate the model slightly based on yawOffset
        Vector3 turnRotation = shipModelOrigRot.eulerAngles + new Vector3(0, 0, -amount * shipModelZModifier);
        ShipBaseTransform.localRotation = Quaternion.Euler(turnRotation);
    }
}

public class ThrottleChangeEventArgs
{
    public float newThrottle;
    public float oldThrottle;

    public ThrottleChangeEventArgs(float newThrottle, float oldThrottle)
    {
        this.newThrottle = newThrottle;
        this.oldThrottle = oldThrottle;
    }

    public bool IsAccelerating
    {
        get
        {
            return newThrottle > oldThrottle;
        }
    }
}
