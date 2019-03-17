using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour 
{
    public Image playerIcon;
    public Transform transformToMirror;

    private void FixedUpdate() 
    {
        if (transformToMirror == null) return;
        var tempRot = transformToMirror.rotation.eulerAngles;
        var newRot = new Vector3(0, 0, tempRot.z);
        playerIcon.rectTransform.rotation = Quaternion.Euler(newRot);
    }
}