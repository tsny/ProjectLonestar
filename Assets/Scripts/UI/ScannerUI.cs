using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScannerUI : ShipUIElement
{
    public GameObject scannerButtonPrefab;
    public VerticalLayoutGroup buttonVLG;

    private Dictionary<ITargetable, GameObject> targetObjectPairs = new Dictionary<ITargetable, GameObject>();

    private ScannerHardpoint scannerHardpoint;

    public override void SetShip(Ship ship)
    {
        base.SetShip(ship);

        scannerHardpoint = ship.hardpointSystem.scannerHardpoint;

        scannerHardpoint.EntryAdded += HandleScannerTargetAdded;
        scannerHardpoint.EntryRemoved += HandleScannerTargetRemoved;

        RefreshScannerList();
    }

    private void HandleScannerTargetRemoved(ScannerHardpoint sender, ITargetable entry)
    {
        GameObject value;

        if (targetObjectPairs.TryGetValue(entry, out value))
        {
            Destroy(value);
        }
    }

    private void HandleScannerTargetAdded(ScannerHardpoint sender, ITargetable entry)
    {
        CreatePanelButton(entry);
    }

    protected override void ClearShip()
    {
        scannerHardpoint.EntryAdded -= HandleScannerTargetAdded;
        scannerHardpoint.EntryRemoved -= HandleScannerTargetRemoved;

        ClearScannerList();

        base.ClearShip();
    }

    public ScannerPanelButton CreatePanelButton(ITargetable target)
    {
        var newButton = Instantiate(scannerButtonPrefab, buttonVLG.transform).GetComponent<ScannerPanelButton>();
        newButton.Setup(target, ship);

        targetObjectPairs.Add(target, newButton.gameObject);

        return newButton;
    }

    private void RefreshScannerList()
    {
        ClearScannerList();

        foreach (var target in scannerHardpoint.targets)
        {
            CreatePanelButton(target);
        }
    }

    public void ClearScannerList()
    {
        foreach (Transform button in buttonVLG.transform)
        {
            Destroy(button.gameObject);
        }

        targetObjectPairs.Clear();
    }
}
 