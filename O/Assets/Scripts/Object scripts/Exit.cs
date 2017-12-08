using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ExitOptions {And, P1, P2};

public class Exit : MonoBehaviour {

	public AudioClip levelComplete;

	[HideInInspector]
	public GameObject GameController;

	//whether or not each player is required to reach this exit
	//in order to beat the level. Options are self-explanatory.
	//we may want this to be loaded for each level with the serializer,
	//but that will come once we start messing with different types of doors.
	public ExitOptions ExitSetting;

	//whether or not each player is currently in the exit.
	[HideInInspector]
	public bool p1Colliding = false;
	[HideInInspector]
	public bool p2Colliding = false;

	void Start() {
		if (ExitSetting == ExitOptions.P1) {
			EventManager.StartListening ("Find_P1_exit", Try_to_exit);
		} else if (ExitSetting == ExitOptions.P2) {
			EventManager.StartListening ("Find_P2_exit", Try_to_exit);
		}
	}

	void Try_to_exit() {
		if (ExitSetting == ExitOptions.P1 && p1Colliding) {
			loadNext ();
		} else if (ExitSetting == ExitOptions.P2 && p2Colliding) {
			loadNext ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name.Contains("Player 1")) {
			p1Colliding = true;
			if (ExitSetting == ExitOptions.P1) {
				EventManager.TriggerEvent ("Find_P2_exit");
			}
		} else if (other.name.Contains("Player 2")) {
			p2Colliding = true;
			if (ExitSetting == ExitOptions.P2) {
				EventManager.TriggerEvent ("Find_P1_exit");
			}
		}

		if (ExitSetting == ExitOptions.And) {
			checkAndFinish ();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.name.Contains("Player 1")) {
			p1Colliding = false;
		} else if (other.name.Contains("Player 2")) {
			p2Colliding = false;
		}
	}

	void checkAndFinish() {
		if (p1Colliding && p2Colliding) {
			loadNext ();
		}
	}

	void loadNext() {
		if (GameController == null)
			GameController = GameObject.FindGameObjectWithTag ("GameController");

		// Play level-success sound
		if (SettingsManager.gameSettings != null && levelComplete != null) {
			float volume = SettingsManager.gameSettings.masterVolume;
			GameController.GetComponent<AudioSource> ().PlayOneShot (levelComplete, volume);
		}

		int currentLevel = GameController.GetComponent<Stats> ().currLevel;

		if (!(SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Demo"))) {
			if (LevelPersistence.levelData != null) {
				LevelPersistence.levelData.saveLevelProgress (currentLevel);
			}
		}

		GameController.GetComponent<Levels> ().EndLevel ();
		GameController.GetComponent<Levels> ().LoadLevel (currentLevel + 1);
	}
}