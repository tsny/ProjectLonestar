using UnityEngine;
using System.Collections;

public class CollisionAvoidance : ShipComponent
{
    public float rayCastOffset = 5;
    public float detectionDistance = 20;

    public float obstacleDetectedTime;
    //public float turnSpeed = 5;

    public Engine engine;

    private void Update()
    {
        if (engine == null)
            return;

        var obstacleDetected = CheckForObstacle();

        if (obstacleDetected)
            obstacleDetectedTime += Time.deltaTime;

        else
            obstacleDetectedTime = 0;
    }

    public bool CheckForObstacle()
    {
        RaycastHit hit;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        Debug.DrawRay(left, detectionDistance * transform.forward, Color.cyan);
        Debug.DrawRay(right, detectionDistance * transform.forward, Color.cyan);
        Debug.DrawRay(up, detectionDistance * transform.forward, Color.cyan);
        Debug.DrawRay(down, detectionDistance * transform.forward, Color.cyan);


        if (Physics.Raycast(left, transform.forward, out hit, detectionDistance))
        {
            engine.Strafe = 1;
            return true;
        }

        else if (Physics.Raycast(right, transform.forward, out hit, detectionDistance))
        {
            engine.Strafe = -1;
            return true;
        }

        else if (Physics.Raycast(up, transform.forward, out hit, detectionDistance))
        {
            engine.Pitch(-engine.engineStats.turnSpeed);
            return true;
        }

        else if (Physics.Raycast(down, transform.forward, out hit, detectionDistance))
        {
            engine.Pitch(engine.engineStats.turnSpeed);
            return true;
        }

        else
        {
            engine.Strafe = 0;
            return false;
        }
    }
}
