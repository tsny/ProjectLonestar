using UnityEngine;
using System.Collections;

public abstract class FLAction : ScriptableObject
{
    public abstract void Init();
    public abstract void Act(StateController controller); 
}

