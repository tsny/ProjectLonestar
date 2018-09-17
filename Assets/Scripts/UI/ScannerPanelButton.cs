using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScannerPanelButton : MonoBehaviour
{
    private WorldObject target;
    private Ship owner;
    public Text text;

    private void Awake()
    {
        enabled = false;
    }

    public void Setup(WorldObject target, Ship owner)
    {
        this.target = target;
        this.owner = owner;
        target.Killed += HandleTargetKilled;
        enabled = true;
    }

    private void HandleTargetKilled(WorldObject sender, DeathEventArgs e)
    {
        target.Killed -= HandleTargetKilled;
        Destroy(gameObject);
    }

    private void Update()
    {
        var distance = (int) Vector3.Distance(target.transform.position, owner.transform.position) + "M";
        text.text = target.ToStringForScannerEntry() + " - " + distance;
    }
}
