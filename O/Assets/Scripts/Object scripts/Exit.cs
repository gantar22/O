using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ExitOptions {And, P1, P2};

public class Exit : MonoBehaviour {

	public AudioClip levelComplete;

	private Vector3 camPos;
	private bool trans;

	[HideInInspector]
	public GameObject GameController;


	//whether or not each player is required to reach this exit
	//in order to beat the level. Options are self-explanatory.
	//we may want this to be loaded for each level with the serializer,
	//but that will come once we start messing with different types of doors.
	public ExitOptions ExitSetting;

	//whether or not each player is currently in the exit.
	[HideInInspector]
	public bool p1Colliding = false;
	[HideInInspector]
	public bool p2Colliding = false;
	private Camera camera;

	//If the level has already been completed, but the animation is playing
	private bool loadingNext = false;

	void Start() {
		camera = Camera.main;
		camPos = Vector3.forward * -9;
		if (ExitSetting == ExitOptions.P1) {
			EventManager.StartListening ("Find_P1_exit", Try_to_exit);
		} else if (ExitSetting == ExitOptions.P2) {
			EventManager.StartListening ("Find_P2_exit", Try_to_exit);
		}
		Invoke("init",.2f);
	}




	IEnumerator resize(){
		trans = true;
		float elapsed = 0f;
		float[] r = GameController.GetComponent<Levels>().nextLevelData();
		float oldSize = camera.orthographicSize;
		float oldX = camera.transform.position.x;
		float rot = 0f;
		while(elapsed < 4.1f){
			rot = Mathf.Lerp(0,180,elapsed / 4);
			camera.transform.eulerAngles = Vector3.forward * rot;
			camera.orthographicSize = Mathf.Lerp(oldSize,r[0],elapsed / 4);
			camPos = new Vector3(Mathf.Lerp(oldX, r[1], elapsed / 4),
				Mathf.Lerp(0,200,elapsed / 4),camera.transform.position.z);
			Camera.main.transform.position = camPos;
			//Vector3 dest = new Vector3(r[1],40,camera.transform.position.z);
			//Vector3 velo = Vector3.zero;
			//camera.reposition = Vector3.SmoothDamp(camera.transform.position,dest,ref velo,4);
			elapsed += Time.deltaTime;
			yield return null;	
		}
		trans = false;
	}

	void Update(){
		if(trans) Camera.main.transform.position = camPos;
	}


	void init(){
		p2Colliding = false;
		p1Colliding = false;
	}

	void Try_to_exit() {
		if (ExitSetting == ExitOptions.P1 && p1Colliding) {
			//loadNext ();
		} else if (ExitSetting == ExitOptions.P2 && p2Colliding) {
			//loadNext ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name.Contains("Player 1")) {
			p1Colliding = true;
			if (ExitSetting == ExitOptions.P1) {
				EventManager.TriggerEvent ("Find_P2_exit");
			}
		} else if (other.name.Contains("Player 2")) {
			p2Colliding = true;
			if (ExitSetting == ExitOptions.P2) {
				EventManager.TriggerEvent ("Find_P1_exit");
			}
		}

		if (ExitSetting == ExitOptions.And) {
			checkAndFinish ();
		}

	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.name.Contains("Player 1")) {
			p1Colliding = false;
		} else if (other.name.Contains("Player 2")) {
			p2Colliding = false;
		}
	}

	void checkAndFinish() {
		if (p1Colliding && p2Colliding) {
			loadNext ();
		}
	}

	void loadNext() {
		if (loadingNext)
			return;
		loadingNext = true;

		if (GameController == null)
			GameController = GameObject.FindGameObjectWithTag ("GameController");


		// Play level-success sound
		if (SettingsManager.gameSettings != null && levelComplete != null) {
			float volume = SettingsManager.gameSettings.SFXVolume;
			GameController.GetComponent<AudioSource> ().PlayOneShot (levelComplete, volume);
		}
		EventManager.TriggerEvent("hitdoor");
		float[] r = GameController.GetComponent<Levels>().nextLevelData();
		//Camera.main.gameObject.GetComponent<transition>().activate(r[0],r[1]);
		StartCoroutine(resize());

		gameObject.GetComponent<Animator>().SetTrigger("suc");
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("end");
		GameController.GetComponent<AudioSource> ().PlayOneShot (levelComplete, SettingsManager.gameSettings.masterVolume);
		Invoke("finish",4f);
	}


	void finish(){
		if (GameController == null)
			GameController = GameObject.FindGameObjectWithTag ("GameController");

		int currentLevel = GameController.GetComponent<Stats> ().currLevel;
		GameController.GetComponent<Stats> ().currLevel++;

		if (!(SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Demo"))) {
			if (LevelPersistence.levelData != null) {
				LevelPersistence.levelData.saveLevelProgress (currentLevel);

			}
		}

/*		GameController.GetComponent<Levels> ().EndLevel ();
		GameController.GetComponent<Levels> ().LoadLevel (currentLevel + 1);*/

		GameController.GetComponent<LoadLevel>().reset = true;
		GameController.GetComponent<LoadLevel>().level = currentLevel + 1;

		GameController.GetComponent<LoadLevel>().load();
		camera.transform.eulerAngles = Vector3.zero;
	}


}