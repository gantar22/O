using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggersOnCollision : MonoBehaviour {

	public List<string> mappingNames;
	public string callName;
	public bool playerSpecific;
	public bool switched;

	void OnTriggerEnter2D(Collider2D coll) {
		if (playerSpecific) {
			if (coll.name == "Player1"){
				foreach (string name in mappingNames)
					EventManager.TriggerEvent(name + "_2");
			}
			if (coll.name == "Player2"){
				foreach (string name in mappingNames)
					EventManager.TriggerEvent(name + "_1");
			} 
			EventManager.TriggerEvent(callName);
		} else {
			EventManager.TriggerEvent(callName);
			foreach (string name in mappingNames)
				EventManager.TriggerEvent(name);
		}
	}

}
