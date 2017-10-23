using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	private bool pressed;
	private int n;
	ButtonTrigger triggerer;
	private bool animating;
	private float scale;
	private string name;

	// Use this for initialization
	void Start () {
		pressed = false;
		triggerer = GetComponent<ButtonTrigger>();
		animating = false;
		scale = transform.parent.transform.localScale[1];
		name = "";

	}

	void Update () {
		print(pressed);
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
	
	/*void OnCollisionExit2D(Collision2D coll) {
		n--;
		if (pressed && n == 0 && !animating) {
			pressed = false;
			triggerer.untrigger(coll.collider.name);
			animating = true;
			StartCoroutine("startGrow");

		}
	}

	void OnCollisionEnter2D(Collision2D coll) { //Don't start a level pressing the button!!!!
		if (coll.relativeVelocity[1] < -.5f && !pressed && !animating) {
			pressed = true;
			triggerer.trigger(coll.collider.name);
			animating = true;
			StartCoroutine("startShrink");
		}
		n++;
		print("!");
	}*/

	IEnumerator startShrink() {
		/*while(transform.parent.transform.localScale[1] > .3) {
			transform.parent.transform.localScale -= new Vector3(0,Time.deltaTime * 4,0);
			if (transform.parent.transform.localScale[1] < 0)
				transform.parent.transform.localScale += new Vector3(0,Time.deltaTime * 4,0);
			yield return null;
		}*/
		while (transform.localPosition.y > .1 && !animating) {
			transform.position -= new Vector3(0,Time.deltaTime * 3,0);
			yield return null;
		}
		yield return new WaitForSeconds(.1f);
		animating = false;

	}
	IEnumerator startGrow() {
		/*while(transform.parent.transform.localScale[1] > .3) {
			transform.parent.transform.localScale -= new Vector3(0,Time.deltaTime * 4,0);
			if (transform.parent.transform.localScale[1] < 0)
				transform.parent.transform.localScale += new Vector3(0,Time.deltaTime * 4,0);
			yield return null;
		}*/
		while (transform.localPosition.y < .7) {
			transform.position += new Vector3(0,Time.deltaTime * 4,0);
			yield return null;
		}
		yield return new WaitForSeconds(.1f);
		animating = false;

	}
}
