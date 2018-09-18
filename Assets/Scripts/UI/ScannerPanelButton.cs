using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScannerPanelButton : MonoBehaviour
{
    private WorldObject target;
    private Ship owner;
    public Text text;

    public void Setup(WorldObject target, Ship owner)
    {
        this.target = target;
        this.owner = owner;
        target.Killed += HandleTargetKilled;

        StartCoroutine(UpdateText());
    }

    private void HandleTargetKilled(WorldObject sender, DeathEventArgs e)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        target.Killed -= HandleTargetKilled;
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
