using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	private bool pressed;
	private int n;
	ButtonTrigger triggerer;
	private bool animating;

	// Use this for initialization
	void Start () {
		pressed = false;
		triggerer = GetComponent<ButtonTrigger>();
		animating = false;
	}

	void Update () {
		string name = "";
		Transform playerOne = null;
		Transform playerTwo = null;
		foreach (Transform child in transform) {
			if (child.gameObject.name.Contains("Player 1"))
				playerOne = child;
			if (child.gameObject.name.Contains("Player 2"))
				playerTwo = child;
		}
		if (playerOne != null && !pressed) {
			pressed = true;
			StartCoroutine("startShrink");
			triggerer.trigger("Player 1");
			name = "Player 1";
		}
		if (playerTwo != null && !pressed) {
			pressed = true;
			StartCoroutine("startShrink");
			triggerer.trigger("Player 2");
			name = "Player 2";
		}
		if (pressed) {
			if (transform.childCount == 0) {
				triggerer.untrigger(name);
				StartCoroutine("startGrow");
				name = "";
				pressed = false;
				animating = true;
			}
		}
	}

	IEnumerator startShrink() {
		while (transform.localPosition.y > .1 && !animating) {
			transform.position -= new Vector3(0,Time.deltaTime * 3,0);
			yield return null;
		}
		yield return new WaitForSeconds(.1f);
		animating = false;

	}
	IEnumerator startGrow() {
		while (transform.localPosition.y < .7) {
			transform.position += new Vector3(0,Time.deltaTime * 4,0);
			yield return null;
		}
		yield return new WaitForSeconds(.1f);
		animating = false;

	}
}
