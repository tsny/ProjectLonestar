using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScannerUI : ShipUIElement
{
    public GameObject scannerButtonPrefab;
    public VerticalLayoutGroup buttonVLG;

    private Dictionary<ITargetable, ScannerPanelButton> targetButtonPairs = new Dictionary<ITargetable, ScannerPanelButton>();
    //private Scanner scanner;

    private void HandleNewScan(Scanner sender, List<ITargetable> targets)
    {
        ClearPanel();
        targets.ForEach(x => CreatePanelButton(x));
    }

    private void HandleScannerTargetAdded(Scanner sender, ITargetable entry)
    {
        CreatePanelButton(entry);
    }

    public ScannerPanelButton CreatePanelButton(ITargetable target)
    {
        var newButton = Instantiate(scannerButtonPrefab, buttonVLG.transform).GetComponent<ScannerPanelButton>();

        newButton.Setup(target, ship);

        targetButtonPairs.Add(target, newButton);

        target.BecameUntargetable += HandleTargetBecameUntargetable;

        return newButton;
    }

    private void HandleTargetBecameUntargetable(ITargetable sender)
    {
        RemovePanelButton(sender);
    }

    public void RemovePanelButton(ITargetable targetToRemove)
    {
        ScannerPanelButton button;

        if (targetButtonPairs.TryGetValue(targetToRemove, out button))
        {
            Destroy(button.gameObject);
            targetButtonPairs.Remove(targetToRemove);
            targetToRemove.BecameUntargetable -= HandleTargetBecameUntargetable;
        }
    }

    public void PopulatePanel()
    {
        //foreach (var target in scanner.targets)
        //{
        //CreatePanelButton(target);
        //}
    }

    private void RefreshPanel()
    {
        ClearPanel();
        PopulatePanel();
    }

    public void ClearPanel()
    {
        var keys = targetButtonPairs.Keys.ToList();

        foreach (var key in keys)
        {
            RemovePanelButton(key);
        }
    }

    public override void OnPossessed(PlayerController pc, PossessionEventArgs e)
    {

    }

    public override void OnReleased(PlayerController pc, PossessionEventArgs e)
    {
        ClearPanel();
    }
}