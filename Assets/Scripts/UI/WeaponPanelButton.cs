using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponPanelButton : MonoBehaviour
{
    public WeaponHardpoint weaponHardpoint;
    public Image image;
    public Button button;
    public Text text;

    public Color enabledColor;
    public Color disabledColor;

    private void Awake()
    {
        enabledColor = button.colors.normalColor;
    }

    public void ToggleHardpoint()
    {
        weaponHardpoint.Toggle();
        SetName();
        SetColor();
    }

    public void Initialize(WeaponHardpoint weaponHardpoint)
    {
        this.weaponHardpoint = weaponHardpoint;
        SetName();
    }

    private void SetName()
    {
        //if (weaponHardpoint.active)
        //{
        //    text.text = weaponHardpoint.CurrentEquipment.name + " - Active";
        //}

        //else
        //{
        //    text.text = weaponHardpoint.CurrentEquipment.name + " - Inactive";
        //}
    }

    private void SetColor()
    {
        if (weaponHardpoint.active)
        {
            image.color = enabledColor;
        }

        else
        {
            image.color = disabledColor;
        }
    }
}
