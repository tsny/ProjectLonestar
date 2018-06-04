using UnityEngine;
using System.Collections;

public abstract class FLDecision : ScriptableObject
{
    public abstract bool Decide(StateController controller);
}
