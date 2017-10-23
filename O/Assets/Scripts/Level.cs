using UnityEngine;
using System;

[CreateAssetMenu(menuName="Level")]
[Serializable]
public class Level : ScriptableObject {

	public LevelObject[] components;
	public float cameraSize;

}
