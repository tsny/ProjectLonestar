using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateController : MonoBehaviour
{
    public Ship ship;
    public Ship targetShip;

    public Transform targetLoc;
    public float acceptableDistance;

    public bool aiActive;

    public State currentState;
    public State remainState;

    public float timeInCurrentState;

    [HideInInspector] public int nextWaypoint;
    [HideInInspector] public List<Transform> wayPointList;

    public bool HasTarget
    {
        get
        {
            return targetLoc != null || targetShip != null;
        }
    }


    private void Awake()
    {
        ship = GetComponent<Ship>();
    }

    public void Setup()
    {

    }

    private void Update()
    {
        if (!aiActive)
            return;

        currentState.UpdateState(this);
        timeInCurrentState += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if(currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * .1f, .05f);
        }
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState)
        {
            currentState = nextState;
            timeInCurrentState = 0;
        }
    }
}
