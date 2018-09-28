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
        base.SetShip(newShip);

        ClearIndicators();

        newShip.hardpointSystem.scanner.ScannerUpdated += HandleScannerUpdated;

        FindObjectOfType<PlayerController>().ReleasedShip += HandleShipReleased;
    }

    private void HandleScannerUpdated(Scanner sender, List<ITargetable> targets)
    {
        foreach (var target in targets)
        {
            AddIndicator(target);
        }
    }

    private void HandleShipReleased(PlayerController sender, PossessionEventArgs args)
    {
        ClearIndicators();
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

    public void ClearIndicators()
    {
        DeselectCurrentIndicator();

        foreach (var value in indicatorPairs.Values)
        {
            Destroy(value.gameObject);
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

        newTarget.BecameUntargetable += HandleTargetBecameUntargetable;

        indicatorPairs.Add(newTarget, newIndicator);
    }

    private void HandleTargetBecameUntargetable(ITargetable sender)
    {
        RemoveIndicator(sender);
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

        if (indicatorPairs.TryGetValue(target, out pairedIndicator))
        {
            RemoveIndicator(pairedIndicator);
            target.BecameUntargetable -= HandleTargetBecameUntargetable;
        }
    }
}
