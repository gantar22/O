using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
	//public Toggle fullscreenToggle;
	//public Slider masterVolumeSlider;

	public static SettingsManager settingsManager;

	public static GameSettings gameSettings;

	void onEnable() {
		gameSettings = new GameSettings ();
	}

	void Awake () {
		if (settingsManager == null) {
			DontDestroyOnLoad (gameObject);
			settingsManager = this;

		} else if (settingsManager != this) {
			Destroy (gameObject);
		}
	}
}
