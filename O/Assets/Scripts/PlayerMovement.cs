﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public KeyCode left;
	public KeyCode right;
	public KeyCode jump;

	private Rigidbody2D rb;

	public float runSpeed;
	public float jumpForce;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();

	}

	// Update is called once per frame
	void Update () {

		//horizontal movement:
		float velo = 0f;

		if (Input.GetKey (left))
			velo -= runSpeed;
		if (Input.GetKey (right))
			velo += runSpeed;

		rb.velocity = new Vector2 (velo, rb.velocity.y);

		//jumping:
		if (Input.GetKeyDown (jump) && OnGround())
			rb.AddForce (Vector2.up * jumpForce);

	}

	bool OnGround () {

		//find width and height of character
		BoxCollider2D coll = GetComponent<BoxCollider2D> ();
		Vector2 pos = new Vector2(transform.position.x + coll.offset.x * transform.localScale.x, 
			transform.position.y + coll.offset.y * transform.localScale.y);
		float width = coll.bounds.size.x;
		float height = coll.bounds.size.y;

		//the ground check draws a line right underneath the player
		//if there is a collider on that line, the player is on something
		//and therefore can jump
		//p1 and p2 are the ends of that line
		Vector2 p1 = new Vector2 (pos.x - width / 2f + 0.01f, pos.y - height / 2f - 0.02f);
		Vector2 p2 = new Vector2 (pos.x + width / 2f - 0.01f, pos.y - height / 2f - 0.02f);

		return Physics2D.Linecast (p1, p2);

	}

}
