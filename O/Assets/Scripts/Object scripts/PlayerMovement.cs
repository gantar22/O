using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour {

	enum h_state {attack,decay,release,sustain,idle,walk};
	enum v_state {up,down,peak,idle};
	public string horizontal;
	public string jump;
	public int Player;

	private Rigidbody2D rb;


	public float drag;
	public float runSpeed;
	public float maxSpeed;
	public float sustainSpeed;
	public float jumpForce;
	private float targetVelo;
	private InputManager inputManager;
	private bool canJump;
	private float timeSinceLastGround;
	private float jumpGrace;

	public float accelTime;
	public float decayTime;
	private float runUp;
	private float lastIdleTime;
	private h_state h_s;
	private v_state v_s;
	private bool jumpHeld;
	private float lastWinkTime;
	public GameObject body;
	[HideInInspector]
	public bool dead;
	[HideInInspector]
	public GameObject GameController;
	public AudioClip death;
	private bool indoor;
	private Transform door;
	private float doorbounds;


	// Use this for initialization


	/*
       We need to make it so that if you jump right before you land
       you will jump anyway when you do land.
       So, the jump key activates a process that last for a few fractions
       of a second, and you jump if you are grounded and in that mode.
	*/ 
	void Start () {
		runUp = maxSpeed / Mathf.Sqrt(accelTime);
		targetVelo = 0f;
		jumpGrace = .1f;
		lastIdleTime = Time.time;
		h_s = h_state.idle;
		v_s = v_state.idle;
		lastWinkTime = Time.time;
		GameController = GameObject.FindGameObjectWithTag ("GameController");
	}

	void Awake()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void OnEnable () {
		rb = GetComponent<Rigidbody2D> ();
		inputManager = InputManager.instance;
		inputManager.Map("Jump"  + Player.ToString(),jump);
		inputManager.Map("horizontal" + Player.ToString(),horizontal);

		if (Player == 1) {
			inputManager.Map ("Jump1", "Jump_K2");
			inputManager.Map ("horizontal1", "Horizontal_K1");
		} else {
			inputManager.Map ("Jump2", "Jump_K1");
			inputManager.Map ("horizontal2", "Horizontal_K2");
		}
		EventManager.StartListening ("hitdoor", hitdoor);
	}

	void hitdoor(){
		if(indoor) return;
		indoor = true;
		door = GameObject.FindGameObjectWithTag("door").transform.parent;
		doorbounds = door.gameObject.GetComponent<CapsuleCollider2D>().size.x * .5f * door.localScale.x;
		if(Mathf.Abs(doorbounds - 1f) > .1f)
		{
			print("You changed the scale of the door or its collider");
		}
		doorbounds += Mathf.Abs(transform.localScale.x) * .5f * gameObject.GetComponent<CapsuleCollider2D>().size.x;
		if(Mathf.Abs(doorbounds - 1.4f) > .1f)
		{
			print("You changed the scale of the player or its collider");
		}
		print(doorbounds);
	}

	void LetCSharpCollectItsOwnGarbage () {
		inputManager.Remove("Jump"  + Player.ToString(),jump);
		inputManager.Remove("horizontal" + Player.ToString(),horizontal);
	}


	void Attack()
	{
		rb.velocity = new Vector2(runUp * Mathf.Sqrt(Time.time - lastIdleTime),rb.velocity.y);
		if(Time.time - lastIdleTime > accelTime)
			h_s = h_state.decay;
		if(targetVelo < .85)
			h_s = h_state.release;
	}
	void Idle()
	{
		lastIdleTime = Time.time;
		if(targetVelo > .85)
			h_s = h_state.attack;
	}
	void release()
	{
		if(targetVelo > .85)
		{
			lastIdleTime = Time.time - Mathf.Pow(rb.velocity.x / runUp,2);
			h_s = h_state.attack;
		}
	}
	void sustain()
	{
		rb.velocity = new Vector2(sustainSpeed,rb.velocity.y);
	}

	// Update is called once per frame
	void Update () {
		if (PauseManager.paused) {
			return;
		}
		if (indoor){
			transform.position = new Vector3 
			(Mathf.Clamp(transform.position.x,
						door.position.x - doorbounds,
						door.position.x + doorbounds),
				transform.position.y,transform.position.z);
				}

		this.GetComponent<Animator>().SetFloat("speed",Mathf.Abs(rb.velocity.x));
		if(Time.time - lastWinkTime > 5)
		{
			lastWinkTime = Time.time;
			if(Random.Range(0.0f,2.0f) > 1.0f) 
			{
				this.GetComponent<Animator>().SetTrigger("blink");
			}
		}

		this.GetComponent<Animator>().SetBool("left",rb.velocity.x < 0);
		transform.localScale = new Vector3(rb.velocity.x < 0 ? -.8f : .8f,.75f,1f);

		//horizontal movement:
		targetVelo = 0f;
		foreach(string axis in inputManager.Get_Axis("horizontal" + Player.ToString())) {
			targetVelo += Input.GetAxis(axis) * runSpeed;
		}

		//rb.AddForce(new Vector2(targetVelo,0),ForceMode2D.Force);
		rb.velocity = new Vector2 (Mathf.Lerp(rb.velocity.x,targetVelo, 17.5f * Time.deltaTime),rb.velocity.y);
		rb.velocity = new Vector2 (rb.velocity.x,Mathf.Clamp(rb.velocity.y, -1 * maxSpeed,9999999));



		timeSinceLastGround += Time.deltaTime;
		if (OnGround()) 
		{
			timeSinceLastGround = 0;
			v_s = v_state.idle;
		}
		canJump = timeSinceLastGround < jumpGrace;

		jumpHeld = false;
		//jumping, it checks each key that could cause the player to jump:
		foreach (string key in inputManager.Get_Axis("Jump" + Player.ToString())) {
			if (Input.GetButtonDown (key) && canJump) {
				rb.velocity = new Vector2(rb.velocity.x,jumpForce);
				timeSinceLastGround += jumpGrace;
				v_s = v_state.up;
			}
			if(Input.GetAxis (key) > 0)
				jumpHeld = true;
		}
		if(v_s == v_state.up )
		{
			if(rb.velocity.y < 0 || !jumpHeld)
			{
				rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y*.5f); //stops you from going up
				v_s = v_state.down;
				rb.AddForce(new Vector2(0,-7),ForceMode2D.Impulse);
			}
		}

			
	}

	bool OnGround () {
		if(v_s == v_state.down)
			v_s = v_state.idle;

		//find width and height of character
		CapsuleCollider2D coll = GetComponent<CapsuleCollider2D> ();
		Vector2 pos = new Vector2(transform.position.x + coll.offset.x * transform.localScale.x, 
			transform.position.y + coll.offset.y * transform.localScale.y);
		float width = coll.bounds.size.x - 0.3f;
		float height = coll.bounds.size.y;

		//the ground check draws a line right underneath the player
		//if there is a collider on that line, the player is on something
		//and therefore can jump
		//p1 and p2 are the ends of that line
		Vector2 p1 = new Vector2 (pos.x - width / 2f + 0.01f, pos.y - height / 2f - 0.02f);
		Vector2 p2 = new Vector2 (pos.x + width / 2f - 0.01f, pos.y - height / 2f - 0.02f);

		if (Physics2D.Linecast (p1, p2) && (Physics2D.Linecast (p1, p2).collider.name.Contains ("Platform"))) {
			transform.parent = Physics2D.Linecast (p1, p2).collider.gameObject.transform.parent;
		} else if(Physics2D.Linecast (p1, p2) && Physics2D.Linecast (p1, p2).collider.name.Contains ("Button")){
			transform.parent = Physics2D.Linecast (p1, p2).collider.gameObject.transform;
		} else if(Physics2D.Linecast (p1, p2) && Physics2D.Linecast (p1, p2).collider.name.Contains ("pressable")){
			
		} else if (transform.parent != null) {
			transform.parent = null;
		}
		if (Physics2D.Linecast (p1, p2) && Physics2D.Linecast (p1, p2).collider.name.Contains ("head"))
		{
			rb.velocity = Physics2D.Linecast (p1, p2).collider.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
			rb.velocity = new Vector3(targetVelo + rb.velocity.x,rb.velocity.y,0);
		}

		if (Physics2D.Linecast (p1, p2)) {
			string collider = Physics2D.Linecast (p1, p2).collider.name;
			if (!(collider.Contains ("Exit") || collider.Contains ("Checkpoint") || collider.Contains("Spikes"))) {
				return true;
			}
		}
		return false;
	}

	public bool OnAir(){
		//find width and height of character
		CapsuleCollider2D coll = GetComponent<CapsuleCollider2D> ();
		Vector2 pos = new Vector2(transform.position.x + coll.offset.x * transform.localScale.x, 
			transform.position.y + coll.offset.y * transform.localScale.y);
		float width = coll.bounds.size.x;
		float height = coll.bounds.size.y;

		//the ground check draws a line right underneath the player
		//if there is a collider on that line, the player is on something
		//and therefore can jump
		//p1 and p2 are the ends of that line
		Vector2 p1 = new Vector2 (pos.x, pos.y - height / 2f - 0.02f);
		Vector2 p2 = new Vector2 (pos.x, pos.y - height / 2f - 4f);

		Debug.DrawLine(p1,p2);
		if (Physics2D.Linecast (p1, p2)) {
			string collider = Physics2D.Linecast (p1, p2).collider.name;
			if (!(collider.Contains("Spikes"))) {
				return false;
			}
		}
		return true;

	}

	public float moving()
	{
		if(rb == null) return 1;
		return Mathf.Abs(rb.velocity.x);
	}

	public void die(){
		if(dead) return;
		dead = true;

		jumpGrace = 0f;

		GameController.GetComponent<AudioSource> ().PlayOneShot (death, SettingsManager.gameSettings.masterVolume);
		GameObject mybody = Instantiate(body,transform.position,Quaternion.identity);
		foreach(Transform child in mybody.transform)
		{
			child.parent = null;
			Rigidbody2D rbc = child.gameObject.GetComponent<Rigidbody2D>();
			rbc.interpolation = RigidbodyInterpolation2D.None;
			rbc.AddForce(new Vector2(Random.Range(-100,100),Random.Range(500,1200)),ForceMode2D.Impulse);
			Destroy(child.gameObject,.5f);
		}
		foreach(Transform child in transform){
			child.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			if(child.childCount > 0)
			{
				foreach(Transform child2 in child){
					child2.gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
		}
	}

}
