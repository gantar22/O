using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public EventSystem EventSystem;
	public GameObject defaultSelect;
	public GameObject defaultUpSelect;

	public Toggle FullscreenToggle;
	public Slider MasterVolumeSlider;

	private bool buttonSelected;

	void onEnable() {
		if (FullscreenToggle != null)
			FullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); } );

		if (MasterVolumeSlider != null)
			MasterVolumeSlider.onValueChanged.AddListener (delegate { OnMasterVolumeChange (); });
	}

	void Update() {
		if (EventSystem == null)
			Debug.LogError("Missing event system in MenuManager script");

		if (Input.GetAxisRaw ("Vertical") != 0 && !buttonSelected) {
			print ("Got axis");
			if (Input.GetAxisRaw ("Vertical") == -1) {
				EventSystem.SetSelectedGameObject (defaultSelect);
			} else {
				EventSystem.SetSelectedGameObject (defaultUpSelect);
			}

			buttonSelected = true;
		}
	}

	private void OnDisable() {
		buttonSelected = false;
	}

	public void OnFullscreenToggle() {
		Screen.fullScreen = FullscreenToggle.isOn;
		SettingsManager.gameSettings.fullscreen = FullscreenToggle.isOn;
	}

	public void OnMasterVolumeChange() {
		//AudioSource.volume = masterVolumeSlider.value;
		SettingsManager.gameSettings.masterVolume = MasterVolumeSlider.value;
	}

	public void MainMenu() {
		SceneManager.LoadScene (0);
	}

	public void Quit() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

	// Static level persistence calls
	public void Continue() {
		LevelPersistence.levelData.Continue ();
	}
		
	public void startLevel (int levelNum) {
		LevelPersistence.levelData.startLevel (levelNum);
	}
		
	public void startDemo() {
		LevelPersistence.levelData.startDemo ();
	}

	public void resetProgress() {
		LevelPersistence.levelData.resetProgress ();
	}
}
