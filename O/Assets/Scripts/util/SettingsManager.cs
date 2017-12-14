using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SettingsManager : MonoBehaviour {
	//public Toggle fullscreenToggle;
	//public Slider masterVolumeSlider;

	public static SettingsManager settingsManager;

	public static GameSettings gameSettings;

	void Awake () {
		if (settingsManager == null) {
			if (!LoadSettings ()) {
				gameSettings = new GameSettings ();
				gameSettings.masterVolume = 1.0f;
				gameSettings.localMusicVolume = 1.0f;
				gameSettings.localSFXVolume = 1.0f;
				UpdateFinalVolumes ();
			}

			DontDestroyOnLoad (gameObject);
			settingsManager = this;

		} else if (settingsManager != this) {
			Destroy (gameObject);
		}
	}

	void UpdateFinalVolumes() {
		gameSettings.musicVolume = gameSettings.masterVolume * gameSettings.localMusicVolume;
		gameSettings.SFXVolume = gameSettings.masterVolume * gameSettings.localSFXVolume;
		SaveSettings ();
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

	// Persistence (Saving and loading)
	private static void SaveSettings() {
		string path = Application.persistentDataPath + "/Settings.dat";

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;

		if (!File.Exists (path)) {
			file = File.Create (path);
		} else {
			file = File.Open (path, FileMode.Open);
		}

		bf.Serialize (file, gameSettings);
		file.Close ();
	}

	// Load level progress
	private static bool LoadSettings() {
		string path = Application.persistentDataPath + "/Settings.dat";

		if (File.Exists (path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);

			gameSettings = (GameSettings)bf.Deserialize (file);
			file.Close ();

			return true;
		}

		return false;
	}
}
