using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScannerUI : ShipUIElement
{
    public Text text;
    public ScannerHardpoint pairedScanner;


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
    }

    private void HandleScannerEntryChanged(WorldObject entry, bool added)
    {
        RefreshScannerList();
    }

    private void RefreshScannerList()
    {
        text.text = "";

        foreach (WorldObject detectedObject in pairedScanner.detectedObjects)
        {
            text.text += detectedObject.name + "\n";
        }
    }
}
 