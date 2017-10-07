using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

	public GameObject GameController;

	//whether or not each player is required to reach this exit
	//in order to beat the level. At least one must be true.
	//we may want this to be loaded for each level, but that will
	//come once we start messing with different types of doors.
	public bool p1Exit = true;
	public bool p2Exit = true;

	//whether or not each player is currently in the exit.
	[HideInInspector]
	public bool p1Colliding = false;
	[HideInInspector]
	public bool p2Colliding = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player 1") {
			p1Colliding = true;
		} else if (other.name == "Player 2") {
			p2Colliding = true;
		}

		checkFinish ();
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Player 1") {
			p1Colliding = false;
		} else if (other.name == "Player 2") {
			p2Colliding = false;
		}

		checkFinish ();
	}

	//checks to see if each player is Finnish if they are required to be Finnish.
	void checkFinish() {
		Debug.Log ("p1Exit: " + p1Exit + ". p1Colliding: " + p1Colliding + "\n p2Exit: " + p2Exit + ". p2Colliding: " + p2Colliding);
		if ( (p1Exit && !p1Colliding) || (p2Exit && !p2Colliding)) {
			return;
		} else {
			int nextLevel = 1 + GameController.GetComponent<Stats> ().currLevel;

			GameController.GetComponent<Levels> ().EndLevel ();
			GameController.GetComponent<Levels> ().LoadLevel (nextLevel);
		}
	}
}
