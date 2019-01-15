using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : ShipUIElement
{
    public GameObject indicatorPrefab;
    public Transform indicatorLayer;

    public TargetIndicator selectedIndicator;

    public override void SetShip(Ship newShip)
    {
        base.SetShip(newShip);
        CreateIndicators();
    }

    private void CreateIndicators()
    {
        var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>().ToList();

        foreach(var target in targets)
        {
            var ship = target as Ship;

            if (ship != null && ship == GameSettings.pc.ship)
            {
                targets.Remove(target);
                break;
            }
        }

        targets.ForEach(x => AddIndicator(x));
    }

    private void HandleShipReleased(PlayerController sender, PossessionEventArgs args)
    {
        ClearIndicators();
        ClearShip();
    }

    protected override void ClearShip()
    {
        base.ClearShip();
        ClearIndicators();
        CreateIndicators();
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

        FindObjectsOfType<TargetIndicator>().ToList().ForEach(x => Destroy(x.gameObject));
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
    }
}
