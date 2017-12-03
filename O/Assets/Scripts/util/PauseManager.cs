using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	public GameObject PausePanel;
	private bool paused = false;

	void Update () {
		// Pause keys
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Keypad7)) {
			if (paused) {
				resume ();
			} else {
				pause ();
			}
		}

		// Menu navigation

	}

	public void pause() {
		paused = true;
		Time.timeScale = 0f;
		PausePanel.SetActive (true);
	}

	public void resume() {
		paused = false;
		Time.timeScale = 1.0f;
		PausePanel.SetActive (false);
	}
}
