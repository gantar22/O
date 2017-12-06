using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelPersistence : MonoBehaviour {

	public static LevelPersistence levelData;
	public static int highestLevel; // Tracks the highest level completed. If no data is saved, begins at -1 (no levels completed).
	public static int levelToLoad;

	// Make sure this is a singleton
	void Awake () {
		if (levelData == null) {
			DontDestroyOnLoad (gameObject);
			levelData = this;

			LoadLevelProgress ();
		} else if (levelData != this) {
			Destroy (gameObject);
		}
	}

	// Save level progress (where int level should be the index of the level completed, but 1-indexed)
	public void saveLevelProgress(int level) {
		if (level > highestLevel) {
			Save (level);
		}
	}

	// RESETS SAVED LEVEL PROGRESS TO -1!
	public void resetProgress() {
		Save (-1);
	}

	private static void Save(int level) {
		string path = Application.persistentDataPath + "/LevelProgress.dat";
		highestLevel = level;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;

		if (!File.Exists (path)) {
			file = File.Create (path);
		} else {
			file = File.Open (path, FileMode.Open);
		}

		SavedData data = new SavedData ();
		data.levelProgress = level;

		bf.Serialize (file, data);
		file.Close ();
	}

	// Load level progress
	private static void LoadLevelProgress() {
		string path = Application.persistentDataPath + "/LevelProgress.dat";

		if (File.Exists (path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);

			SavedData data = (SavedData)bf.Deserialize (file);
			file.Close ();

			highestLevel = data.levelProgress;
		} else {
			highestLevel = -1;
		}
	}

	// Continue the game at one level past the highest level beaten
	public void Continue() {
		startLevel (highestLevel + 1);
	}

	// Start the game at whichever level was selected
	public void startLevel (int levelNum) {
		Debug.Log ("Starting level " + levelNum);
		SceneManager.LoadScene (1);

		levelToLoad = levelNum;
	}

	// Start demo levels
	public void startDemo() {
		//SceneManager.LoadScene(2);
		//levelToLoad = 0;
		return;
	}
}

[Serializable]
class SavedData {
	public int levelProgress;
}