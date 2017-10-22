using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour {

	public List<string> mappingNames;
	public string callName;
	public bool playerSpecific;
	public bool switched;
	public List<string> triggerList;
	public List<string> untriggerList;
	string one;
	string two;

	public void trigger(string collName) {
		if (switched) {
			string one = "_2";
			string two = "_1";
		} else {
			string one = "_1";
			string two = "_2";
		}
		if (playerSpecific) {
			if (collName == "Player1"){
				foreach (string name in mappingNames)
					EventManager.TriggerEvent(name + one);
			}
			if (collName == "Player2"){
				foreach (string name in mappingNames)
					EventManager.TriggerEvent(name + two);
			} 
			EventManager.TriggerEvent(callName);
		} else {
			EventManager.TriggerEvent(callName);
			foreach (string name in mappingNames)
				EventManager.TriggerEvent(name);
		}
		foreach (string name in triggerList)
			EventManager.TriggerEvent(name);
	}

	public void untrigger(string collName) {
		if (switched) {
			string one = "_2";
			string two = "_1";
		} else {
			string one = "_1";
			string two = "_2";
		}
		if (playerSpecific) {
			if (collName == "Player1"){
				foreach (string name in mappingNames)
					EventManager.TriggerEvent(name + "_undo" + one);
			}
			if (collName == "Player2"){
				foreach (string name in mappingNames)
					EventManager.TriggerEvent(name + "_undo" + two);
			} 
			EventManager.TriggerEvent(callName);
		} else {
			EventManager.TriggerEvent(callName);
			foreach (string name in mappingNames)
				EventManager.TriggerEvent(name + "undo");
		}
		foreach (string name in untriggerList)
			EventManager.TriggerEvent(name);
	}

}
