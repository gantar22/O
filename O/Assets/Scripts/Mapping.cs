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


	void callP1() {
		foreach (KeyCode input in inputs) {
			foreach (string action in actions) {
				if (action[action.Length - 1] == '1')
					InputManager.instance.Map(action,input);
			}
		}
		foreach (KeyCode input in remove_inputs) {
			foreach (string action in remove_actions) {
				if (action[action.Length - 1] == '1')
					InputManager.instance.Remove(action,input);
			}
		}
	}
	void callP2() {
		foreach (KeyCode input in inputs) {
			foreach (string action in actions) {
				if (action[action.Length - 1] == '2')
					InputManager.instance.Map(action,input);
			}
		}
		foreach (KeyCode input in remove_inputs) {
			foreach (string action in remove_actions) {
				if (action[action.Length - 1] == '2')
					InputManager.instance.Map(action,input);
			}
		}
	}
	void callNull() {
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
		EventManager.StartListening(callName,callNull);
		EventManager.StartListening(callName + "_1",callP1);
		EventManager.StartListening(callName + "_2",callP2);
	}
}
