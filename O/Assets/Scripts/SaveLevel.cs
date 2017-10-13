using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SaveLevel : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;

	public Level newLevel;

	//make all level object prefabs a public game object here
	public GameObject exitBoth;
	public GameObject exitEither;
	public GameObject exitP1;
	public GameObject exitP2;
	public GameObject wall;
	public GameObject platform;
	//ADD NEW PREFABS ABOVE THIS LINE

	void Update() {

		newLevel.components = GetComponentList();
		newLevel.player1loc = player1.transform.position;
		newLevel.player2loc = player2.transform.position;
		newLevel.p1left = player1.GetComponent<PlayerMovement>().left;
		newLevel.p1right = player1.GetComponent<PlayerMovement>().right;
		newLevel.p1jump = player1.GetComponent<PlayerMovement>().jump;
		newLevel.p2left = player2.GetComponent<PlayerMovement>().left;
		newLevel.p2right = player2.GetComponent<PlayerMovement>().right;
		newLevel.p2jump = player2.GetComponent<PlayerMovement>().jump;

	}

	LevelObject[] GetComponentList () {
		
		GameObject[] levelComponents = GameObject.FindGameObjectsWithTag ("LevelObj");

		LevelObject[] things = new LevelObject[levelComponents.Length];

		for (int i = 0; i < things.Length; i++) {

			GameObject compToAdd = levelComponents [i];
			LevelObject newObj = new LevelObject ();

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
		//ADD NEW PREFABS ABOVE THIS LINE

		//object's name didn't match a prefab
		Debug.Log(comp.name + " has an improper name in its object identifier and could not be saved to level data.");
		return null;

	}

}
