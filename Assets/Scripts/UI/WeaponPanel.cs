using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponPanel : ShipUIElement
{
    public HardpointSystem hardpointSystem;
    public VerticalLayoutGroup vlg;

    public GameObject weaponPanelButton;

    public override void SetShip(Ship ship)
    {
        base.SetShip(ship);

        hardpointSystem = ship.hardpointSystem;
        ClearPanel();
        PopulatePanel();
    }

    private void PopulatePanel()
    {
        foreach (WeaponHardpoint weaponHardpoint in hardpointSystem.weaponHardpoints.Values)
        {
            if (!weaponHardpoint.IsMounted) continue;

            Instantiate(weaponPanelButton, vlg.transform).GetComponent<WeaponPanelButton>().Initialize(weaponHardpoint);
        }
    }

    private void ClearPanel()
    {
        foreach (Transform child in vlg.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
