using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class ScannerHardpoint : MonoBehaviour
{
    public List<ITargetable> targets = new List<ITargetable>();

    public ScannerStats scanner;

    public int scanFrequency = 10;

    public delegate void EntryChangeEventHandler(ScannerHardpoint sender, ITargetable entry);

    public event EntryChangeEventHandler EntryAdded;
    public event EntryChangeEventHandler EntryRemoved;

    private void OnEntryAdded(ITargetable target)
    {
        if (EntryAdded != null) EntryAdded(this, target);
    }

    private void OnEntryRemoved(ITargetable target)
    {
        if (EntryRemoved != null) EntryRemoved(this, target);
    }

    public void ScanForTargets()
    {
        if (scanner == null) return;

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
        targets.ForEach(x => RemoveEntry(x));
        targets.Clear();
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
        if (targets.Contains(targetToAdd)) return;
    
        targets.Add(targetToAdd);

        targetToAdd.BecameUntargetable += HandleTargetBecameUntargetable;

        OnEntryAdded(targetToAdd);
    }

    public void RemoveEntry(ITargetable targetToRemove)
    {
        if (!targets.Contains(targetToRemove)) return;

        targets.Remove(targetToRemove);

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
