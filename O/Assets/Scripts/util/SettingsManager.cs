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

			DontDestroyOnLoad (gameObject);
			settingsManager = this;

		} else if (settingsManager != this) {
			Destroy (gameObject);
		}
	}
}
