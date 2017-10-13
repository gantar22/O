using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	public int platformID;

	//[HideInInspector]
	public KeyCode left, right, up, down;

	public float horizontalMoveSpeed;
	public float verticalMoveSpeed;
	[HideInInspector]
	public Vector2 velo;
	private InputManager inputManager;

	// Use this for initialization
	void Start () {
		velo = Vector2.zero;
	}

	void OnEnable () {
		inputManager = InputManager.instance;
		inputManager.Map(actionString("left"),left);
		inputManager.Map(actionString("right"),right);
		inputManager.Map(actionString("up"),up);
		inputManager.Map(actionString("down"),down);
	}

	void LetCSharpCollectItsOwnGarbage () {
		inputManager.Remove(actionString("left"),left);
		inputManager.Remove(actionString("right"),right);
		inputManager.Remove(actionString("up"),up);
		inputManager.Remove(actionString("down"),down);
	}


	// Update is called once per frame
	void Update () {

		velo = Vector2.zero;

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
				velo.y -= horizontalMoveSpeed;
		}

		this.GetComponent<Transform>().Translate(velo*Time.deltaTime);
	}

	string actionString(string key) {
		return (key + "_Platform" + platformID.ToString()); 
	}
}
