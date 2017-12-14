using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
	//public Toggle fullscreenToggle;
	//public Slider masterVolumeSlider;

	public static SettingsManager settingsManager;

	public static GameSettings gameSettings;

	void Awake () {
		if (settingsManager == null) {
			gameSettings = new GameSettings ();
			gameSettings.masterVolume = 1.0f;
			gameSettings.localMusicVolume = 1.0f;
			gameSettings.localSFXVolume = 1.0f;
			UpdateFinalVolumes ();

			DontDestroyOnLoad (gameObject);
			settingsManager = this;

		} else if (settingsManager != this) {
			Destroy (gameObject);
		}
	}

	void UpdateFinalVolumes() {
		gameSettings.musicVolume = gameSettings.masterVolume * gameSettings.localMusicVolume;
		gameSettings.SFXVolume = gameSettings.masterVolume * gameSettings.localSFXVolume;
	}

	public static void changeMasterVolume(float newVolume) {
		gameSettings.masterVolume = newVolume;
		settingsManager.UpdateFinalVolumes ();
	}

	public static void changeMusicVolume(float newVolume) {
		gameSettings.localMusicVolume = newVolume;
		settingsManager.UpdateFinalVolumes ();
	}

	public static void changeSFXVolume(float newVolume) {
		gameSettings.localSFXVolume = newVolume;
		settingsManager.UpdateFinalVolumes ();
	}
}
