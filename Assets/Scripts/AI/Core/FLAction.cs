using UnityEngine;
using System.Collections;

public abstract class FLAction : ScriptableObject
{
    public abstract void Act(StateController controller); 
}

