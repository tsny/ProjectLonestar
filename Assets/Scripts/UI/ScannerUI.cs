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
    }

    private void HandleScannerEntryChanged(WorldObject entry, bool added)
    {
        RefreshScannerList();
    }

    private void RefreshScannerList()
    {
        foreach (Transform button in buttonVLG.transform)
        {
            Destroy(button.gameObject);
        }

        foreach (var obj in scannerHardpoint.detectedObjects)
        {
            var newButton = Instantiate(scannerButtonPrefab, buttonVLG.transform).GetComponent<ScannerPanelButton>();
            newButton.Setup(obj, ship);
        }
    }
}
 