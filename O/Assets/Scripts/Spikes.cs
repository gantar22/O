using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

	[HideInInspector]
	public GameObject GameController;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name.Contains("Player 1") || other.name.Contains("Player 2")) {
			if (GameController == null)
				GameController = GameObject.FindGameObjectWithTag ("GameController");

			int currLevel = GameController.GetComponent<Stats> ().currLevel;
			GameController.GetComponent<Levels> ().EndLevel ();
			GameController.GetComponent<Levels> ().LoadLevel (currLevel);
		}
	}
}
