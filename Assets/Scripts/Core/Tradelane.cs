using UnityEngine;

public class Tradelane : MonoBehaviour, ITargetable
{
    public Transform transformToMove;
    public Health health;

    public event TargetEventHandler BecameTargetable;
    public event TargetEventHandler BecameUntargetable;

    private void Awake() 
    {
        health = Utilities.CheckComponent<Health>(gameObject);
    }

    public bool IsTargetable()
    {
        //throw new System.NotImplementedException();
        return true;
    }

    public void SetupTargetIndicator(TargetIndicator indicator)
    {
        //throw new System.NotImplementedException();
        indicator.header.text = "Trade Lane";
    }
}