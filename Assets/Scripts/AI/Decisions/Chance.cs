using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AI/Decisions/Chance")]
public class Chance : FLDecision
{
    [Range(1, 100)]
    public float chance = 50;

    public override bool Decide(StateController controller)
    {
        return chance < Random.Range(1, 100);
    }
}