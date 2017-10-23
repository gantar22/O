using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LoadLevel : MonoBehaviour {

	public bool reset;
	public int level;
	
	void Update () {
		if (reset)
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<Levels> ().EndLevel ();
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<Levels> ().LoadLevel (level);

	}
}
