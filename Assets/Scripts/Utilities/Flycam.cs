/*
EXTENDED FLYCAM
    Desi Quintans (CowfaceGames.com), 17 August 2012.
    Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
    Updated 5 Sept 2018 by tsny

LICENSE
    Free as in speech, and free as in beer.

FEATURES

WASD/Arrows:    Movement
Q:              Climb
E:              Drop
Shift:          Move faster
Control:        Move slower
F:              Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).

*/
using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Camera), typeof(AudioListener))]
public class Flycam : MonoBehaviour
{
    [Header("Rotation")]
    public float pitchSensitivity = 2;
    public float yawSensitivity = 2;
    public float minPitch = -90;
    public float maxPitch = 90;

    [Range(0, 1)]
    public float rotSmoothFactor = .85f;

    [Header("Movement")]
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;

    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            TakeScreenshot();

        Movement();

        if (Cursor.lockState == CursorLockMode.Locked) Rotate();
    }

    public static String TakeScreenshot(string filePrefix = "lonestar", string dateformat = "yyyyMMddHHmmssfff")
    {
        var filename = filePrefix + DateTime.Now.ToString(dateformat) + ".png";
        ScreenCapture.CaptureScreenshot(filename);
        print("Took screenshot: " + filename);
        return filename;
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    void Rotate()
    {
        var mouseInput = new Vector3(Mathf.Clamp(-Input.GetAxis("Mouse Y") * pitchSensitivity, minPitch, maxPitch), Input.GetAxis("Mouse X") * yawSensitivity);
        transform.rotation *= Quaternion.Euler(mouseInput);
    }
}