using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class ScannerHardpoint : Hardpoint
{
    public List<ITargetable> scannerRegistry = new List<ITargetable>();
    public Scanner Scanner
    {
        get
        {
            return CurrentEquipment as Scanner;
        }
    }

    public int scanFrequency = 10;

    public delegate void EntryChangeEventHandler(ScannerHardpoint sender, ITargetable entry);

    public event EntryChangeEventHandler EntryAdded;
    public event EntryChangeEventHandler EntryRemoved;

    protected override bool EquipmentMatchesHardpoint(Equipment equipment)
    {
        return equipment is Scanner;
    }

    private void OnEntryAdded(ITargetable target)
    {
        if (EntryAdded != null) EntryAdded(this, target);
    }

    private void OnEntryRemoved(ITargetable target)
    {
        if (EntryRemoved != null) EntryRemoved(this, target);
    }

    protected override void OnMounted(Equipment newEquipment)
    {
        base.OnMounted(newEquipment);

        ScanForTargets();

        StartCoroutine(ScanCoroutine());
    }

    public void ScanForTargets()
    {
        if (Scanner == null) return;

        var scannedTargets = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>().ToList();

        RemoveSelfFromScanner(scannedTargets);

        AddTargetsToRegistry(scannedTargets);
    }

    private void AddTargetsToRegistry(List<ITargetable> newTargets)
    {
        newTargets.ForEach(x => AddEntry(x));
    }

    private void RemoveSelfFromScanner(List<ITargetable> targets)
    {
        var targetsToIgnore = transform.root.gameObject.GetComponentsInChildren<MonoBehaviour>().OfType<ITargetable>();

        targets.RemoveAll(x => targetsToIgnore.Contains(x));
    }

    public void ClearEntries()
    {
        scannerRegistry.ForEach(x => RemoveEntry(x));
        scannerRegistry.Clear();
    }

    private void HandleEntryKilled(ITargetable sender, DeathEventArgs e)
    {
        RemoveEntry(sender);
    }

    private void HandleTargetBecameUntargetable(ITargetable sender)
    {
        RemoveEntry(sender);
    }

    public void AddEntry(ITargetable targetToAdd)
    {
        if (scannerRegistry.Contains(targetToAdd)) return;
    
        scannerRegistry.Add(targetToAdd);

        targetToAdd.BecameUntargetable += HandleTargetBecameUntargetable;

        OnEntryAdded(targetToAdd);
    }

    public void RemoveEntry(ITargetable targetToRemove)
    {
        if (!scannerRegistry.Contains(targetToRemove)) return;

        scannerRegistry.Remove(targetToRemove);

        targetToRemove.BecameUntargetable -= HandleTargetBecameUntargetable;

        OnEntryRemoved(targetToRemove);
    }

    private IEnumerator ScanCoroutine()
    {
        for (; ;)
        {
            ScanForTargets();
            yield return new WaitForSeconds(scanFrequency);
        }
    }
}
