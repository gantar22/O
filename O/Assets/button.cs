using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour {

	private bool pressed;
	private int n;

	// Use this for initialization
	void Start () {
		pressed = false;
		
	}
	
	void OnCollisionExit2D(Collision2D coll) {
		if (pressed) {

		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.relativeVelocity[1] < -.5f && !pressed) {
			pressed = false;
			ButtonTrigger triggerer = GetComponent<ButtonTrigger>();
			triggerer.trigger();
	}
}
