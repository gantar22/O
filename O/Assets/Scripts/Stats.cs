using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

	[HideInInspector]
	public int currLevel;
	[HideInInspector]
	public Vector3 P1_respawn, P2_respawn;
	[HideInInspector]
	public bool respawning;
	[HideInInspector]
	public Checkpoint trig_P1_check, trig_P2_check;

	void Start() {
		respawning = false;
		trig_P1_check = null;
		trig_P2_check = null;
	}

}