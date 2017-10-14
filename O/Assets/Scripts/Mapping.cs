using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapping : MonoBehaviour {

	//This is the string to call in event manager to trigger this mapping
	[SerializeField]
	public string callName;

	//When this gets triggered, these inputs now trigger these actions
	[SerializeField]
	public List<KeyCode> inputs;
	[SerializeField]
	public List<string>  actions;

	//this inputs will no longer trigger these actions if they did before
	[SerializeField]
	public List<KeyCode> remove_inputs;
	[SerializeField]
	public List<string> remove_actions;

	void call() {
		foreach (KeyCode input in inputs) {
			foreach (string action in actions) {
				InputManager.instance.Map(action,input);
			}
		}
		foreach (KeyCode input in remove_inputs) {
			foreach (string action in remove_actions) {
				InputManager.instance.Remove(action,input);
			}
		}
	}
	
	void Start() {
		EventManager.StartListening(callName,call);
	}
}
