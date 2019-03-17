using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHelper : SceneHelper
{
	public Ship[] shipsToPutAtFullThrottle;
	public Rigidbody cameraRb;
	public bool canTransitionToNewScene;

	protected override void Start() 
	{
		base.Start();

		foreach (var ship in shipsToPutAtFullThrottle)
		{
			ship.engine.Throttle = 1;	
		}
	}

	public void SetCanTransition()
	{
		canTransitionToNewScene = true;
	}

	public void LoadGameplayScene()
	{
		SceneManager.LoadScene("SCN_Debug");
	}

	private void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Return) && canTransitionToNewScene)
		{
			fade.fadeIn = false;
			fade.updateElapsed = true;
		}
	}
}
