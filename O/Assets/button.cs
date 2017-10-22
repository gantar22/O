using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	private bool pressed;
	private int n;
	ButtonTrigger triggerer;
	private bool animating;
	private float scale;

	// Use this for initialization
	void Start () {
		pressed = false;
		triggerer = GetComponent<ButtonTrigger>();
		animating = false;
		scale = transform.parent.transform.localScale[1];
	}
	
	void OnCollisionExit2D(Collision2D coll) {
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
	}
	IEnumerator startShrink() {
		while(transform.parent.transform.localScale[1] > .3) {
			transform.parent.transform.localScale -= new Vector3(0,Time.deltaTime * 4,0);
			if (transform.parent.transform.localScale[1] < 0)
				transform.parent.transform.localScale += new Vector3(0,Time.deltaTime * 4,0);
			yield return null;
		}
		yield return new WaitForSeconds(.1f);
		animating = false;

	}
	IEnumerator startGrow() {
		while(transform.parent.transform.localScale[1] < scale) {
			transform.parent.transform.localScale += new Vector3(0,Time.deltaTime * 5,0);
			yield return null;
		}
		yield return new WaitForSeconds(.1f);
		animating = false;

	}
}
