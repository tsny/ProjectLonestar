using UnityEngine;
using UnityEngine.UI;

public class CooldownIndicator : MonoBehaviour 
{
    public Image image;    
    public Cooldown cd;

    private void Update()
    {
        if (image == null || cd == null) return;

        if (cd.isDecrementing)
        {
            image.fillAmount = cd.elapsed / cd.duration;
        }
        else
        {
            image.fillAmount = 1;
        }
    }
}