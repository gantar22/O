using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

	List<Pair<string,KeyCode>> removed;

	void callP1() {
		if ( inputs.Count > actions.Count) {
			print("fix the mapping assigned to this object");
		}
		if ( remove_inputs.Count > remove_actions.Count) {
			print("fix the mapping assigned to this object");
		}

		for (int i = 0; i < inputs.Count; i++) {
			if (actions[i][actions[i].Length - 1] == '1')
				InputManager.instance.Map(actions[i],inputs[i]);
		}
		removed = new List<Pair<string,KeyCode>>();
		for (int i = 0; i < remove_inputs.Count; i++) {
			if (remove_actions[i][remove_actions[i].Length - 1] == '1') {
				if (remove_inputs[i] == KeyCode.Asterisk) {
					ArrayList toRemove = InputManager.instance.Remove(remove_actions[i]);
					if (toRemove != null) {
						List<Pair<string,KeyCode>> r = new List<Pair<string,KeyCode>>();
						foreach (KeyCode input in toRemove) {
							r.Add(new Pair<string,KeyCode>(remove_actions[i],input));
						}
						removed.AddRange(r);
					}
				}
				if (InputManager.instance.Remove(remove_actions[i],remove_inputs[i])) {
					removed.Add(new Pair<string,KeyCode>(remove_actions[i],remove_inputs[i]));
				}
			}
		}
	}
	void callP2() {
		if ( inputs.Count > actions.Count) {
			print("fix the mapping assigned to this object");
		}
		if ( remove_inputs.Count > remove_actions.Count) {
			print("fix the mapping assigned to this object");
		}


		for (int i = 0; i < inputs.Count; i++) {
			if (actions[i][actions[i].Length - 1] == '2')
				InputManager.instance.Map(actions[i],inputs[i]);
		}
		removed = new List<Pair<string,KeyCode>>();
		for (int i = 0; i < remove_inputs.Count; i++) {
			if (remove_actions[i][remove_actions[i].Length - 1] == '2') {
				if (remove_inputs[i] == KeyCode.Asterisk) {
					ArrayList toRemove = InputManager.instance.Remove(remove_actions[i]);
					if (toRemove != null) {
						List<Pair<string,KeyCode>> r = new List<Pair<string,KeyCode>>();
						foreach (KeyCode input in toRemove) {
							r.Add(new Pair<string,KeyCode>(remove_actions[i],input));
						}
						removed.AddRange(r);
					}
				}
				if (InputManager.instance.Remove(remove_actions[i],remove_inputs[i])) {
					removed.Add(new Pair<string,KeyCode>(remove_actions[i],remove_inputs[i]));
				}
			}
		}
	}
	void callNull() {
		if ( inputs.Count > actions.Count) {
			print("fix the mapping assigned to this object");
		}
		if ( remove_inputs.Count > remove_actions.Count) {
			print("fix the mapping assigned to this object");
		}

		for (int i = 0; i < inputs.Count; i++) {
			InputManager.instance.Map(actions[i],inputs[i]);
		}
		removed = new List<Pair<string,KeyCode>>();
		for (int i = 0; i < remove_inputs.Count; i++) {
			if (remove_inputs[i] == KeyCode.Asterisk) {
				ArrayList toRemove = InputManager.instance.Remove(remove_actions[i]);
				if (toRemove != null) {
					List<Pair<string,KeyCode>> r = new List<Pair<string,KeyCode>>();
					foreach (KeyCode input in toRemove) {
						r.Add(new Pair<string,KeyCode>(remove_actions[i],input));
					}
					removed.AddRange(r);
				}
			}
			if (InputManager.instance.Remove(remove_actions[i],remove_inputs[i])) {
				removed.Add(new Pair<string,KeyCode>(remove_actions[i],remove_inputs[i]));
			}
		}
	}
	void callUndo () { //you don't really need to use this, but if your
	                   // thing is a togglable it might be cool to undo
	                   // your changes
	                   // this also only works for the whole list, but
	                   // that could be added if really want
		if (inputs.Count > actions.Count) {
			print("The input list doesn't match the actions list");
		}
		for (int i = 0; i < remove_inputs.Count; i++) {
			if (removed.Any(pair => pair.fst == remove_actions[i] && pair.snd == remove_inputs[i])) {
				InputManager.instance.Map(remove_actions[i],remove_inputs[i]);
			}
		}
		removed = new List<Pair<string,KeyCode>>();
		for (int i = 0; i < inputs.Count; i++) {
			InputManager.instance.Remove(actions[i],inputs[i]);
		}
	}
	void callUndoP1 () { //you don't really need to use this, but if your
	                   // thing is a togglable it might be cool to undo
	                   // your changes
	                   // this also only works for the whole list, but
	                   // that could be added if really want
		if (inputs.Count != actions.Count || remove_inputs.Count != remove_actions.Count) {
			print("The input list doesn't match the actions list");
		}
		for (int i = 0; i < remove_inputs.Count; i++) {
			if (removed.Any(pair => pair.fst == remove_actions[i] && pair.snd == remove_inputs[i])) {
				if (remove_actions[i][remove_actions[i].Length - 1] == '1')
					InputManager.instance.Map(remove_actions[i],remove_inputs[i]);
			}
		}
		removed = new List<Pair<string,KeyCode>>();
		for (int i = 0; i < inputs.Count; i++) {
			if (actions[i][actions[i].Length - 1] == '1')
				InputManager.instance.Remove(actions[i],inputs[i]);
		}
	}
	void callUndoP2 () { //you don't really need to use this, but if your
	                   // thing is a togglable it might be cool to undo
	                   // your changes
	                   // this also only works for the whole list, but
	                   // that could be added if really want
		if (inputs.Count != actions.Count || remove_inputs.Count != remove_actions.Count) {
			print("The input list doesn't match the actions list");
		}
		for (int i = 0; i < remove_inputs.Count; i++) {
			if (removed.Any(pair => pair.fst == remove_actions[i] && pair.snd == remove_inputs[i])) {
				if (remove_actions[i][remove_actions[i].Length - 1] == '2')
					InputManager.instance.Map(remove_actions[i],remove_inputs[i]);
			}
		}
		removed = new List<Pair<string,KeyCode>>();
		for (int i = 0; i < inputs.Count; i++) {
			if (actions[i][actions[i].Length - 1] == '2')
				InputManager.instance.Remove(actions[i],inputs[i]);
		}
	}
	void Start() {
		EventManager.StartListening(callName,callNull);
		EventManager.StartListening(callName + "_1",callP1);
		EventManager.StartListening(callName + "_2",callP2);
		EventManager.StartListening(callName + "_undo",callUndo);
		EventManager.StartListening(callName + "_undo_1",callUndoP1);
		EventManager.StartListening(callName + "_undo_2",callUndoP2);


	}
}
