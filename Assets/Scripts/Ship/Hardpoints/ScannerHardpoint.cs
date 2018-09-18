using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScannerHardpoint : Hardpoint
{
    //public TractorHardpoint tractor;
    public List<WorldObject> scannerEntries;
    public Scanner Scanner
    {
        get
        {
            return CurrentEquipment as Scanner;
        }
    }

    public int scanFrequency = 10;

    public delegate void EntryChangeEventHandler(WorldObject entry, bool added);
    public event EntryChangeEventHandler EntryChanged;

    protected override bool EquipmentMatchesHardpoint(Equipment equipment)
    {
        return equipment is Scanner;
    }

    protected override void OnMounted(Equipment newEquipment)
    {
        base.OnMounted(newEquipment);

        Scan();

        StartCoroutine(ScanCoroutine());
    }

    public void Scan()
    {
        if (Scanner == null) return;

        WorldObject[] scannedObjects = FindObjectsOfType<WorldObject>();

        foreach (WorldObject scannedObject in scannedObjects)
        {
            if (scannerEntries.Contains(scannedObject) || scannedObject == owningShip) continue;
            AddEntry(scannedObject);
        }
    }

    public void ClearEntries()
    {
        foreach (var entry in scannerEntries)
        {
            RemoveEntry(entry);
        }

        scannerEntries.Clear();
    }

    private void HandleEntryKilled(WorldObject sender, DeathEventArgs e)
    {
        RemoveEntry(sender);
    }

    public void AddEntry(WorldObject objectToAdd)
    {
        if (scannerEntries.Contains(objectToAdd) || objectToAdd == owningShip) return;
    
        scannerEntries.Add(objectToAdd);

        objectToAdd.Killed += HandleEntryKilled;

        if (EntryChanged != null) EntryChanged(objectToAdd, true);
    }

    public void RemoveEntry(WorldObject objectToRemove)
    {
        if (!scannerEntries.Contains(objectToRemove)) return;

        scannerEntries.Remove(objectToRemove);

        objectToRemove.Killed -= HandleEntryKilled;

        if (EntryChanged != null) EntryChanged(objectToRemove, false);
    }

    private IEnumerator ScanCoroutine()
    {
        for (; ;)
        {
            Scan();
            yield return new WaitForSeconds(scanFrequency);
        }
    }
}
