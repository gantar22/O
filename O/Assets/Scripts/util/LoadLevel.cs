using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LoadLevel : MonoBehaviour {

	public bool reset;
	public int level;
	
	void OnEnable () {
		if (reset)
			gameObject.GetComponent<Levels> ().EndLevel ();
		gameObject.GetComponent<Levels> ().LoadLevel (level);
	}
	public void load(){
		gameObject.GetComponent<Levels> ().EndLevel ();
		gameObject.GetComponent<Levels> ().LoadLevel (level);
		Invoke("doubleloadbestload",.00001f);
	}

	void doubleloadbestload(){ //Don't ask
		load();
		CancelInvoke();
	}
}
