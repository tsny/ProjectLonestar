using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : ShipUIElement
{
    public TargetIndicator indicatorPrefab;
    public ScannerPanelButton scannerPanelButtonPrefab;

    public Camera cam;
    public Transform indicatorLayer;
    public Transform scannerEntries;

    public TargetIndicator selectedIndicator;

    public override void Init(PlayerController pc)
    {
        base.Init(pc);
        cam = pc.cam;

        Refresh();
        TargetingInfo.Spawned += AddIndicator;
    }

    private void OnDestroy()
    {
        TargetingInfo.Spawned -= AddIndicator;
    }

    private void Refresh()
    {
        ClearIndicators();
        CreateIndicators();
    }

    // Not really perfomant at all, consider not using ever lol
    private void CreateIndicators()
    {
        var targets = FindObjectsOfType<TargetingInfo>();
        foreach (var target in targets) AddIndicator(target);
    }

    private void HandleIndicatorSelected(TargetIndicator newIndicator)
    {
        if (selectedIndicator != null) selectedIndicator.Deselect();
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

    public void AddIndicator(TargetingInfo info)
    {
        var newIndicator = Instantiate(indicatorPrefab, indicatorLayer);
        newIndicator.Setup(info, Color.white, cam);
        newIndicator.Selected += HandleIndicatorSelected;
    }

    public override void OnPossessed(PlayerController pc, PossessionEventArgs e)
    {
        Refresh();
    }

    public override void OnReleased(PlayerController pc, PossessionEventArgs e)
    {
        ClearIndicators();
    }
}