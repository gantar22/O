using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	private bool pressed;
	private int n;
	ButtonTrigger triggerer;

	// Use this for initialization
	void Start () {
		pressed = false;
		triggerer = GetComponent<ButtonTrigger>();
		
	}
	
	void OnCollisionExit2D(Collision2D coll) {
		n--;
		print(n);
		if (pressed && n == 0) {
			pressed = false;
			triggerer.untrigger(coll.collider.name);

		}
	}

	void OnCollisionEnter2D(Collision2D coll) { //Don't start a level pressing the button!!!!
		if (coll.relativeVelocity[1] < -.5f && !pressed) {
			pressed = true;
			triggerer.trigger(coll.collider.name);
		}
		n++;
	}
}
