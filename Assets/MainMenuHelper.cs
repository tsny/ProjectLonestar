using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHelper : MonoBehaviour 
{
	public Ship[] shipsToPutAtFullThrottle;

	private void Start() 
	{
		foreach (var ship in shipsToPutAtFullThrottle)
		{
			ship.engine.Throttle = 1;	
		}
	}
}
