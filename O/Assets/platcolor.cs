using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platcolor : MonoBehaviour {

	SpriteRenderer sp;
	bool up;

	// Use this for initialization
	void Start () {
		sp = gameObject.GetComponent<SpriteRenderer>();
		up = false;

	}
	
	// Update is called once per frame
	void Update () {
		float tick = Time.deltaTime * .5f;
		if(sp.color.r + Time.deltaTime > 1)
			up = true;
		if(sp.color.r - Time.deltaTime < 0)
			up = false;
		if(up){
			sp.color = new Color(sp.color.r - tick,sp.color.g,sp.color.b + tick);
		} else{
			sp.color = new Color(sp.color.r + tick,sp.color.g,sp.color.b - tick);
		}

		
	}
}
