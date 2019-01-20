# Project Lonestar Changelog

## 01/20/19

It's been quite a while since I've updated the changelog, but like a more professional project I would like to
begin updating the changelog as issues come and go.

### Gameplay

* Some basic AI is in!
	* Ships use a slightly modified version of Unity's PluggableAI which uses Scriptable Objects
	* My modificaton is an addition of a queue that the agent adds to whenever it moves to a new state so that it can fall
	back on it's last state if it reaches a null decision (still allows for the 'remain' state feature)
	* To make the game feel a little more lively, there are a couple random ships just wandering throughout the system, this will probably
	feel really nice when they can communicate/attack/drop loot.

* Testing some basic UI notifications! (Imagine the (Harvested x10 Wood) notification from Rust)
* Target Indicator are quicker and state driven rather than created manually
* Weapon panel has buttons for nanobots/shieldbots (not programmed yet)
* Asteroid fields now apply slight random spinning to created asteroids

* UI
	* Sidestep and Blink cooldown indicators are in the bottom left corner
	* New logos!


### Misc

* You'll now find a few random bases and planets throughout the map
* Tradelane placeholder models are in and they look... amazing...

* New console command: "last"
	* Use this to repossess your last ship if you released it and want to get back to it without restarting the scene

### Performance
* Projectiles are slightly more performant and should hit their target more often



## 09/08/18

* Placeholder ambient background audio
* Engine effects
	* Cruise engine effects!
* Quit and restart debug commands
* Refactoring and speed improvements
	* Should be less stuttering if any was experienced before

Made possessing easier to improve and more modular on my end

## 09/07/18

* Changelog begins
* Flycam improved, no more stuttering
* Debug console now uses reflection to find all debug commands and populates itself on start
	* Features:
		* You can now spawn a default ship and possess it if you are stuck in flycam
		* You can now quit the game
* New skyboxes!
	* Thanks to Hedgehog Team

