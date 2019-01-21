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
            return camera.WorldToScreenPoint(target.transform.position);
        }
    }
    public bool TargetIsOnScreen
    {
        get
        {
            Vector3 targetViewportPoint = camera.WorldToScreenPoint(target.transform.position);
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
            return DistanceFromTarget < endFadeRange;
        }
    }
    public Ship Ship
    {
        get
        {
            return target.GetComponent<Ship>();
        }
    }
    public float DistanceFromTarget
    {
        get
        {
            return target != null ? Vector3.Distance(camera.transform.position, target.transform.position) : 0;
        }
    }

    private Health targetHealth;
    public bool selected;

    public Text header;
    public GameObject content;

    [Header("Target")]

    public GameObject target;

    public GameObject healthObject;
    public GameObject shieldObject;
    public Image healthBarImage;
    public Image shieldBarImage;

    [Header("Scaling")]

    public bool useScaling = true;
    public float minScale = .1f;
    public float maxScale = .5f;

    public float endFadeRange = 500;
    public float beginFadeRange = 250;

    private bool wasOnScreenLastFrame;
    private Vector3 originalScale;

    public Animator animator;
    public Image buttonImage;
    public new Camera camera;

    public delegate void SelectionEventHandler(TargetIndicator selectedIndicator);
    public delegate void TargetDestroyedEventHandler(TargetIndicator source);

    public event SelectionEventHandler Selected;
    public event SelectionEventHandler Deselected;

    private void Awake()
    {
        originalScale = transform.localScale;
        enabled = false;
        content.SetActive(false);
    }

    private void Start()
    {
        camera = GameSettings.pc.cam != null ? GameSettings.pc.cam : Camera.main;
        ShowHealthBars(false);
        ShowName(false);
    }

    public virtual void SetTarget(GameObject newTarget)
    {
        //newTarget.SetupTargetIndicator(this);

        Ship ship = newTarget.GetComponent<Ship>();
        if (ship == GameSettings.pc.ship)
        {
            Destroy(gameObject);
        }

        var targetIndicator = newTarget.GetComponent<ITargetable>();
        if (targetIndicator != null)
        {
            targetIndicator.SetupTargetIndicator(this);
        }

        targetHealth = newTarget.GetComponent<Health>();

        if (targetHealth.Invulnerable)
        {
            shieldBarImage.gameObject.SetActive(false);
            healthBarImage.gameObject.SetActive(false);
        }

        name = "T-IND: " + newTarget.name;
        enabled = true;

        target = newTarget;

        if (Ship != null)
        ship.Died += (s) => { Destroy(gameObject); };
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        RangeCheck();

        CalculatePosition();
        CalculateScale();
        CalculateTransparency();

        //if (Ship == null) return;
        if (targetHealth == null) return;

        healthBarImage.fillAmount = targetHealth.health / targetHealth.stats.maxHealth;
        shieldBarImage.fillAmount = targetHealth.shield / targetHealth.stats.maxShield;
    }

    public virtual void Select()
    {
        if (selected) return;

        selected = true;

        ShowHealthBars(true);
        ShowName(true);
        buttonImage.raycastTarget = false;
        animator.SetTrigger("Select");
        if (Selected != null) Selected(this);
    }

    public virtual void Deselect()
    {
        if (!selected) return;

        selected = false;

        ShowHealthBars(false);
        ShowName(false);
        buttonImage.raycastTarget = true;
        animator.SetTrigger("Select");

        if (Deselected != null) Deselected(this);
    }

    private void CalculatePosition()
    {
        // Only reactivate children if the target was not onscreen
        // in the last frame and is now.
        if (TargetIsOnScreen && !wasOnScreenLastFrame)
        {
            content.SetActive(true);
        }

        // If the target is now offscreen but was on screen in the
        // last frame, then deactivate children
        else if (!TargetIsOnScreen && wasOnScreenLastFrame)
        {
            content.SetActive(false);
            wasOnScreenLastFrame = false;
            return;
        }


        else if (!TargetIsOnScreen && !wasOnScreenLastFrame)
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
        if (useScaling)
        {
            float distanceQuotient = DistanceFromTarget / endFadeRange;
            float newScale = Mathf.Clamp(distanceQuotient, minScale, maxScale);
            transform.localScale = newScale * Vector3.one;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    private void RangeCheck()
    {
        if ( (DistanceFromTarget > endFadeRange) && selected)
        {
            Deselect();
        }
    }

    private void CalculateTransparency()
    {
        Color newColor;
        newColor = buttonImage.color;

        if (DistanceFromTarget < beginFadeRange)
        {
            newColor.a = 1;
        }
        else
        {
            //float distanceRatio = Mathf.Clamp(distanceFromTarget, beginFadeRange, endFadeRange);
            //distanceRatio = distanceRatio / endFadeRange;

            float distanceRatio = DistanceFromTarget / endFadeRange;
            distanceRatio = Mathf.Clamp(distanceRatio, 0, 1);

            newColor.a = 1 - distanceRatio;
        }

        buttonImage.color = newColor;
    }

    public void SetButtonColor(Item item)
    {
        buttonImage.color = Item.GetMatchingColor(item);
    }

    private void ShowHealthBars(bool value)
    {
        healthObject.SetActive(value);
        shieldObject.SetActive(value);
    }

    private void ShowName(bool value)
    {
        header.gameObject.SetActive(value);
    }
}
