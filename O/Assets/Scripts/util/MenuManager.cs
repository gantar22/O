using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public EventSystem EventSystem;
	public GameObject defaultSelect;
	public GameObject defaultUpSelect;

	private bool buttonSelected;

	public void Update() {
		if (EventSystem == null)
			Debug.LogError("Missing event system in MenuManager script");

		if (Input.GetAxisRaw ("Vertical") != 0 && !buttonSelected) {
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
