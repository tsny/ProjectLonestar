using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScannerUI : ShipUIElement
{
    public GameObject scannerButtonPrefab;
    public VerticalLayoutGroup buttonVLG;

    private ScannerHardpoint scannerHardpoint;

    public override void SetShip(Ship ship)
    {
        base.SetShip(ship);

        scannerHardpoint = ship.hardpointSystem.scannerHardpoint;
        scannerHardpoint.EntryChanged += HandleScannerEntryChanged;

        RefreshScannerList();
    }

    protected override void ClearShip()
    {
        scannerHardpoint.EntryChanged -= HandleScannerEntryChanged;
        base.ClearShip();
    }

    private void HandleScannerEntryChanged(WorldObject entry, bool added)
    {
        if (added) CreatePanelButton(entry);
    }

    public ScannerPanelButton CreatePanelButton(WorldObject entry)
    {
        var newButton = Instantiate(scannerButtonPrefab, buttonVLG.transform).GetComponent<ScannerPanelButton>();
        newButton.Setup(entry, ship);

        return newButton;
    }

    private void RefreshScannerList()
    {
        ClearScannerList();

        foreach (var entry in scannerHardpoint.scannerEntries)
        {
            CreatePanelButton(entry);
        }
    }

    public void ClearScannerList()
    {
        foreach (Transform button in buttonVLG.transform)
        {
            Destroy(button.gameObject);
        }
    }
}
 