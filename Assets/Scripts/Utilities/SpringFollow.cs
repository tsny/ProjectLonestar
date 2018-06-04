using UnityEngine;

public class SpringFollow : MonoBehaviour
{
    public Transform target;
    public Camera springCamera;

    public float stiffness = 1800.0f;
    public float damping = 600.0f;
    public float mass = 50.0f;
    public Vector3 desiredOffset;
    public Vector3 lookAtOffset;

    private Vector3 desiredPosition = Vector3.zero;
    private Vector3 cameraVelocity = Vector3.zero;

    void FollowUpdate()
    {
        Vector3 stretch = springCamera.transform.position - desiredPosition;
        Vector3 force = -stiffness * stretch - damping * cameraVelocity;

        Vector3 acceleration = force / mass;

        cameraVelocity += acceleration * Time.deltaTime;

        springCamera.transform.position += cameraVelocity * Time.deltaTime;

        desiredPosition = target.position + desiredOffset;
        Vector3 lookat = target.position + lookAtOffset;

        transform.LookAt(lookat, target.up);
        transform.rotation = target.transform.rotation;
    }

    void FixedUpdate()
    {
        FollowUpdate();
    }
}

