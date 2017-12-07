using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour {

	[SerializeField]
	public bool updateSaveNewLevel;
	public Level[] levels;
	public int startingLevel;

	[HideInInspector]
	Stats stats;

	void Start() {

		stats = GetComponent<Stats> ();
		EndLevel ();
		LoadLevel (startingLevel);

	}

	public void restart() {
		int currLevel = stats.currLevel;
		EndLevel ();
		LoadLevel (currLevel);
	}

	public void respawn() {
		if (stats == null)
			stats = GetComponent<Stats> ();
		
		stats.respawning = true;
		restart ();
		stats.respawning = false;
	}

	public void EndLevel () {
		//destroy all level objects not to carry over into next level
		if (stats == null)
			stats = GetComponent<Stats> ();

		GameObject[] objs = GameObject.FindGameObjectsWithTag ("LevelObj");

		foreach (GameObject obj in objs) {
			if (!(obj == null)) {
				if (obj.GetComponent<Checkpoint> () != null && stats.respawning) {
					if (obj.GetComponent<Checkpoint> ().Checkpt_State == States.triggered)
						obj.GetComponent<Checkpoint> ().change_state (States.idle);
				} else {
					if (Application.isPlaying)
						Destroy (obj);
					else
						DestroyImmediate (obj);
				}
			}
		} 
	}

	public void LoadLevel(int num) {
		if (stats == null)
			stats = GetComponent<Stats> ();

		if (0 <= num && num < levels.Length) { //make sure level index is valid

			if (levels [num] != null) { //make sure level to load exists
				//update current level in Stats
				stats.currLevel = num;

			
				//set the level to be loaded
				Level level = levels [num];
				if (updateSaveNewLevel) {
					gameObject.GetComponent<SaveLevel> ().newLevel = level;
				}
					
				if (levels [num].dynamicCam) {
					Camera.main.GetComponent<DynamicCamera>().enabled = true;
				} else {
					Camera.main.GetComponent<DynamicCamera>().enabled = false;
					Camera.main.transform.position = new Vector3 (0, 0, Camera.main.transform.position.z);
				}
				if (!stats.respawning)
					Camera.main.orthographicSize = levels [num].cameraSize;

				stats.trig_P1_check = null;
				stats.trig_P2_check = null;

				LevelObject[] objs = level.components;

				//put all the objects in the level
				foreach (LevelObject obj in objs) {

					if (obj.type != null && !(obj.type.name.Contains("Checkpoint") && stats.respawning)) {

						GameObject comp = Instantiate (obj.type, obj.position, Quaternion.identity);
					
						comp.transform.eulerAngles = Vector3.forward * obj.rotation;
						if (obj.scale == Vector2.zero)
							comp.transform.localScale = Vector3.one;
						else
							comp.transform.localScale = new Vector3 (obj.scale.x, obj.scale.y, 1f);

						if (comp.name.Contains ("Player 1")) {

							if (!stats.respawning) {
								stats.P1_respawn = comp.transform.position;
								if (Application.isPlaying) {
									GameObject checkpt = Instantiate (this.GetComponentInParent<SaveLevel> ().CheckpointP1, comp.transform.position, Quaternion.identity);
									checkpt.GetComponent<Checkpoint> ().Checkpt_State = States.active;
								}
							}
							comp.transform.position = stats.P1_respawn - new Vector3(0, 0, 1);
						}

						if (comp.name.Contains ("Player 2")) {
							if (!stats.respawning) {
								stats.P2_respawn = comp.transform.position;
								if (Application.isPlaying) {
									GameObject checkpt = Instantiate (this.GetComponentInParent<SaveLevel> ().CheckpointP2, comp.transform.position, Quaternion.identity);
									checkpt.GetComponent<Checkpoint> ().Checkpt_State = States.active;
								}
							}
							comp.transform.position = stats.P2_respawn - new Vector3(0, 0, 1);
						}

						if (comp.GetComponent<Platform> () != null) {
							Platform plat = comp.GetComponent<Platform> ();
							plat.platformID = obj.platformID;
							plat.MoveSetting = obj.platMoveSetting;
							plat.translation = obj.platTranslation;
							plat.travelTime = obj.platTravelTime;
							plat.moveDelay = obj.platMoveDelay;
							plat.manualMapping = obj.platManualMapping;
							plat.horizontalMoveSpeed = obj.platHorizontalMoveSpeed;
							plat.verticalMoveSpeed = obj.platVerticalMoveSpeed;
							plat.returnOnUntrigger = obj.platReturnOnUntrigger;

							comp.transform.localScale = new Vector3(1,1,1);
							comp.transform.GetChild(0).localScale = new Vector3(obj.scale.x,obj.scale.y,1);
						}
						/*
						if (comp.GetComponent<PlayerMovement> () != null) {
							PlayerMovement move = comp.GetComponent<PlayerMovement> ();
							move.horizontal = obj.horizontal;
							move.jump = obj.jump;
							move.Player = obj.Player;
							move.runSpeed = obj.runSpeed;
							move.jumpForce = obj.jumpForce;
						}*/

						if (comp.GetComponentInChildren<ButtonTrigger> () != null) {
							ButtonTrigger BT = comp.GetComponentInChildren<ButtonTrigger> ();
							BT.mappingNames = obj.BTmappingNames;
							BT.callName = obj.BTcallName;
							BT.playerSpecific = obj.BTplayerSpecific;
							BT.switched = obj.BTswitched;
							BT.triggerList = obj.BTtriggerList;
							BT.untriggerList = obj.BTuntriggerList;
						}

						//ADD NEW PROPERTIES ABOVE THIS LINE
					}
				}

			} else //skips over gaps in list of levels
				LoadLevel (num + 1);

		} else //loads first level if the level to load is outside the list of levels
			LoadLevel (0);

	}

}


