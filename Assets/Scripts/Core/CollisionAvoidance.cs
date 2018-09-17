using UnityEngine;
using System.Collections;

public class CollisionAvoidance : ShipComponent
{
    //public float rayCastOffset;
    //public float detectionDistance;

    //public bool obstacleDetected;
    //public float obstacleDetectedTime;

    //public ShipEngine shipMovement;
    
    //private void Update()
    //{
    //    obstacleDetected = CheckForObstacle();

    //    if (obstacleDetected)
    //        obstacleDetectedTime += Time.deltaTime;

    //    else
    //        obstacleDetectedTime = 0;
    //}

    //public bool CheckForObstacle()
    //{
    //    RaycastHit hit;

    //    Vector3 left = transform.position - transform.right * rayCastOffset;
    //    Vector3 right = transform.position + transform.right * rayCastOffset;
    //    Vector3 up = transform.position + transform.up * rayCastOffset;
    //    Vector3 down = transform.position - transform.up * rayCastOffset;

    //    Debug.DrawRay(left, detectionDistance * transform.forward, Color.cyan);
    //    Debug.DrawRay(right, detectionDistance * transform.forward, Color.cyan);
    //    Debug.DrawRay(up, detectionDistance * transform.forward, Color.cyan);
    //    Debug.DrawRay(down, detectionDistance * transform.forward, Color.cyan);


    //    if (Physics.Raycast(left, transform.forward, out hit, detectionDistance))
    //    {
    //        shipMovement.ChangeStrafe(1);
    //        return true;
    //    }

    //    else if(Physics.Raycast(right, transform.forward, out hit, detectionDistance))
    //    {
    //        shipMovement.ChangeStrafe(-1);
    //        return true;
    //    }

    //    else if(Physics.Raycast(up, transform.forward, out hit, detectionDistance))
    //    {
    //        shipMovement.Rotate(0, shipMovement.turnSpeed);
    //        return true;
    //    }

    //    else if(Physics.Raycast(down, transform.forward, out hit, detectionDistance))
    //    {
    //        shipMovement.Rotate(0, -1);
    //        return true;
    //    }

    //    else
    //    {
    //        shipMovement.ResetStrafe();
    //        return false;
    //    }
    //}
}
