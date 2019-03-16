using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHelper : MonoBehaviour 
{
	public Ship[] shipsToPutAtFullThrottle;
	public Rigidbody cameraRb;
	public SceneHelper helper;

	private void Start() 
	{
		foreach (var ship in shipsToPutAtFullThrottle)
		{
			ship.engine.Throttle = 1;	
		}
	}

	private void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Return) && helper.HasStartedFadeIn)
		{
			helper.FadeToScene("SCN_Debug");
		}
	}

	// This will happen and then a fade out?
	public void Activate()
	{
		cameraRb.velocity = new Vector3(0, -5, 0);
	}
}
