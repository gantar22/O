using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelObject {

	public GameObject type;
	public Vector3 position;
	public float rotation;
	public Vector2 scale = Vector2.one;

	//platform specific parameters
	public int platformID;
	public MoveOptions platMoveSetting;
	public Vector2 platTranslation;
	public float platTravelTime;
	public float platMoveDelay;
	public bool platManualMapping;
	public float platHorizontalMoveSpeed;
	public float platVerticalMoveSpeed;
	public bool platReturnOnUntrigger;
	public KeyCode right;
	public KeyCode left;
	public KeyCode jump;
	public int Player;
	public float runSpeed;
	public float jumpForce;
	

	//ButtonTrigger specific parameters
	public List<string> BTmappingNames;
	public string BTcallName;
	public bool BTplayerSpecific;
	public bool BTswitched;
	public List<string> BTtriggerList;
	public List<string> BTuntriggerList;
}
