using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : ShipUIElement
{
    #region Fields

    public GameObject indicatorPrefab;
    public Transform indicatorLayer;
    public List<TargetIndicator> indicators;
    public Dictionary<WorldObject, TargetIndicator> indicatorPairs;

    public TargetIndicator selectedIndicator;

    #endregion

    #region Event Handlers

    private void HandleIndicatorSelected(TargetIndicator newIndicator)
    {
        if (selectedIndicator != null)
        {
            selectedIndicator.Deselect();
        }

        selectedIndicator = newIndicator;
    }

    protected override void HandlePossessed(PlayerController sender, Ship newShip)
    {
        base.HandlePossessed(sender, newShip);

        ClearIndicators();

        foreach (WorldObject worldObject in newShip.hardpointSystem.scannerHardpoint.detectedObjects)
        {
            AddIndicator(worldObject);
        }

        newShip.hardpointSystem.scannerHardpoint.EntryChanged += HandleEntryChanged;
    }

    protected override void HandleUnpossessed(PlayerController sender, Ship oldShip)
    {
        base.HandleUnpossessed(sender, oldShip);
        oldShip.hardpointSystem.scannerHardpoint.EntryChanged -= HandleEntryChanged;
        ClearIndicators();
    }

    private void HandleEntryChanged(WorldObject entry, bool added)
    {
        if (added)
        {
            AddIndicator(entry);
        }

        else
        {
            RemoveIndicator(entry);
        }
    }

    #endregion

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();

        indicators = new List<TargetIndicator>();
        indicatorPairs = new Dictionary<WorldObject, TargetIndicator>();
    }

    #endregion

    #region Methods

    public void ClearIndicators()
    {
        foreach (TargetIndicator indicator in indicators)
        {
            RemoveIndicator(indicator);
        }

        indicatorPairs.Clear();
        indicators.Clear();

        DeselectCurrentIndicator();
    }


    public void DeselectCurrentIndicator()
    {
        if (selectedIndicator != null)
        {
            selectedIndicator.Deselect();
        }

        selectedIndicator = null;
    }

    public void AddIndicator(WorldObject newTarget)
    {
        TargetIndicator newIndicator = Instantiate(indicatorPrefab, indicatorLayer).GetComponent<TargetIndicator>();

        newIndicator.SetTarget(newTarget);

        newIndicator.Selected += HandleIndicatorSelected;
        newIndicator.TargetDestroyed += RemoveIndicator;

        indicators.Add(newIndicator);

        indicatorPairs.Add(newTarget, newIndicator);
    }

    public void RemoveIndicator(TargetIndicator indicatorToRemove)
    {
        if (indicatorToRemove == null) return;

        if (indicatorToRemove == selectedIndicator)
        {
            DeselectCurrentIndicator();
        }
        
        Destroy(indicatorToRemove.gameObject);
    }

    public void RemoveIndicator(WorldObject worldObject)
    {
        TargetIndicator pairedIndicator;

        if (indicatorPairs.TryGetValue(worldObject, out pairedIndicator))
        {
            RemoveIndicator(pairedIndicator);
        }
    }

    #endregion

}













