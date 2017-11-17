using UnityEngine;
using System;

[CreateAssetMenu(menuName="Level")]
[Serializable]
public class Level : ScriptableObject {

	public LevelObject[] components;
	public float cameraSize = 5; //default so you don't get an error with default levels;
	public bool dynamicCam = false;

}
