using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public KeyCode left;
	public KeyCode right;
	public KeyCode jump;
	public int Player;

	private Rigidbody2D rb;

	public float runSpeed;
	public float jumpForce;
	private float velo;
	private InputManager inputManager;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		velo = 0f;

	}

	void OnEnable () {
		inputManager = InputManager.instance;
		inputManager.Map("Jump"  + Player.ToString(),jump);
		inputManager.Map("left"  + Player.ToString(),left);
		inputManager.Map("right" + Player.ToString(),right);
	}

	void LetCSharpCollectItsOwnGarbage () {
		inputManager.Remove("Jump"  + Player.ToString(),jump);
		inputManager.Remove("left"  + Player.ToString(),left);
		inputManager.Remove("right" + Player.ToString(),right);
	}

	// Update is called once per frame
	void Update () {
		OnPlatform();
		//horizontal movement:
		velo = 0f;
		
		foreach(KeyCode key in inputManager.Get_Buttons("left" + Player.ToString())) {
			if (Input.GetKey (key))
				velo -= runSpeed;
		}

		foreach(KeyCode key in inputManager.Get_Buttons("right" + Player.ToString())) {
			if (Input.GetKey (key))
				velo += runSpeed;
		}

		//if player is on a platform, move with it
		//this.GetComponent<Transform>().Translate(GetPlatformVelo ()*Time.deltaTime);

		rb.velocity = new Vector2 (velo, rb.velocity.y);

		//jumping, it checks each key that could cause the player to jump:
		foreach (KeyCode key in inputManager.Get_Buttons("Jump" + Player.ToString())) {
			if (Input.GetKeyDown (key) && OnGround())
				rb.AddForce (Vector2.up * jumpForce);
		}
			
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
		
		return (Physics2D.Linecast (p1, p2) && !Physics2D.Linecast (p1, p2).collider.name.Contains("Exit"));

	}

	void OnPlatform () {
		//find width and height of character
		BoxCollider2D coll = GetComponent<BoxCollider2D> ();
		Vector2 pos = new Vector2(transform.position.x + coll.offset.x * transform.localScale.x, 
			transform.position.y + coll.offset.y * transform.localScale.y);
		float width = coll.bounds.size.x;
		float height = coll.bounds.size.y;

		Vector2 p1 = new Vector2 (pos.x - width / 2f + 0.01f, pos.y - height / 2f - 0.02f);
		Vector2 p2 = new Vector2 (pos.x + width / 2f - 0.01f, pos.y - height / 2f - 0.02f);

		if (Physics2D.Linecast (p1, p2) && (Physics2D.Linecast (p1, p2).collider.name.Contains("Platform")
			|| Physics2D.Linecast (p1, p2).collider.name.Contains("Button"))) {
			transform.parent = Physics2D.Linecast (p1, p2).collider.gameObject.transform;
		}
	}
	void OnCollisionExit2D(Collision2D coll) {
		print(transform.parent);
		if (coll.collider.name.Contains("Platform") || coll.collider.name.Contains("Button")) {
			if (transform.parent != null && transform.parent == coll.collider.transform) {
				transform.parent = null;
			}
		}
	}

}
