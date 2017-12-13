using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour {

	public float maxMusicVolume;

	void Update () {
		gameObject.GetComponent<AudioSource>().volume = maxMusicVolume * SettingsManager.gameSettings.masterVolume;
	}
}
