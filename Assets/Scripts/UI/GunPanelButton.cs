using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunPanelButton : MonoBehaviour
{
    public Gun gun;
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
        gun.Toggle();
        SetName();
        SetColor();
    }

    public void Initialize(Gun gun)
    {
        this.gun = gun;
        SetName();
    }

    private void SetName()
    {
        if (gun.projectile == null) return;

        var activeString = (gun.IsActive) ? " - Active" : " - Inactive";
        text.text = gun.stats.name + activeString;
    }

    private void SetColor()
    {
        image.color = (gun.IsActive) ? enabledColor : disabledColor;
    }
}
