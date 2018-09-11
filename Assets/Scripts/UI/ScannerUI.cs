using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScannerUI : ShipUIElement
{
    public GameObject scannerButtonPrefab;
    public VerticalLayoutGroup buttonVLG;

    private ScannerHardpoint pairedScanner;

    protected override void HandlePossessed(PlayerController sender, Ship newShip)
    {
        base.HandlePossessed(sender, newShip);

        pairedScanner = newShip.hardpointSystem.scannerHardpoint;
        pairedScanner.EntryChanged += HandleScannerEntryChanged;

        RefreshScannerList();

        gameObject.SetActive(true);
    }

    protected override void HandleUnpossessed(PlayerController sender, Ship oldShip)
    {
        base.HandleUnpossessed(sender, oldShip);
        pairedScanner.EntryChanged -= HandleScannerEntryChanged;
        pairedScanner = null;

        gameObject.SetActive(false);
    }

    private void HandleScannerEntryChanged(WorldObject entry, bool added)
    {
        RefreshScannerList();
    }

    private void RefreshScannerList()
    {
        foreach (var obj in pairedScanner.detectedObjects)
        {
            var newButton = Instantiate(scannerButtonPrefab, buttonVLG.transform).GetComponent<ScannerPanelButton>();
            newButton.Setup(obj, ship);
        }
    }
}
 