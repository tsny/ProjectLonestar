using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScannerUI : ShipUIElement
{
    public Text scannerText;
    private ScannerHardpoint pairedScanner;

    protected override void HandlePossessed(PlayerController sender, Ship newShip)
    {
        pairedScanner = newShip.hardpointSystem.scannerHardpoint;
        pairedScanner.EntryChanged += HandleScannerEntryChanged;

        RefreshScannerList();
    }

    protected override void HandleUnpossessed(PlayerController sender, Ship oldShip)
    {
        base.HandleUnpossessed(sender, oldShip);
        pairedScanner.EntryChanged -= HandleScannerEntryChanged;

        scannerText.text = "";
        pairedScanner = null;
    }

    private void HandleScannerEntryChanged(WorldObject entry, bool added)
    {
        RefreshScannerList();
    }

    private void RefreshScannerList()
    {
        scannerText.text = "";

        foreach (WorldObject detectedObject in pairedScanner.detectedObjects)
        {
            scannerText.text += detectedObject.name + "\n";
        }
    }
}
 