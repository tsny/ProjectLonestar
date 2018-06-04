using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScannerUI : ShipUIElement
{
    public Text text;
    public ScannerHardpoint pairedScanner;


    protected override void HandlePossession(PlayerController sender, PossessionEventArgs e)
    {
        if (e.oldShip != null)
        {
            pairedScanner.EntryChanged -= HandleScannerEntryChanged;
        }

        pairedScanner = e.newShip.hardpointSystem.scannerHardpoint;
        pairedScanner.EntryChanged += HandleScannerEntryChanged;

        RefreshScannerList();
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
 