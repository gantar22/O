using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SaveLevel : MonoBehaviour {

	public Level newLevel;

	//make all level object prefabs a public game object here
	public GameObject exitBoth;
	public GameObject exitEither;
	public GameObject exitP1;
	public GameObject exitP2;
	public GameObject wall;
	public GameObject platform;
	public GameObject button;
	public GameObject spikes;
	public GameObject Player1;
	public GameObject Player2;
	public GameObject spikedPlatform;
	//ADD NEW PREFABS ABOVE THIS LINE

	private LevelObject newObj;

	void Update() {

		newLevel.components = GetComponentList();
	}

	LevelObject[] GetComponentList () {
		
		GameObject[] levelComponents = GameObject.FindGameObjectsWithTag ("LevelObj");

		LevelObject[] things = new LevelObject[levelComponents.Length];

		for (int i = 0; i < things.Length; i++) {

			GameObject compToAdd = levelComponents [i];
			newObj = new LevelObject ();

			newObj.type = GetType (compToAdd);
			newObj.position = compToAdd.transform.position;
			newObj.rotation = compToAdd.transform.eulerAngles.z;
			newObj.scale = compToAdd.transform.localScale;

			things [i] = newObj;

		}

		return things;

	}

	GameObject GetType (GameObject comp) {

		//every level object must have an ObjIdentifier component
		//the variable "Prefab name" in the ObjIdentifier MUST be the same as its prefab
		if (comp.GetComponent<ObjIdentifier> () == null) {
			Debug.Log (comp.name + " has no object identifier and could not be saved to level data.");
			return null;
		}

		//find prefab's name
		string name = comp.GetComponent<ObjIdentifier> ().prefabName;

		if (comp.GetComponent<PlayerMovement>() != null) {
			PlayerMovement move = comp.GetComponent<PlayerMovement>();
			newObj.left = move.left;
			newObj.right = move.right;
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

		//“ADD NEW PROPERTIES ABOVE THIS LINE”

		//match prefab name to prefab
		if (name == "exitBoth")
			return exitBoth;
		if (name == "exitEither")
			return exitEither;
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
		//ADD NEW PREFABS ABOVE THIS LINE

		//object's name didn't match a prefab
		Debug.Log(comp.name + " has an improper name in its object identifier and could not be saved to level data.");
		return null;

	}

}
