using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScannerPanelButton : MonoBehaviour
{
    private Transform target;
    private Ship owner;
    public Text text;

    public void Setup(ITargetable target, Ship owner)
    {
        this.owner = owner;

        var monoBehaviour = target as MonoBehaviour;

        if (monoBehaviour != null)
        {
            this.target = monoBehaviour.transform;
        }

        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        for (; ;)
        {
            var distance = (int) Vector3.Distance(target.transform.position, owner.transform.position) + "M";
            text.text = target.name + " " + distance;

            yield return new WaitForSeconds(2);
        }
    }
}
