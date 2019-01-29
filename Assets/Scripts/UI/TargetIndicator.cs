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
            return cam.WorldToScreenPoint(target.transform.position);
        }
    }
    public bool TargetIsOnScreen
    {
        get
        {
            Vector3 targetViewportPoint = cam.WorldToScreenPoint(target.transform.position);
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
    public Ship Ship { get; set; }
    public float DistanceFromTarget
    {
        get
        {
            return target != null ? Vector3.Distance(cam.transform.position, target.transform.position) : 0;
        }
    }

    public bool selected;

    public Text header;
    public GameObject content;

    [Header("Target")]

    public Rigidbody targetRb;
    public TargetingInfo target;

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
    public CanvasGroup canvasGroup;

    private bool wasOnScreenLastFrame;
    private Vector3 originalScale;

    public TargetReticle reticle;
    public Animator animator;
    public Image buttonImage;
    public Camera cam;

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
        cam = GameSettings.pc.cam != null ? GameSettings.pc.cam : Camera.main;
        ToggleHealthBars(false);
        ShowName(false);
        StartCoroutine(RefreshHeader(2));
    }

    public void SetTarget(TargetingInfo info)
    {
        targetRb = info.GetComponent<Rigidbody>();

        Ship = info.GetComponent<Ship>();
        if (Ship != null)
            Ship.Died += (s) => { Destroy(gameObject); };

        targetRb = info.GetComponent<Rigidbody>();

        name = "T_IND: " + info.name;
        target = info;
        enabled = true;
    }

    private void Update()
    {
        if (!HasTarget)
        {
            Destroy(gameObject);
            return;
        }

        if (!target.targetable)
        {
            content.SetActive(false);
            return;
        }

        RangeCheck();

        CalculatePosition();
        CalculateScale();
        CalculateTransparency();

        //if (Ship == null) return;
        if (target.health == null) return;

        var health = target.health;
        healthBarImage.fillAmount = health.health / health.stats.maxHealth;
        shieldBarImage.fillAmount = health.shield / health.stats.maxShield;
    }

    public virtual void Select()
    {
        if (selected) return;

        if (target.showHealthOnSelect) ToggleHealthBars(true);
        ShowName(true);
        buttonImage.raycastTarget = false;
        animator.SetTrigger("Select");

        selected = true;
        if (Selected != null) Selected(this);
    }

    public virtual void Deselect()
    {
        if (!selected) return;

        selected = false;

        ToggleHealthBars(false);
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
        if (DistanceFromTarget < beginFadeRange)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            float distanceRatio = DistanceFromTarget / endFadeRange;
            distanceRatio = Mathf.Clamp(distanceRatio, 0, 1);
            canvasGroup.alpha = 1 - distanceRatio;
        }

        if (canvasGroup.alpha == 0 && selected) Deselect();
    }

    public void SetButtonColor(Item item)
    {
        buttonImage.color = Item.GetMatchingColor(item);
    }

    private void ToggleHealthBars(bool value)
    {
        healthObject.SetActive(value);
        shieldObject.SetActive(value);
    }

    private void ShowName(bool value)
    {
        header.gameObject.SetActive(value);
    }

    private IEnumerator RefreshHeader(float waitDur)
    {
        while (true)
        {
            header.text = target.indicatorHeaderText + " - " + (int) DistanceFromTarget;
            yield return new WaitForSeconds(waitDur);
        }
    }
}
