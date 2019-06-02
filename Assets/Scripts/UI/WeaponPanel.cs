using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : ShipUIElement
{
    public VerticalLayoutGroup vlg;
    public GameObject weaponPanelButton;

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

    public override void OnPossessed(PlayerController pc, PossessionEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public override void OnReleased(PlayerController pc, PossessionEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}