﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicmanager : MonoBehaviour {

	private GameObject p1;
	private GameObject p2;
	private bool a1; //p1 wants music!
	private bool a2;
	public float duration;
	public AudioSource music;
	public AudioSource music1;
	public AudioSource music2;
	public float maxPlayerVolume;



	void Start(){
		music.Play();
		music1.Play();
		music1.volume = 0;
		music2.Play();
		music2.volume = 0;
	}

	void Update () {
		GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
	     for(int i=0;i<gos.Length;i++)
	     {
	         if(gos[i].name.Contains("Player 1"))
	         {
	             p1 = gos[i];
	         }
	         else if(gos[i].name.Contains("Player 2"))
	         {
	         	p2 = gos[i];
	         }
	     }

 
		if(p1.GetComponent<PlayerMovement>().OnAir())
		{
			CancelInvoke("end1");
			Invoke("end1",duration);
			a1 = true;
			music.volume = Mathf.Clamp(music.volume - 2 * Time.deltaTime,0,1);
		}
		if(p2.GetComponent<PlayerMovement>().OnAir())
		{
			CancelInvoke("end2");
			Invoke("end2",duration);
			a2 = true;
			music.volume = Mathf.Clamp(music.volume - 2 * Time.deltaTime,0,1);
		}

		if(p1.GetComponent<Rigidbody2D>().velocity.x != 0f) music1.volume = Mathf.Clamp(music1.volume + Time.deltaTime * Mathf.Abs(p1.GetComponent<Rigidbody2D>().velocity.x) / (8 * 2),0,maxPlayerVolume);
		else                   music1.volume = Mathf.Clamp(music1.volume - Time.deltaTime * .5f,                  0,maxPlayerVolume);


		print(p1.GetComponent<Rigidbody2D>().velocity.x);
		if(p2.GetComponent<Rigidbody2D>().velocity.x != 0f) music2.volume = Mathf.Clamp(music2.volume + Time.deltaTime * Mathf.Abs(p2.GetComponent<Rigidbody2D>().velocity.x) / (8 * 2),0,maxPlayerVolume);
		else                   music2.volume = Mathf.Clamp(music2.volume - Time.deltaTime * .5f,                  0,maxPlayerVolume);


		if(a1 && a2) 
		{
			music.volume = Mathf.Clamp(music.volume - duration * Time.deltaTime / 10,0,1); // in a tenth of the duration in seconds this moves from full to off
			music1.volume = 1;
			music2.volume = 1;
		}
		else 
		{
			music.volume = Mathf.Clamp(music.volume + duration * Time.deltaTime / 7,0,1);
		}
		print(new Vector3(music.volume,music1.volume,music2.volume));
	}


	void end1()
	{
		a1 = false;
	}

	void end2(){
		a2 = false;
	}
}
