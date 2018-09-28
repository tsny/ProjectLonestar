using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScannerPanelButton : MonoBehaviour
{
    //private ITargetable target;
    private Transform targetTransform;
    private Ship owner;
    public Text text;

    public void Setup(ITargetable target, Ship owner)
    {
        //this.target = target;
        this.owner = owner;

        var component = target as MonoBehaviour;

        if (component == null)
        {
            Destroy(gameObject);
            return;
        }

        targetTransform = component.transform;
        name = component.name;

        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        for (; ;)
        {
            if (targetTransform == null || owner == null) break;

            var distance = (int) Vector3.Distance(targetTransform.transform.position, owner.transform.position) + "M";
            text.text = targetTransform.name + " " + distance;

            yield return new WaitForSeconds(2);
        }
    }
}
