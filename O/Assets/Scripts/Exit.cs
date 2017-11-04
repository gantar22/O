using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExitOptions {And, Or, P1, P2};

public class Exit : MonoBehaviour {

	[HideInInspector]
	public GameObject GameController;

	//whether or not each player is required to reach this exit
	//in order to beat the level. Options are self-explanatory.
	//we may want this to be loaded for each level with the serializer,
	//but that will come once we start messing with different types of doors.
	public ExitOptions ExitSetting;
	[HideInInspector]
	public bool p1Exit;
	[HideInInspector]
	public bool p2Exit;

	//whether or not each player is currently in the exit.
	[HideInInspector]
	public bool p1Colliding = false;
	[HideInInspector]
	public bool p2Colliding = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name.Contains("Player 1")) {
			p1Colliding = true;
		} else if (other.name.Contains("Player 2")) {
			p2Colliding = true;
		}

		checkFinish ();
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.name.Contains("Player 1")) {
			p1Colliding = false;
		} else if (other.name.Contains("Player 2")) {
			p2Colliding = false;
		}

		checkFinish ();
	}

	//checks to see if each player is Finnish if they are required to be Finnish.
	void checkFinish() {
		p1Exit = true;
		p2Exit = true;

		if (ExitSetting == ExitOptions.P2 || ExitSetting == ExitOptions.Or) {
			p1Exit = false;
		}
		if (ExitSetting == ExitOptions.P1 || ExitSetting == ExitOptions.Or) {
			p2Exit = false;
		}
			
		bool OrNotSatisfied = (ExitSetting == ExitOptions.Or && !p1Colliding && !p2Colliding);

		if ((p1Exit && !p1Colliding) || (p2Exit && !p2Colliding) || OrNotSatisfied) {
			return;
		} else {
			if (GameController == null)
				GameController = GameObject.FindGameObjectWithTag ("GameController");
			int nextLevel = 1 + GameController.GetComponent<Stats> ().currLevel;

			GameController.GetComponent<Levels> ().EndLevel ();
			GameController.GetComponent<Levels> ().LoadLevel (nextLevel);
		}
	}
}