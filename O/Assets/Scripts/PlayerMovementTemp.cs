using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTemp : MonoBehaviour {

	public KeyCode left;
	public KeyCode right;
	public KeyCode jump;
	public string horizontal;
	public int Player;
	public int maxRunSpeed;
	public int terminalVelocity;
	public float runAcc;
	public float quadraticDragCoefficient;

	private Rigidbody2D rb;

	public float runSpeed;
	public float jumpForce;
	private float velo;
	private InputManager inputManager;
	private bool canJump;
	private float timeSinceLastGround;
	private float jumpGrace;

	private Vector2 velocity;
	private Vector2 acceleration;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		velo = 0f;
		jumpGrace = .2f;
	}

	void OnEnable () {
		inputManager = InputManager.instance;
		inputManager.Map("Jump"  + Player.ToString(),jump);
		inputManager.Map("left"  + Player.ToString(),left);
		inputManager.Map("right" + Player.ToString(),right);
		inputManager.Map("horizontal" + Player.ToString(),horizontal);
	}

	void LetCSharpCollectItsOwnGarbage () {
		inputManager.Remove("Jump"  + Player.ToString(),jump);
		inputManager.Remove("left"  + Player.ToString(),left);
		inputManager.Remove("right" + Player.ToString(),right);
		inputManager.Remove("horizontal" + Player.ToString(),horizontal);
	}

	// Update is called once per frame
	void Update () {
		OnPlatform();
		//horizontal movement:
		velo = 0f;


		acceleration = new Vector2(Mathf.Sign(velocity.x) * quadraticDragCoefficient * velocity.x * velocity.x,
						   Mathf.Sign(velocity.y) * quadraticDragCoefficient * velocity.y * velocity.y);
		//acceleration = new Vector2(0,0);
		foreach(KeyCode key in inputManager.Get_Buttons("left" + Player.ToString())) {
			if (Input.GetKey (key)) {
				velo -= runSpeed;
				if(velocity.x > 0)
					acceleration.x += -10 * runAcc;
				else
					acceleration.x -= runAcc;
				}
		}

		foreach(KeyCode key in inputManager.Get_Buttons("right" + Player.ToString())) {
			if (Input.GetKey (key)) {
				velo += runSpeed;
				if(velocity.x < 0)
					acceleration.x += 10 * runAcc;
				else
					acceleration.x += runAcc;
				}
		}


		

		timeSinceLastGround += Time.deltaTime;
		if (GetComponent<CharacterController>().isGrounded) {
			timeSinceLastGround = 0;
			print(":");}
		else
			acceleration.y -= 2f;
		canJump = timeSinceLastGround < jumpGrace;

		foreach (KeyCode key in inputManager.Get_Buttons("Jump" + Player.ToString())) {
			if (Input.GetKeyDown (key) && canJump) {
				//rb.AddForce (Vector2.up * jumpForce);
				velocity.y = jumpForce;
				timeSinceLastGround += jumpGrace;
			}
		}

		velocity = new Vector2(Mathf.Clamp(velocity.x + acceleration.x,-1 * maxRunSpeed,maxRunSpeed),Mathf.Clamp(velocity.y + acceleration.y,-1 * terminalVelocity,terminalVelocity));

		//transform.position = new Vector3(transform.position.x + (velocity.x * Time.deltaTime) + (acceleration.x * (Time.deltaTime * Time.deltaTime * .5f))
		//						,        transform.position.y + (velocity.y * Time.deltaTime) + (acceleration.y * (Time.deltaTime * Time.deltaTime * .5f))
		//						,0f);
		GetComponent<CharacterController>().Move (new Vector3((velocity.x * Time.deltaTime) + (acceleration.x * (Time.deltaTime * Time.deltaTime * .5f))
								,         (velocity.y * Time.deltaTime) + (acceleration.y * (Time.deltaTime * Time.deltaTime * .5f))
								,0f));
		//rb.velocity = new Vector2 (velo, rb.velocity.y);

			
	}

	void OnPlatform () {
		//find width and height of character
		CharacterController coll = GetComponent<CharacterController> ();
		Vector2 pos = new Vector2(transform.position.x + coll.center.x * transform.localScale.x, 
			transform.position.y + (coll.center.y - (coll.height / 2)) * transform.localScale.y);
		float width = coll.bounds.size.x;
		float height = coll.bounds.size.y;

		Vector2 p1 = new Vector2 (pos.x - coll.radius * transform.localScale.x, pos.y);
		Vector2 p2 = new Vector2 (pos.x + coll.radius * transform.localScale.x, pos.y);




		if (Physics2D.Linecast (p1, p2) && (Physics2D.Linecast (p1, p2).collider.name.Contains("Platform")
			|| Physics2D.Linecast (p1, p2).collider.name.Contains("Button"))) {
			transform.parent = Physics2D.Linecast (p1, p2).collider.gameObject.transform;
		} 
	}
	void OnCollisionExit2D(Collision2D coll) {
		if (coll.collider.name.Contains("Platform") || coll.collider.name.Contains("Button")) {
			if (transform.parent != null && transform.parent == coll.collider.transform) {
				transform.parent = null;
			}
		}
	}
	void OnCollisionEnter2D(Collision2D coll) { //wont get used
		if (coll.collider.name.Contains("Button")) {
			transform.parent = coll.collider.transform;
		}
	}

}
