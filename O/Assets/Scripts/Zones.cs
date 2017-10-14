using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zones : MonoBehaviour {

	private PlayerMovement playerEnteredInput;
	private PlayerMovement playerOtherInput;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void onTriggerEnter(Collider other)
	{
		if (other.tag == "Player1")
		{

		}
		if (other.tag == "Player2")
		{

		}
	}
}
