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

    protected override void ClearShip()
    {
        ClearPanel();
        base.ClearShip();
    }

    private void PopulatePanel()
    {
        foreach (Gun gun in hardpointSystem.guns)
        {
            if (gun.projectilePrefab == null) continue;

            Instantiate(weaponPanelButton, vlg.transform).GetComponent<GunPanelButton>().Initialize(gun);
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
