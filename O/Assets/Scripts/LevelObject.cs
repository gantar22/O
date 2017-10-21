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

}
