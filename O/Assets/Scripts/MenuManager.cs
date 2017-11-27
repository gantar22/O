using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {

	public EventSystem EventSystem;
	public GameObject defaultSelect;

	private bool buttonSelected;

	public void Update() {
		if (EventSystem == null)
			Debug.LogError("Missing event system in MenuManager script");

		if (Input.GetAxisRaw ("Vertical") != 0 && !buttonSelected) {
			EventSystem.SetSelectedGameObject (defaultSelect);
			buttonSelected = true;
		}
	}

	private void OnDisable() {
		buttonSelected = false;
	}

	public void Start() {

	}

	public void Quit() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
