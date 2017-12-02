using UnityEngine;
using System;

[CreateAssetMenu(menuName="Level")]
[Serializable]
public class Level : ScriptableObject {

	[TextArea(7,20)]
	public string designNotes;

	public float cameraSize = 5; //default so you don't get an error with default levels;
	public bool dynamicCam = false;

	public LevelObject[] components;

}
