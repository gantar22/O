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

	public Button backButton;

	public Toggle FullscreenToggle;
	public Slider MasterVolumeSlider;
	public Slider MusicVolumeSlider;
	public Slider SFXVolumeSlider;

	public static float backTimer;

	private bool buttonSelected;
	private GameObject prevSelected;

	private bool delayed;
	private int lastDirection;
	private float delayTimer;
	private float holdTimer;

	private float slowDelay = 0.65f;
	private float fastDelay = 0.14f;
	private float currentDelay;

	void OnEnable() {
		if (FullscreenToggle != null) {
			FullscreenToggle.onValueChanged.AddListener (delegate {
				OnFullscreenToggle ();
			});
		}

		if (MasterVolumeSlider != null) {
			MasterVolumeSlider.value = SettingsManager.gameSettings.masterVolume;
			MusicVolumeSlider.value = SettingsManager.gameSettings.localMusicVolume;
			SFXVolumeSlider.value = SettingsManager.gameSettings.localSFXVolume;

			MasterVolumeSlider.onValueChanged.AddListener (delegate {OnMasterVolumeChange ();} );
			MusicVolumeSlider.onValueChanged.AddListener (delegate {OnMusicVolumeChange ();} );
			SFXVolumeSlider.onValueChanged.AddListener (delegate {OnSFXVolumeChange ();} );
		}

		resetDelay ();
	}

	void Update() {
		if (EventSystem == null)
			Debug.LogError("Missing event system in MenuManager script");

		// Press 'b' on joystick or backspace on keyboard to go back
		backTimer += Time.deltaTime;
		if (Input.GetAxisRaw ("Back") > 0 && backButton != null && (backTimer >= 0.5f || PauseManager.paused)) {
			backButton.onClick.Invoke ();
			backTimer = 0;
		}

		// Default select the top or bottom of the menu
		if (EventSystem.currentSelectedGameObject == null) {
			buttonSelected = false;
		}

		float vertical = Input.GetAxisRaw ("Vertical");
		if ((vertical == -1 || vertical == 1) && !buttonSelected) {
			if (vertical == -1) {
				EventSystem.SetSelectedGameObject (defaultSelect);
			} else if (vertical == 1) {
				EventSystem.SetSelectedGameObject (defaultUpSelect);
			}

			buttonSelected = true;
		}

		// Wrapping top-to-bottom and bottom-to-top
		if (prevSelected != EventSystem.currentSelectedGameObject) {
			delayed = true;
			delayTimer = 0;
		}
		if (vertical != 0) {
			lastDirection = (int)vertical;
		}

		if ((vertical == -1 || vertical == 1) && buttonSelected) {
			if (prevSelected != EventSystem.currentSelectedGameObject) {
				delayed = true;
				delayTimer = 0;
			}

			if ((int)vertical == lastDirection) {
				delayTimer += Time.deltaTime;
				holdTimer += Time.deltaTime;
			} else {
				resetDelay ();
			}

			if (holdTimer >= slowDelay) {
				currentDelay = fastDelay;
			} else {
				currentDelay = slowDelay;
			}

			if (EventSystem.currentSelectedGameObject == defaultSelect && vertical == 1 && (!delayed || (delayTimer >= currentDelay)) ) {
				EventSystem.SetSelectedGameObject (defaultUpSelect);
				delayTimer = 0;
			} else if (EventSystem.currentSelectedGameObject == defaultUpSelect && vertical == -1 && (!delayed || (delayTimer >= currentDelay)) ) {
				EventSystem.SetSelectedGameObject (defaultSelect);
				delayTimer = 0;
			}

		} else {
			resetDelay ();
		}

		prevSelected = EventSystem.currentSelectedGameObject;
	}

	private void resetDelay() {
		delayTimer = 0;
		holdTimer = 0;
		lastDirection = 2;
		delayed = false;
	}

	private void OnDisable() {
		buttonSelected = false;
	}

	public void OnFullscreenToggle() {
		Screen.fullScreen = FullscreenToggle.isOn;
		SettingsManager.gameSettings.fullscreen = FullscreenToggle.isOn;
	}

	// Update volume settings
	public void OnMasterVolumeChange() {
		SettingsManager.changeMasterVolume(MasterVolumeSlider.value);
	}

	public void OnMusicVolumeChange() {
		SettingsManager.changeMusicVolume(MusicVolumeSlider.value);
	}

	public void OnSFXVolumeChange() {
		SettingsManager.changeSFXVolume(SFXVolumeSlider.value);
	}

	// Change the scene to the main menu
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

	public void resetProgress() {
		LevelPersistence.levelData.resetProgress ();
	}
}
