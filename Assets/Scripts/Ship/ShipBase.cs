using UnityEngine;

[ExecuteInEditMode]
public class ShipBase : MonoBehaviour 
{
    public MeshRenderer shipMesh;
    public Ship owner;

    void Awake() 
    {
        if (owner == null)
            owner = GetComponentInParent<Ship>();
    }
}