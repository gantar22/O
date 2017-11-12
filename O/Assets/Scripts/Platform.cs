using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif

public enum MoveOptions {Constant, ConstantAfterTrigger, OneWayTrigger, ThereAndBackTrigger, ButtonMoves, ButtonStops, InputMapping};
public enum MoveStateOptions {idleAtStart, delayedAtStart, movingToEnd, idleAtEnd, delayedAtEnd, movingToStart, returningToStart};


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

	public bool manualMapping, returnOnUntrigger;
	public float horizontalMoveSpeed;
	public float verticalMoveSpeed;

	// Variables
	[HideInInspector]
	public Vector2 startPoint, endPoint, velo;
	[HideInInspector]
	public float delayTimer, speed;
	[HideInInspector]
	public MoveStateOptions MoveState;
	[HideInInspector]
	public bool inverted;


	// Use this for initialization
	void Start () {
		if (travelTime <= 0)
			throw new UnityException ("Set the platform travel time to a positive value");

		velo = Vector2.zero;
		startPoint = new Vector2(transform.position.x, transform.position.y);
		endPoint = startPoint + translation;
		speed = translation.magnitude / travelTime;
		inverted = false;


		MoveState = MoveStateOptions.idleAtStart;
		if (MoveSetting == MoveOptions.Constant || MoveSetting == MoveOptions.ButtonStops) {
			startMoving ();
		}

		EventManager.StartListening ("platform_" + platformID, trigger);
		EventManager.StartListening ("platform_untrigger_" + platformID, untrigger);
		EventManager.StartListening ("platform_invert_" + platformID, invertButtonSetting);
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
		} else if (MoveSetting == MoveOptions.ButtonMoves) {
			startMoving ();
		} else if (MoveSetting == MoveOptions.ButtonStops) {
			stopMoving_ButtonControlled ();
		}
	}

	// Called by a button being unpressed or zone being exited
	void untrigger () {
		if (MoveSetting == MoveOptions.ButtonMoves) {
			stopMoving_ButtonControlled ();
		} else if (MoveSetting == MoveOptions.ButtonStops) {
			startMoving ();
		}
	}

	void invertButtonSetting() {
		if (inverted)
			return;

		if (MoveSetting == MoveOptions.ButtonStops) {
			MoveSetting = MoveOptions.ButtonMoves;
		} else if (MoveSetting == MoveOptions.ButtonMoves) {
			MoveSetting = MoveOptions.ButtonStops;
		}

		if (MoveState == MoveStateOptions.idleAtEnd || MoveState == MoveStateOptions.idleAtStart) {
			startMoving ();
		} else {
			stopMoving_ButtonControlled ();
		}

		inverted = true;
	}

	void startMoving() {
		if (MoveState == MoveStateOptions.idleAtStart || MoveState == MoveStateOptions.delayedAtStart || MoveState == MoveStateOptions.returningToStart) {
			MoveState = MoveStateOptions.movingToEnd;
		} else if (MoveState == MoveStateOptions.idleAtEnd || MoveState == MoveStateOptions.delayedAtEnd) {
			MoveState = MoveStateOptions.movingToStart;
		}
	}

	void stopMoving() {
		if (MoveState == MoveStateOptions.returningToStart) {
			MoveState = MoveStateOptions.idleAtStart;
		} else if (MoveState == MoveStateOptions.movingToEnd) {
			if (MoveSetting == MoveOptions.Constant || MoveSetting == MoveOptions.ConstantAfterTrigger || MoveSetting == MoveOptions.ThereAndBackTrigger
				|| MoveSetting == MoveOptions.ButtonMoves || MoveSetting == MoveOptions.ButtonStops) {
				MoveState = MoveStateOptions.delayedAtEnd;
				delayTimer = moveDelay;
			} else {
				MoveState = MoveStateOptions.idleAtEnd;
			}
		} else if (MoveState == MoveStateOptions.movingToStart) {
			if (MoveSetting == MoveOptions.Constant || MoveSetting == MoveOptions.ConstantAfterTrigger || MoveSetting == MoveOptions.ButtonMoves || MoveSetting == MoveOptions.ButtonStops) {
				MoveState = MoveStateOptions.delayedAtStart;
				delayTimer = moveDelay;
			} else {
				MoveState = MoveStateOptions.idleAtStart;
			}
		}
	}

	void stopMoving_ButtonControlled() {
		if (returnOnUntrigger) {
			MoveState = MoveStateOptions.returningToStart;
		} else {
			if (MoveState == MoveStateOptions.movingToEnd || MoveState == MoveStateOptions.delayedAtStart) {
				MoveState = MoveStateOptions.idleAtStart;
			} else if (MoveState == MoveStateOptions.movingToStart || MoveState == MoveStateOptions.delayedAtEnd) {
				MoveState = MoveStateOptions.idleAtEnd;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		velo = Vector2.zero;
		Vector2 Pos = new Vector2 (transform.position.x, transform.position.y);


		// Managing automatic movement with movement settings, V2
		if (MoveState == MoveStateOptions.delayedAtEnd || MoveState == MoveStateOptions.delayedAtStart) {
			delayTimer -= Time.deltaTime;
			if (delayTimer <= 0f) {
				startMoving ();
			}
		} else if (MoveState == MoveStateOptions.movingToEnd) {
			velo = (endPoint - Pos).normalized * speed;
			if (hasReached(endPoint)) {
				stopMoving ();
			}
		} else if (MoveState == MoveStateOptions.movingToStart || MoveState == MoveStateOptions.returningToStart) {
			velo = (startPoint - Pos).normalized * speed;
			if (hasReached(startPoint)) {
				stopMoving ();
			}
		}


		// Input mapping
		if (MoveSetting == MoveOptions.InputMapping) {
			foreach (KeyCode key in inputManager.Get_Buttons(actionString("left"))) {
				if (Input.GetKey (key))
					velo.x -= horizontalMoveSpeed;
			}
			foreach (KeyCode key in inputManager.Get_Buttons(actionString("right"))) {
				if (Input.GetKey (key))
					velo.x += horizontalMoveSpeed;
			}
			foreach (KeyCode key in inputManager.Get_Buttons(actionString("up"))) {
				if (Input.GetKey (key))
					velo.y += verticalMoveSpeed;
			}
			foreach (KeyCode key in inputManager.Get_Buttons(actionString("down"))) {
				if (Input.GetKey (key))
					velo.y -= verticalMoveSpeed;
			}
		}

		this.GetComponent<Transform> ().position += (Vector3) (velo * Time.deltaTime);
	}

	// Helper functions
	string actionString(string key) {
		return (key + "_Platform" + platformID.ToString()); 
	}

	bool hasReached (Vector2 destination) {
		Vector2 Pos = new Vector2 (transform.position.x, transform.position.y);
		return (Pos - destination).magnitude < 0.1f; // Change this float to adjust how close the platform has to get to start/end
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Platform))]
public class PlatformEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// Visual settings
		EditorGUIUtility.labelWidth = 185;

		// Custom editor setup
		var plat = target as Platform;
		GUI.changed = false;

		// Variable Fields
		plat.platformID = EditorGUILayout.IntField("Platform ID", plat.platformID);
		plat.MoveSetting = (MoveOptions) EditorGUILayout.EnumPopup ("Movement Setting", plat.MoveSetting);

		if (plat.MoveSetting == MoveOptions.ButtonMoves || plat.MoveSetting == MoveOptions.ButtonStops) {
			plat.returnOnUntrigger = EditorGUILayout.Toggle ("Return on Untrigger (vs pause)", plat.returnOnUntrigger);
		}

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
#endif
