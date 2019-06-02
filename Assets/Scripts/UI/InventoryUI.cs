using System.Collections;
using UnityEngine;

public class InventoryUI : ShipUIElement
{
    public override void OnPossessed(PlayerController pc, PossessionEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public override void OnReleased(PlayerController pc, PossessionEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public void Toggle() { gameObject.SetActive(!gameObject.activeSelf); }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}