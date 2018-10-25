using System.Collections;
using System.Linq;
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

        FindObjectOfType<PlayerController>().ReleasedShip += HandleShipReleased;
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

        FindObjectsOfType<TargetIndicator>().ToList().ForEach(x => Destroy(x));
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

        //newTarget.BecameUntargetable += HandleTargetBecameUntargetable;

        indicatorPairs.Add(newTarget, newIndicator);
    }
}
