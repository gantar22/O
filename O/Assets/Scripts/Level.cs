using UnityEngine;
using System;

[CreateAssetMenu(menuName="Level")]
[Serializable]
public class Level : ScriptableObject {

	public LevelObject[] components;
	public Vector3 exitLoc;
	public Vector3 player1loc;
	public Vector3 player2loc;
	public KeyCode p1left;
	public KeyCode p1right;
	public KeyCode p1jump;
	public KeyCode p2left;
	public KeyCode p2right;
	public KeyCode p2jump;

}
