﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour {

	public Level[] levels;
	public int startingLevel;

	void Start() {

		EndLevel ();
		LoadLevel (startingLevel);

	}

	public void EndLevel () {

		//destroy all level objects not to carry over into next level
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("LevelObj");
		foreach (GameObject obj in objs) {
			Destroy (obj);
		}

	}

	public void LoadLevel(int num) {

		if (0 <= num && num < levels.Length) { //make sure level index is valid

			if (levels [num] != null) { //make sure level to load exists
				//update current level in Stats
				GetComponent<Stats>().currLevel = num;

			
				//set the level to be loaded
				Level level = levels [num];
				Camera.main.orthographicSize = levels[num].cameraSize;
				LevelObject[] objs = level.components;

				//put all the objects in the level
				foreach (LevelObject obj in objs) {

					if (obj.type != null) {

						GameObject comp = Instantiate (obj.type, obj.position, Quaternion.identity);
					
						comp.transform.eulerAngles = Vector3.forward * obj.rotation;
						if (obj.scale == Vector2.zero)
							comp.transform.localScale = Vector3.one;
						else
							comp.transform.localScale = new Vector3 (obj.scale.x, obj.scale.y, 1f);

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
						}
						if(comp.GetComponent<PlayerMovement> () != null) {
							PlayerMovement move = comp.GetComponent<PlayerMovement> ();
							move.left = obj.left;
							move.right = obj.right;
							move.jump = obj.jump;
							move.Player = obj.Player;
							move.runSpeed = obj.runSpeed;
							move.jumpForce = obj.jumpForce;
						}

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


