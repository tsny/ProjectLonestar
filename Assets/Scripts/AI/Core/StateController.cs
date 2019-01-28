using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateController : MonoBehaviour
{
    private Ship _targetShip;
    public Ship TargetShip 
    { 
        get 
        { 
            return _targetShip;

        } 
        set
        {
            _targetShip = value;
        }
    }
    public Transform TargetTransform { get { return _target.transform; } } 
    public float DistanceToTarget { get { return TargetTransform ? Vector3.Distance(ship.transform.position, TargetTransform.position) : 0; } }
    public bool HasTarget { get { return Target != null; } }

    [HideInInspector] public Ship ship;
    public GameObject _target;
    public GameObject Target
    {
        get
        {
            return _target;
        }
        set
        {
            if (!value) return;
            _targetShip = value.GetComponent<Ship>();
            _target = value;
        }
    }
    public bool importantToPlayer = true;

    public Ship[] allies;
    public Ship[] enemies;

    [Header("Gizmo")]

    public Vector3 gizmoOffset = new Vector3(0, 5, 0);
    public float gizmoRadius = 1;

    [Header("State")]

    public bool aiIsActive;
    public State stopState;
    public State currentState;
    public State remainState;

    public Queue<State> pastStates = new Queue<State>();

    [Header("Details")]
    public float maxPlayerDistance = 2000;
    public float gotoDistanceThreshold = 100;
    public float combatDistanceThreshold = 20;
    public float weaponsRange = 500;

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
