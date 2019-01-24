using UnityEngine;

public class ShipBase : MonoBehaviour 
{
    public MeshRenderer shipMesh;
    public Ship owner;

    void Awake() 
    {
        owner = GetComponentInParent<Ship>();

        if (owner == null) 
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (owner.ShipBase != gameObject) Destroy(gameObject);
    }
}