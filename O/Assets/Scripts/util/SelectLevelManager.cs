using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevelManager : MonoBehaviour {

	// Set level button interactable based on if it is unlocked or not
	void OnEnable () {
		int levelProgress = LevelPersistence.highestLevel;

		Button[] buttons = gameObject.GetComponentsInChildren<Button> ();
		foreach (Button butt in buttons) {
			if (int.Parse (butt.name) <= levelProgress + 1) {
				butt.interactable = true;
			}
		}
	}
}
