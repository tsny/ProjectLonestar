using UnityEngine;
using System.Collections;

[System.Serializable]
public class Transition 
{
    public FLDecision decision;
    public State trueState;
    public State falseState;
}
