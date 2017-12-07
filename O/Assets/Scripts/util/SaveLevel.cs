using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif

[ExecuteInEditMode]
public class SaveLevel : MonoBehaviour {

	public Level newLevel;
	[HideInInspector]
	Stats stats;

	//make all level object prefabs a public game object here
	public GameObject exitBoth;
	public GameObject exitP1;
	public GameObject exitP2;
	public GameObject wall;
	public GameObject platform;
	public GameObject button;
	public GameObject spikes;
	public GameObject Player1;
	public GameObject Player2;
	public GameObject spikedPlatform;
	public GameObject exitPlatform;
	public GameObject spikedWall;
	public GameObject spikedBall;
	public GameObject CheckpointP1;
	public GameObject CheckpointP2;


	private LevelObject newObj;

	void Update() {
		newLevel.cameraSize = Camera.main.orthographicSize;
		newLevel.dynamicCam = Camera.main.GetComponent<DynamicCamera> ().enabled;
		newLevel.components = GetComponentList();
		#if UNITY_EDITOR
			EditorUtility.SetDirty (newLevel);
			AssetDatabase.SaveAssets ();
		#endif
		Debug.Log("Level Saved: " + newLevel.name);
	}

	LevelObject[] GetComponentList () {


		GameObject[] levelComponents = GameObject.FindGameObjectsWithTag ("LevelObj");

		// First get number of non-null LevelObj's
		int numOfObjs = 0;
		for (int i = 0; i < levelComponents.Length; i++) {
			if (GetType (levelComponents [i], true) != null) {
				numOfObjs++;
			}
		}

		LevelObject[] things = new LevelObject[numOfObjs];

		int objsIndex = 0;
		for (int i = 0; i < levelComponents.Length; i++) {

			newObj = new LevelObject ();
			GameObject type = GetType (levelComponents [i], false);
			if (type != null) {
				
				GameObject compToAdd = levelComponents [i];
				newObj.type = type;
				newObj.position = compToAdd.transform.position;
				newObj.rotation = compToAdd.transform.eulerAngles.z;
				newObj.scale = compToAdd.transform.localScale;

				things [objsIndex] = newObj;
				objsIndex++;
			}
		}

		return things;

	}

	GameObject GetType (GameObject comp, bool justReturnType) {

		//every level object must have an ObjIdentifier component
		//the variable "Prefab name" in the ObjIdentifier MUST be the same as its prefab
		if (comp.GetComponent<ObjIdentifier> () == null) {
			string prefabName = comp.transform.parent.gameObject.GetComponent<ObjIdentifier> ().prefabName;

			if (comp.transform.parent.gameObject.GetComponent<ObjIdentifier> () != null)
				if (prefabName == "button" || prefabName == "spikedPlatform" || prefabName == "spikedBall" || prefabName == "spikedWall" || prefabName == "exitPlatform") //if you make a parent object add it here
					return null;

			Debug.Log (comp.name + " has no object identifier and could not be saved to level data." + comp.transform.parent.gameObject.GetComponent<ObjIdentifier>().prefabName);
			return null;
		}

		//find prefab's name
		string name = comp.GetComponent<ObjIdentifier> ().prefabName;

		if (!justReturnType) {

			if (stats == null)
				stats = GetComponent<Stats> ();
			
			if (comp.name.Contains ("Player 1")) {
				stats.P1_respawn = comp.transform.position;
			}
			if (comp.name.Contains ("Player 2")) {
				stats.P2_respawn = comp.transform.position;
			}

			if (comp.GetComponent<PlayerMovement>() != null) {
				PlayerMovement move = comp.GetComponent<PlayerMovement>();
				newObj.horizontal = move.horizontal;
				newObj.jump = move.jump;
				newObj.Player = move.Player;
				newObj.runSpeed = move.runSpeed;
				newObj.jumpForce = move.jumpForce;
			}
			//if the prefab is any kind of platform, get its properties
			if (comp.GetComponent<Platform> () != null) {
				Platform plat = comp.GetComponent<Platform> ();
				newObj.platformID = plat.platformID;
				newObj.platMoveSetting = plat.MoveSetting;
				newObj.platTranslation = plat.translation;
				newObj.platTravelTime = plat.travelTime;
				newObj.platMoveDelay = plat.moveDelay;
				newObj.platManualMapping = plat.manualMapping;
				newObj.platHorizontalMoveSpeed = plat.horizontalMoveSpeed;
				newObj.platVerticalMoveSpeed = plat.verticalMoveSpeed;
				newObj.platReturnOnUntrigger = plat.returnOnUntrigger;
			}

			if (comp.GetComponentInChildren<ButtonTrigger> () != null) {
				ButtonTrigger BT = comp.GetComponentInChildren<ButtonTrigger> ();
				newObj.BTmappingNames = BT.mappingNames;
				newObj.BTcallName = BT.callName;
				newObj.BTplayerSpecific = BT.playerSpecific;
				newObj.BTswitched = BT.switched;
				newObj.BTtriggerList = BT.triggerList;
				newObj.BTuntriggerList = BT.untriggerList;
			}
			

			//ADD NEW PROPERTIES ABOVE THIS LINE
		}

		//match prefab name to prefab
		if (name == "exitBoth")
			return exitBoth;
		if (name == "exitP1")
			return exitP1;
		if (name == "exitP2")
			return exitP2;
		if (name == "wall")
			return wall;
		if (name == "platform")
			return platform;
		if (name == "button")
			return button;
		if (name == "spikes")
			return spikes;
		if(name == "Player1")
			return Player1;
		if(name == "Player2")
			return Player2;
		if (name == "spikedPlatform")
			return spikedPlatform;
		if (name == "exitPlatform")
			return exitPlatform;
		if (name == "spikedWall")
			return spikedWall;
		if (name == "spikedBall")
			return spikedBall;
		if (name == "CheckpointP1")
			return CheckpointP1;
		if (name == "CheckpointP2")
			return CheckpointP2;
		//ADD NEW PREFABS ABOVE THIS LINE

		//object's name didn't match a prefab
		Debug.Log(comp.name + " has an improper name in its object identifier and could not be saved to level data.");
		return null;

	}

}
