using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateController : MonoBehaviour
{
    public Ship TargetShip { get { return targetTrans ? targetTrans.GetComponent<Ship>() : null; } }
    public float DistanceToTarget { get { return Vector3.Distance(ship.transform.position, targetTrans.position); } }
    public bool HasTarget { get { return targetTrans; } }

    public Ship ship;
    public Transform targetTrans;
    public bool importantToPlayer = true;

    public Ship[] allies;
    public Ship[] enemies;

    [Header("Gizmo")]

    public Vector3 gizmoOffset = new Vector3(0, 5, 0);
    public float gizmoRadius = 1;

    [Header("State")]

    public State stopState;
    public State currentState;
    public State remainState;

    public Queue<State> pastStates = new Queue<State>();

    public float maxPlayerDistance = 2000;
    public float gotoDistanceThreshold = 100;
    public float combatDistanceThreshold = 20;
    public float weaponsRange = 500;

    public bool aiIsActive;

    public float timeInCurrentState;

    private void Awake()
    {
        ship = GetComponent<Ship>();
        aiIsActive = true;
    }

    private void Update()
    {
        if (!aiIsActive) return;

        if (currentState == null)
        {
            aiIsActive = false;
            return;
        }

        currentState.UpdateState(this);
        timeInCurrentState += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawSphere(transform.position + gizmoOffset, gizmoRadius);
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState && nextState != null)
        {
            pastStates.Enqueue(currentState);
            currentState = nextState;
            timeInCurrentState = 0;
        }

        else if (nextState == null)
        {
            if (pastStates.Count >= 1)
            {
                currentState = pastStates.Dequeue();
            }

            else if (currentState == stopState)
            {
                currentState = null;
            }

            else
            {
                currentState = stopState;
            }

            timeInCurrentState = 0;
        }
    }
}
