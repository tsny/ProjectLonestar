using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponPanel : ShipUIElement
{
    public HardpointSystem hardpointSystem;
    public VerticalLayoutGroup vlg;

    public GameObject weaponPanelButton;

    protected override void HandlePossession(PlayerController sender, PossessionEventArgs e)
    {
        base.HandlePossession(sender, e);

        ClearPanel();

        foreach (WeaponHardpoint weaponHardpoint in e.newShip.hardpointSystem.weaponHardpoints.Values)
        {
            if (weaponHardpoint.IsMounted) continue;

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
