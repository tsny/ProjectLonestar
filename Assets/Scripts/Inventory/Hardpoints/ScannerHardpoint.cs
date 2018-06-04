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

    public ScannerHardpoint()
    {
        associatedEquipmentType = typeof(Scanner);
    }

    public override void Mount(Equipment newEquipment)
    {
        base.Mount(newEquipment);

        scanner = newEquipment as Scanner;

        StartCoroutine("ScanCoroutine");
    }

    public override void Demount()
    {
        base.Demount();

        StopCoroutine("ScanCoroutine");
    }

    protected override void Awake()
    {
        base.Awake();
        hardpointSystem.scannerHardpoint = this;
        detectedObjects.Clear();
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

        objectToAdd.WorldObjectExploded += RemoveEntry;

        if (EntryChanged != null) EntryChanged(objectToAdd, true);
    }

    public void RemoveEntry(WorldObject objectToRemove)
    {
        if (!detectedObjects.Contains(objectToRemove)) return;

        detectedObjects.Remove(objectToRemove);

        objectToRemove.WorldObjectExploded -= RemoveEntry;

        if (EntryChanged != null) EntryChanged(objectToRemove, false);
    }

    public IEnumerator ScanCoroutine()
    {
        for (; ;)
        {
            Scan();
            yield return new WaitForSeconds(scanFrequency);
        }
    }
}
