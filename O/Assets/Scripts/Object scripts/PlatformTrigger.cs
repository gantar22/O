using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : TriggersOnCollision {

	public int platformID;

	// Use this for initialization
	void Start () {
		callName = "platform_" + platformID;
	}

}
