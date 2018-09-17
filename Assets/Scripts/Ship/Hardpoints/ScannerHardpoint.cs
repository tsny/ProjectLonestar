using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScannerHardpoint : Hardpoint
{
    public TractorHardpoint tractor;
    public List<WorldObject> detectedObjects;
    public Scanner scanner;

    public int scanFrequency = 10;

    public delegate void EntryChangeEventHandler(WorldObject entry, bool added);
    public event EntryChangeEventHandler EntryChanged;

    protected override bool EquipmentMatchesHardpoint(Equipment equipment)
    {
        return equipment is Scanner;
    }

    private void OnDestroy()
    {
        detectedObjects.Clear();
    }

    public void Scan()
    {
        if (scanner == null) return;

        WorldObject[] scannedObjects = FindObjectsOfType<WorldObject>();

        foreach (WorldObject scannedObject in scannedObjects)
        {
            AddEntry(scannedObject);
        }
    }

    public void AddEntry(WorldObject objectToAdd)
    {
        if (detectedObjects.Contains(objectToAdd) || objectToAdd == owningShip) return;
    
        detectedObjects.Add(objectToAdd);

        objectToAdd.Killed += HandleEntryKilled;

        if (EntryChanged != null) EntryChanged(objectToAdd, true);
    }

    private void HandleEntryKilled(WorldObject sender, DeathEventArgs e)
    {
        RemoveEntry(sender);
    }

    public void RemoveEntry(WorldObject objectToRemove)
    {
        if (!detectedObjects.Contains(objectToRemove)) return;

        detectedObjects.Remove(objectToRemove);

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
