using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Scanner : Hardpoint 
{
    public List<ITargetable> targets = new List<ITargetable>();

    public ScannerStats scanner;

    public int scanFrequency = 10;

    public event ScannedEventHandler ScannerUpdated;
    public delegate void ScannedEventHandler(Scanner sender, List<ITargetable> targets);

    private void Start()
    {
        //StartCoroutine(ScanCoroutine());
    }

    public void ScanForTargets()
    {
        if (scanner == null) return;

        var scannedTargets = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>().ToList();

        ValidateNewTargets(scannedTargets);

        targets = scannedTargets;

        if (ScannerUpdated != null) ScannerUpdated(this, targets);
    }

    public void ClearList()
    {
        targets.Clear();

        if (ScannerUpdated != null) ScannerUpdated(this, targets);
    }

    private void ValidateNewTargets(List<ITargetable> newTargets)
    {
        var targetsToIgnore = ship.GetComponentsInChildren<MonoBehaviour>().OfType<ITargetable>();

        newTargets.RemoveAll(x => targetsToIgnore.Contains(x));
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
