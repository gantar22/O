using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	public GameObject PausePanel;
	public GameObject OptionsPanel;
	private bool pressed;

	[HideInInspector]
	public static bool paused = false;

	void Update () {
		// Pause keys
		if (Input.GetAxisRaw("Cancel") > 0) {
			if (paused && !pressed) {
				pressed = true;
				resume ();
			} else if (!pressed) {
				pressed = true;
				pause ();
			} 
		} else {
			pressed = false;
		}

		// Reset keys
		if (Input.GetAxis("Restart") > 0) {
			Invoke("restart",2);
		} else {
			CancelInvoke("restart");
		}
	}

	void restart()
	{
		gameObject.GetComponent<Levels> ().restart ();
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
		OptionsPanel.SetActive (false);
	}
}
