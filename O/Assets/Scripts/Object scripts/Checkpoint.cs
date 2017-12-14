using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNums {P1, P2}
public enum States {idle, triggered, active, invis}

public class Checkpoint : MonoBehaviour {

	public AudioClip trigger;
	public AudioClip activate;	

	[HideInInspector]
	public GameObject GameController;
	[HideInInspector]
	Stats stats;

	[HideInInspector]
	public PlayerNums Player;
	public Sprite[] Sprites;

	[HideInInspector]
	public States Checkpt_State;
	[HideInInspector]
	SpriteRenderer SR;

	void Start() {
		SR = GetComponent<SpriteRenderer> ();
		update_sprite ();

		EventManager.StartListening ("Find_" + Player.ToString() + "_checkpt", Try_to_pair);
		EventManager.StartListening ("Pair_found", Pair_found);
	}

	void OnTriggerEnter2D(Collider2D other) {
		get_references ();
		if (other.name.Contains("Player 1") && Player == PlayerNums.P1 && Checkpt_State == States.idle) {
			change_state(States.triggered);
			if (stats.trig_P1_check != null) {
				stats.trig_P1_check.change_state (States.idle);
			}
			stats.trig_P1_check = this;

			Invoke ("playTrigger", 0.02f);

			EventManager.TriggerEvent ("Find_P2_checkpt");
		} else if (other.name.Contains("Player 2") && Player == PlayerNums.P2 && Checkpt_State == States.idle) {
			change_state(States.triggered);
			if (stats.trig_P2_check != null) {
				stats.trig_P2_check.change_state (States.idle);
			}
			stats.trig_P2_check = this;

			Invoke ("playTrigger", 0.02f);

			EventManager.TriggerEvent ("Find_P1_checkpt");
		}
	}

	void Try_to_pair() {
		if (Checkpt_State == States.triggered) {
			EventManager.TriggerEvent ("Pair_found");
			stats.trig_P1_check = null;
			stats.trig_P2_check = null;

			playSFX (2);
		}
	}

	void Pair_found() {
		get_references ();
		CancelInvoke ("playTrigger");

		if (Checkpt_State == States.active) {
			change_state(States.invis);
		}
			
		if (Checkpt_State == States.triggered) {
			change_state(States.active);
			if (Player == PlayerNums.P1)
				stats.P1_respawn = transform.position;
			else
				stats.P2_respawn = transform.position;
		}
	}

	public void change_state(States new_state) {
		Checkpt_State = new_state;
		update_sprite ();
	}
	
	void get_references() {
		if (GameController == null)
			GameController = GameObject.FindGameObjectWithTag ("GameController");
		if (stats == null)
			stats = GameController.GetComponent<Stats> ();
	}

	void update_sprite() {
		Color sprite_color = SR.color;
		if (Checkpt_State == States.invis)
			sprite_color.a = 0;
		else
			sprite_color.a = 255;
		SR.color = sprite_color;

		int sprite_index = 0;
		if (Player == PlayerNums.P2)
			sprite_index += 3;
		if (Checkpt_State == States.triggered)
			sprite_index += 1;
		if (Checkpt_State == States.active)
			sprite_index += 2;

		if (!(SR.sprite == Sprites [sprite_index])) {
			SR.sprite = Sprites [sprite_index];
		}
	}

	void playTrigger() {
		playSFX (1);
	}

	void playSFX(int clipNum) {
		AudioClip toPlay = new AudioClip();
		if (clipNum == 1) {
			toPlay = trigger;
		} else if (clipNum == 2) {
			toPlay = activate;
		} else {
			Debug.Log ("clipNum" + clipNum + "in Checkpoint.cs not found");
		}

		get_references ();
		if (SettingsManager.gameSettings != null && toPlay != null) {
			float volume = SettingsManager.gameSettings.SFXVolume;
			GameController.GetComponent<AudioSource> ().PlayOneShot (toPlay, volume);
		}
	}
}
