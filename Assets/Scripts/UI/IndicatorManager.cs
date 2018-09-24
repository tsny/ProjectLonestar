using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : ShipUIElement
{
    public GameObject indicatorPrefab;
    public Transform indicatorLayer;
    public Dictionary<ITargetable, TargetIndicator> indicatorPairs = new Dictionary<ITargetable, TargetIndicator>();

    public TargetIndicator selectedIndicator;

    public override void SetShip(Ship newShip)
    {
        //base.SetShip(newShip);

        //ClearIndicators();

        //newShip.hardpointSystem.scanner.EntryAdded += HandleScannerEntryAdded;
        //newShip.hardpointSystem.scanner.EntryRemoved += HandleScannerEntryRemoved;

        //FindObjectOfType<PlayerController>().ReleasedShip += HandleShipReleased;

        //RefreshIndicators();
    }

    private void HandleScannerEntryRemoved(Scanner sender, ITargetable entry)
    {
        RemoveIndicator(entry);
    }

    private void HandleScannerEntryAdded(Scanner sender, ITargetable entry)
    {
        AddIndicator(entry);
    }

    private void HandleShipReleased(PlayerController sender, PossessionEventArgs args)
    {
        ClearShip();
    }

    protected override void ClearShip()
    {
        ClearIndicators();
        base.ClearShip();
    }

    private void HandleIndicatorSelected(TargetIndicator newIndicator)
    {
        if (selectedIndicator != null)
        {
            selectedIndicator.Deselect();
        }

        selectedIndicator = newIndicator;
    }

    public void RefreshIndicators()
    {
        ClearIndicators();

        foreach (var target in ship.hardpointSystem.scanner.targets)
        {
            AddIndicator(target);
        }
    }

    public void ClearIndicators()
    {
        DeselectCurrentIndicator();

        foreach (var value in indicatorPairs.Values)
        {
            Destroy(value);
        }

        indicatorPairs.Clear();
    }

    public void DeselectCurrentIndicator()
    {
        selectedIndicator = null;

        if (selectedIndicator != null) selectedIndicator.Deselect();
    }

    public void AddIndicator(ITargetable newTarget)
    {
        TargetIndicator newIndicator = Instantiate(indicatorPrefab, indicatorLayer).GetComponent<TargetIndicator>();

        newIndicator.SetTarget(newTarget);

        newIndicator.Selected += HandleIndicatorSelected;

        indicatorPairs.Add(newTarget, newIndicator);
    }

    public void RemoveIndicator(TargetIndicator indicatorToRemove)
    {
        if (indicatorToRemove == null) return;

        if (indicatorToRemove == selectedIndicator) DeselectCurrentIndicator();

        indicatorToRemove.Selected -= HandleIndicatorSelected;
        
        Destroy(indicatorToRemove.gameObject);
    }

    public void RemoveIndicator(ITargetable target)
    {
        TargetIndicator pairedIndicator;

        if (indicatorPairs.TryGetValue(target, out pairedIndicator)) RemoveIndicator(pairedIndicator);
    }
}
