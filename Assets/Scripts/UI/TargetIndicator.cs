using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TargetIndicator : MonoBehaviour
{
    public Vector3 TargetViewportPoint
    {
        get
        {
            return Camera.main.WorldToScreenPoint(target.transform.position);
        }
    }
    public bool IsOnscreen
    {
        get
        {
            Vector3 targetViewportPoint = Camera.main.WorldToScreenPoint(target.transform.position);
            return targetViewportPoint.z > 0;
        }
    }
    public bool HasTarget
    {
        get
        {
            return target != null;
        }
    }
    public bool TargetIsInRange
    {
        get
        {
            return distanceFromTarget < maxRange;
        }
    }

    public bool selected;

    public Text header;
    public GameObject content;

    [Header("Target")]

    public WorldObject target;

    public GameObject healthObject;
    public GameObject shieldObject;
    public Image healthBarImage;
    public Image shieldBarImage;

    public float targetHealth;
    public float targetShield;
    public float distanceFromTarget;

    [Header("Scaling")]

    public float minScale = .1f;
    public float maxScale = .5f;

    public float maxRange = 500;

    private bool wasOnScreenLastFrame;

    public Animator animator;
    public Image buttonImage;

    public delegate void SelectionEventHandler(TargetIndicator selectedIndicator);
    public delegate void TargetDestroyedEventHandler(TargetIndicator source);

    public event SelectionEventHandler Selected;
    public event SelectionEventHandler Deselected;
    public event TargetDestroyedEventHandler TargetDestroyed;

    protected void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        //if (HasTarget == false) enabled = false;
        RangeCheck();

        CalculatePosition();
        CalculateScale();
        CalculateTransparency();
    }

    private void Start()
    {
        ShowHealthBar(false);
        ShowName(false);
    }

    private void OnDisable()
    {
        content.SetActive(false);
    }

    private void OnDestroy()
    {
        target.TookDamage -= HandleTargetTookDamage;
        target.Killed -= HandleTargetKilled;
    }

    public void HandleTargetKilled(WorldObject sender, DeathEventArgs e)
    {
        if (TargetDestroyed != null) TargetDestroyed(this);
        Destroy(gameObject);
    }
    
    private void HandleTargetTookDamage(WorldObject sender, DamageEventArgs e)
    {
        SetHealthBarFill();
        SetShieldBarFill();
    }

    public void Select()
    {
        if (selected) return;

        selected = true;

        ShowHealthBar(true);
        ShowName(true);
        buttonImage.raycastTarget = false;

        animator.Play("TargetGrow");

        if (Selected != null) Selected(this);
    }

    public void Deselect()
    {
        if (!selected) return;

        selected = false;

        ShowHealthBar(false);
        ShowName(false);
        buttonImage.raycastTarget = true;

        animator.Play("TargetShrink");

        if (Deselected != null) Deselected(this);
    }

    private void CalculatePosition()
    {
        // Only reactivate children if the target was not onscreen
        // in the last frame and is now.
        if (IsOnscreen && !wasOnScreenLastFrame)
        {
            content.SetActive(true);
        }

        // If the target is now offscreen but was on screen in the
        // last frame, then deactivate children
        if (!IsOnscreen && wasOnScreenLastFrame)
        {
            content.SetActive(false);
            wasOnScreenLastFrame = false;
            return;
        }


        if (!IsOnscreen && !wasOnScreenLastFrame)
        {
            return;
        }

        Vector3 newPosition = TargetViewportPoint;
        newPosition.z = 0;
        transform.position = newPosition;
        wasOnScreenLastFrame = true;
    }

    private void CalculateScale()
    {
        distanceFromTarget = Vector3.Distance(target.transform.position, Camera.main.gameObject.transform.position);

        float distanceQuotient = distanceFromTarget / maxRange;
        float newScale = Mathf.Clamp(distanceQuotient, minScale, maxScale);
        transform.localScale = newScale * Vector3.one;
    }

    private void RangeCheck()
    {
        if ( (distanceFromTarget > maxRange) && selected)
        {
            Deselect();
        }
    }

    private void CalculateTransparency()
    {
        Color newColor;

        if ( distanceFromTarget < ( maxRange / 2 ) )
        {
            newColor = buttonImage.color;
            newColor.a = 1;

            buttonImage.color = newColor;
            return;
        }

        float distanceRatio = distanceFromTarget / maxRange;

        distanceRatio = Mathf.Clamp(distanceRatio, 0, 1);

        newColor = buttonImage.color;
        newColor.a = 1 - distanceRatio;

        buttonImage.color = newColor;
    }

    public void SetTarget(WorldObject newTarget)
    {
        target = newTarget;

        target.Killed += HandleTargetKilled;
        target.TookDamage += HandleTargetTookDamage;

        name = target.name;

        SetHealthBarFill();
        SetShieldBarFill();

        enabled = true;
    }

    private void SetButtonColor(Item item)
    {
        buttonImage.color = Item.GetMatchingColor(item);
    }

    private void ShowHealthBar(bool value)
    {
        healthObject.SetActive(value);
        shieldObject.SetActive(value);
    }

    private void ShowName(bool value)
    {
        header.gameObject.SetActive(value);
    }

    private void SetHealthBarFill()
    {
        targetHealth = target.hullHealth;
        healthBarImage.fillAmount = target.hullHealth / target.hullFullHealth;
    }

    private void SetShieldBarFill()
    {
        var ship = target as Ship;

        if (ship == null || ship.hardpointSystem.shieldHardpoint.IsMounted == false)
        {
            shieldBarImage.fillAmount = 0;
            return;
        }

        var shield = ship.hardpointSystem.shieldHardpoint;
        shieldBarImage.fillAmount = shield.health / shield.capacity;
    }
}
