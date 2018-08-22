using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponPanel : ShipUIElement
{
    public HardpointSystem hardpointSystem;
    public VerticalLayoutGroup vlg;

    public GameObject weaponPanelButton;

    protected override void HandlePossessed(PlayerController sender, Ship newShip)
    {
        base.HandlePossessed(sender, newShip);

        hardpointSystem = newShip.hardpointSystem;

        ClearPanel();
        PopulatePanel();
    }

    protected override void HandleUnpossessed(PlayerController sender, Ship oldShip)
    {
        base.HandleUnpossessed(sender, oldShip);

        hardpointSystem = null;

        ClearPanel();
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
