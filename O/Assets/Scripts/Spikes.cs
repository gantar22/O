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

			GameController.GetComponent<Levels> ().respawn ();
		}
	}
}
