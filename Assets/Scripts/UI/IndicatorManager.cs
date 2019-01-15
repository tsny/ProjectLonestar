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

    private void Awake()
    {
        Ship.Spawned += HandleShipSpawned;
    }

    private void HandleShipSpawned(Ship ship)
    {
        if (GameSettings.pc.ship != null && GameSettings.pc.ship != ship)
        {
            AddIndicator(ship.gameObject);
        }
    }

    private void Start() 
    {
        CreateIndicators();     
    }

    // Not really perfomant at all, consider not using ever lol
    private void CreateIndicators()
    {
        var targets = FindObjectsOfType<Ship>().ToList();

        foreach (var ship in targets)
        {
            if (ship == GameSettings.pc.ship)
            {
                targets.Remove(ship);
                break;
            }
        }

        targets.ForEach(x => AddIndicator(x.gameObject));
    }

    private void HandleShipReleased(PlayerController sender, PossessionEventArgs args)
    {
        ClearIndicators();
        ClearShip();
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
        if (selectedIndicator != null) selectedIndicator.Deselect();
        selectedIndicator = null;
    }

    public void AddIndicator(GameObject target)
    {
        TargetIndicator newIndicator = Instantiate(indicatorPrefab, indicatorLayer).GetComponent<TargetIndicator>();
        newIndicator.SetTarget(target);
        newIndicator.Selected += HandleIndicatorSelected;
    }
}
