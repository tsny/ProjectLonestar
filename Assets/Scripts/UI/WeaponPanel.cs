using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponPanel : ShipUIElement
{
    public VerticalLayoutGroup vlg;
    public GameObject weaponPanelButton;

    public override void Init(Ship ship)
    {
        base.Init(ship);

        //hardpointSystem = ship.hardpointSystem;
        //ClearPanel();
        //PopulatePanel();
    }

    protected override void Clear()
    {
        ClearPanel();
        base.Clear();
    }

    private void PopulatePanel()
    {
        // foreach (Gun gun in hardpointSystem.Guns)
        // {
        //     if (gun.projectile == null) continue;

        //     Instantiate(weaponPanelButton, vlg.transform).GetComponent<GunPanelButton>().Initialize(gun);
        // }
    }

    private void ClearPanel()
    {
        foreach (Transform child in vlg.transform)
        {
            if (child.GetComponent<Button>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
