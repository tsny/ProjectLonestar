using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : ShipUIElement
{
    public GameObject indicatorPrefab;
    public Transform indicatorLayer;
    public List<TargetIndicator> indicators = new List<TargetIndicator>();
    public Dictionary<WorldObject, TargetIndicator> indicatorPairs = new Dictionary<WorldObject, TargetIndicator>();

    public TargetIndicator selectedIndicator;

    private void HandleIndicatorSelected(TargetIndicator newIndicator)
    {
        if (selectedIndicator != null) selectedIndicator.Deselect();

        selectedIndicator = newIndicator;
    }

    private void HandleEntryChanged(WorldObject entry, bool added)
    {
        if (added) AddIndicator(entry);

        else RemoveIndicator(entry);
    }

    public void ClearIndicators()
    {
        foreach (TargetIndicator indicator in indicators) RemoveIndicator(indicator);

        indicatorPairs.Clear();
        indicators.Clear();

        DeselectCurrentIndicator();
    }


    public void DeselectCurrentIndicator()
    {
        selectedIndicator = null;

        if (selectedIndicator != null) selectedIndicator.Deselect();

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

        if (indicatorToRemove == selectedIndicator) DeselectCurrentIndicator();

        indicatorToRemove.Selected -= HandleIndicatorSelected;
        indicatorToRemove.TargetDestroyed -= RemoveIndicator;
        
        Destroy(indicatorToRemove.gameObject);
    }

    public void RemoveIndicator(WorldObject worldObject)
    {
        TargetIndicator pairedIndicator;

        if (indicatorPairs.TryGetValue(worldObject, out pairedIndicator)) RemoveIndicator(pairedIndicator);
    }
}
