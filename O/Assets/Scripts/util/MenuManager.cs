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
	private bool delayed;

	void OnEnable() {
		if (FullscreenToggle != null) {
			FullscreenToggle.onValueChanged.AddListener (delegate {
				OnFullscreenToggle ();
			});
		}

		if (MasterVolumeSlider != null) {
			MasterVolumeSlider.value = SettingsManager.gameSettings.masterVolume;
			MasterVolumeSlider.onValueChanged.AddListener (delegate {
				OnMasterVolumeChange ();
			});
		}
	}

	void Update() {
		if (EventSystem == null)
			Debug.LogError("Missing event system in MenuManager script");

		float vertical = Input.GetAxisRaw ("Vertical");
		if ((vertical == -1 || vertical == 1) && !buttonSelected) {
			print (vertical);
			if (vertical == -1) {
				EventSystem.SetSelectedGameObject (defaultSelect);
			} else if (vertical == 1) {
				EventSystem.SetSelectedGameObject (defaultUpSelect);
			}

			buttonSelected = true;
		}

		/* } else if (vertical == -1 || vertical == 1) {
			if (EventSystem.currentSelectedGameObject == defaultSelect && vertical == 1) {
				Invoke(()=>buttonSelected = false,0.2f);
				//buttonSelected = false;
			} else if (EventSystem.currentSelectedGameObject == defaultUpSelect && vertical == -1) {
				Invoke(()=>buttonSelected = false,0.2f);
			}
		} */
	}

	private void OnDisable() {
		buttonSelected = false;
		delayed = false;
	}

	private void undelay() {
		delayed = false;
	}

	public void OnFullscreenToggle() {
		Screen.fullScreen = FullscreenToggle.isOn;
		SettingsManager.gameSettings.fullscreen = FullscreenToggle.isOn;
	}

	public void OnMasterVolumeChange() {
		//AudioSource.volume = masterVolumeSlider.value;
		//Debug.Log("master volume changing to: " + MasterVolumeSlider.value);
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

	public void resetProgress() {
		LevelPersistence.levelData.resetProgress ();
	}
}
