using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHelper : MonoBehaviour 
{
	public Ship[] shipsToPutAtFullThrottle;
	public Rigidbody cameraRb;

	private void Start() 
	{
		foreach (var ship in shipsToPutAtFullThrottle)
		{
			ship.engine.Throttle = 1;	
		}
	}

	// This will happen and then a fade out?
	public void Activate()
	{
		cameraRb.velocity = new Vector3(0, -5, 0);
	}
}
