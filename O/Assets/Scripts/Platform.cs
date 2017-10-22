using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum MoveOptions {Constant, ConstantAfterTrigger, OneWayTrigger, ThereAndBackTrigger, InputMapping};
public enum MoveStateOptions {idleAtStart, delayedAtStart, movingToEnd, idleAtEnd, delayedAtEnd, movingToStart, mappingOnly};


public class Platform : MonoBehaviour {

	// "Constants"
	private InputManager inputManager;
	[HideInInspector]
	public KeyCode left, right, up, down;

	// Settings
	public int platformID;
	[HideInInspector]
	public MoveOptions MoveSetting;

	public Vector2 translation;
	public float travelTime = 1F;
	public float moveDelay;

	public bool manualMapping;
	public float horizontalMoveSpeed;
	public float verticalMoveSpeed;

	// Variables
	[HideInInspector]
	public Vector2 startPoint, endPoint, autoMove, velo;
	[HideInInspector]
	public float delayTimer, moveTimer, speed;
	[HideInInspector]
	public MoveStateOptions MoveState;


	// Use this for initialization
	void Start () {
		if (travelTime <= 0)
			throw new UnityException ("Set the platform travel time to a positive value");

		velo = Vector2.zero;
		startPoint = new Vector2(transform.position.x, transform.position.y);
		endPoint = startPoint + translation;
		speed = translation.magnitude / travelTime;


		MoveState = MoveStateOptions.idleAtStart;
		if (MoveSetting == MoveOptions.InputMapping) {
			MoveState = MoveStateOptions.mappingOnly;
		} else if (MoveSetting == MoveOptions.Constant) {
			startMoving ();
		}

		EventManager.StartListening ("platform_" + platformID, trigger);
	}

	void OnEnable () {
		inputManager = InputManager.instance;
		if (manualMapping) {
			inputManager.Map (actionString ("left"), left);
			inputManager.Map (actionString ("right"), right);
			inputManager.Map (actionString ("up"), up);
			inputManager.Map (actionString ("down"), down);
		}
	}

	void clearInputManager () {
		inputManager.Remove(actionString("left"),left);
		inputManager.Remove(actionString("right"),right);
		inputManager.Remove(actionString("up"),up);
		inputManager.Remove(actionString("down"),down);
	}

	// Called by a button being pressed or zone being entered
	void trigger () {
		if (MoveSetting == MoveOptions.ConstantAfterTrigger || MoveSetting == MoveOptions.OneWayTrigger || MoveSetting == MoveOptions.ThereAndBackTrigger) {
			if (MoveState == MoveStateOptions.idleAtStart || MoveState == MoveStateOptions.idleAtEnd) {
				startMoving ();
			}
		}
	}

	void startMoving() {
		Vector2 Pos = new Vector2 (transform.position.x, transform.position.y);

		if (MoveState == MoveStateOptions.idleAtStart || MoveState == MoveStateOptions.delayedAtStart) {
			MoveState = MoveStateOptions.movingToEnd;
			moveTimer = travelTime;
			autoMove = (endPoint - Pos).normalized * speed;
		} else if (MoveState == MoveStateOptions.idleAtEnd || MoveState == MoveStateOptions.delayedAtEnd) {
			MoveState = MoveStateOptions.movingToStart;
			moveTimer = travelTime;
			autoMove = (startPoint - Pos).normalized * speed;
		}
	}

	void stopMoving() {
		autoMove = Vector2.zero;
		if (MoveState == MoveStateOptions.movingToEnd) {
			if (MoveSetting == MoveOptions.Constant || MoveSetting == MoveOptions.ConstantAfterTrigger || MoveSetting == MoveOptions.ThereAndBackTrigger) {
				MoveState = MoveStateOptions.delayedAtEnd;
				delayTimer = moveDelay;
			} else {
				MoveState = MoveStateOptions.idleAtEnd;
			}
		} else if (MoveState == MoveStateOptions.movingToStart) {
			if (MoveSetting == MoveOptions.Constant || MoveSetting == MoveOptions.ConstantAfterTrigger) {
				MoveState = MoveStateOptions.delayedAtStart;
				delayTimer = moveDelay;
			} else {
				MoveState = MoveStateOptions.idleAtStart;
			}
		}
	}

	// Update is called once per frame
	void Update () {

		velo = Vector2.zero;

		// Managing automatic movement with movement settings
		if (MoveState == MoveStateOptions.delayedAtEnd || MoveState == MoveStateOptions.delayedAtStart) {
			delayTimer -= Time.deltaTime;
			if (delayTimer <= 0F) {
				startMoving ();
			}
		} else {
			velo = autoMove;
			moveTimer -= Time.deltaTime;
			if (moveTimer <= 0f) {
				stopMoving ();
			}
		}

		// Input mapping
		foreach(KeyCode key in inputManager.Get_Buttons(actionString("left"))) {
			if (Input.GetKey (key))
				velo.x -= horizontalMoveSpeed;
		}
		foreach(KeyCode key in inputManager.Get_Buttons(actionString("right"))) {
			if (Input.GetKey (key))
				velo.x += horizontalMoveSpeed;
		}
		foreach(KeyCode key in inputManager.Get_Buttons(actionString("up"))) {
			if (Input.GetKey (key))
				velo.y += verticalMoveSpeed;
		}
		foreach(KeyCode key in inputManager.Get_Buttons(actionString("down"))) {
			if (Input.GetKey (key))
				velo.y -= verticalMoveSpeed;
		}

		this.GetComponent<Transform>().Translate(velo*Time.deltaTime);
	}

	string actionString(string key) {
		return (key + "_Platform" + platformID.ToString()); 
	}
}


[CustomEditor(typeof(Platform))]
public class PlatformEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// Visual settings
		EditorGUIUtility.labelWidth = 140;

		// Custom editor setup
		var plat = target as Platform;
		GUI.changed = false;

		// Variable Fields
		plat.platformID = EditorGUILayout.IntField("Platform ID", plat.platformID);
		plat.MoveSetting = (MoveOptions) EditorGUILayout.EnumPopup ("Movement Setting", plat.MoveSetting);

		if (plat.MoveSetting == MoveOptions.InputMapping) {
			plat.horizontalMoveSpeed = EditorGUILayout.FloatField ("Horizontal Movespeed", plat.horizontalMoveSpeed);
			plat.verticalMoveSpeed = EditorGUILayout.FloatField ("Vertical Movespeed", plat.verticalMoveSpeed);
			plat.manualMapping = EditorGUILayout.Toggle ("Manual Mapping", plat.manualMapping);
			if (plat.manualMapping) {
				plat.left = (KeyCode)EditorGUILayout.EnumPopup ("Left", plat.left);
				plat.right = (KeyCode)EditorGUILayout.EnumPopup ("Right", plat.right);
				plat.up = (KeyCode)EditorGUILayout.EnumPopup ("Up", plat.up);
				plat.down = (KeyCode)EditorGUILayout.EnumPopup ("Down", plat.down);
			}
		} else {
			plat.translation = EditorGUILayout.Vector2Field("Translation to Endpoint", plat.translation);
			plat.travelTime = EditorGUILayout.FloatField ("Travel Time", plat.travelTime);
			plat.moveDelay = EditorGUILayout.FloatField ("Movement Delay", plat.moveDelay);
		}

		if (GUI.changed) {
			EditorUtility.SetDirty (plat);
		}
	}
}
