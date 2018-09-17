using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item/Tractor")]
public class TractorBeam : Equipment
{
    public float range = 50;
    public float pullForce = .1f;
}
