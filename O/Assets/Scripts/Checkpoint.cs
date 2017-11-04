using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNums {P1, P2}
public enum States {idle, triggered, active, invis}

public class Checkpoint : MonoBehaviour {

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
		if (other.name.Contains("Player 1") && Player == PlayerNums.P1 && Checkpt_State == States.idle) {
			Checkpt_State = States.triggered;
			EventManager.TriggerEvent ("Find_P2_checkpt");
		}
	}

	void Try_to_pair() {
		if (Checkpt_State == States.triggered) {
			EventManager.TriggerEvent ("Pair_found");
		}
	}

	void Pair_found() {
		if (Checkpt_State == States.active)
			// Fading/dissapearing animation?
			Checkpt_State = States.invis;
		if (Checkpt_State == States.triggered)
			Checkpt_State = States.active;

		update_sprite ();
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
		SR.sprite = Sprites [sprite_index];
	}
}
