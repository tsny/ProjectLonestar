using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : ShipUIElement
{
    public List<Ship> shipsInScene = new List<Ship>();
    public TargetIndicator indicatorPrefab;
    public ScannerPanelButton scannerPanelButtonPrefab;

    public Camera cam;
    public Transform indicatorLayer;
    public Transform scannerEntries;

    public TargetIndicator selectedIndicator;

    private void Awake()
    {
        Ship.Spawned += HandleShipSpawned;
        Loot.Spawned += HandleLootSpawned;

        TargetingInfo.Spawned += (info) => { AddIndicator(info); };
    }

    private void HandleShipDied(Ship ship)
    {
        shipsInScene.Remove(ship);
    }

    private void HandleShipSpawned(Ship ship)
    {
        shipsInScene.Add(ship);
        ship.Died += HandleShipDied;
    }

    private void HandleLootSpawned(Loot loot)
    {
        //AddIndicator(loot.gameObject);
    }

    // Not really perfomant at all, consider not using ever lol
    private void CreateIndicators()
    {
        var targets = FindObjectsOfType<TargetingInfo>();
        foreach (var target in targets) AddIndicator(target);
    }

    private void HandleShipReleased(PlayerController sender, PossessionEventArgs args)
    {
        ClearIndicators();
        Clear();
    }

    private void HandleIndicatorSelected(TargetIndicator newIndicator)
    {
        if (selectedIndicator != null)
            selectedIndicator.Deselect();

        selectedIndicator = newIndicator;
    }

    public void ClearIndicators()
    {
        DeselectCurrentIndicator();
        FindObjectsOfType<TargetIndicator>().ToList().ForEach(x => Destroy(x.gameObject));
    }

    public void DeselectCurrentIndicator()
    {
        if (selectedIndicator != null) 
            selectedIndicator.Deselect();

        selectedIndicator = null;
    }

    public void AddIndicator(TargetingInfo info)
    {
        var newIndicator = Instantiate(indicatorPrefab, indicatorLayer);
        newIndicator.Setup(info, cam);
        newIndicator.Selected += HandleIndicatorSelected;
    }
}
