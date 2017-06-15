using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	public float movementSpeed = 1f;
	public Vector2 jumpBaseForce;
	public Vector2 doubleJumpBaseForce;
	public bool facingRight;

	public Animator playerAnimator;
	private Rigidbody2D rbody;
	private GameController gc;

	public int maxHealth = 3;
	private int currentHealth = 3;
	public Vector2 injurePlayerForce;
	public float injuredFrequency = 0.5f;
	private bool canBeInjured;

	public bool canThrow = false;
	public InventoryController inventory;

	public bool canJump = true;
	public bool canDoubleJump = false;
	public float jumpDelay = 0.15f;
	public float airborneOffset = 1f;
	public List<GameObject> feet;
	public bool onGround;

	// Use this for initialization
	void Start () {
		/*
		 * store commonly accessed scripts for better performance later
		 */

		// find this objects rigidbody
		this.rbody = transform.GetComponent <Rigidbody2D> ();

		// Find the game controller
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj) {
			this.gc = gcObj.GetComponent <GameController> ();
		}

		this.currentHealth = this.maxHealth;
		this.gc.updateHealth (this.maxHealth);
		this.canBeInjured = true;

		//this.canJump = true;
		//this.canThrow = true;

		this.facingRight = true;

		this.playerAnimator = GetComponent<Animator> ();

		this.inventory = GetComponent<InventoryController> ();
	}
	
	// Update is called once per frame
	void Update () {
		bool isWalking = false;
		bool isGrounded = this.isGrounded ();

		this.groundCheck ();

		if (Input.GetButton ("Horizontal")) {
			float movingSpeed = Input.GetAxisRaw ("Horizontal") * movementSpeed;
			this.walk (movingSpeed);
			isWalking = true;
		}

		if (Input.GetButtonDown ("Jump")) {
			if (this.canJump) {
				this.triggerJump ();
			}
		}

		if (!isGrounded) {
			this.playerAnimator.SetBool ("Airborne", true);
		} else {
			this.playerAnimator.SetBool ("Airborne", false);

			if (isWalking) {
				this.playerAnimator.SetBool ("Walking", true);
			} else {
				this.playerAnimator.SetBool ("Walking", false);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Death Zone") {
			gc.restartScene ("Watch your step and try to land on those platforms!");

		} else if (other.gameObject.tag == "Basic Enemy") {
			//this.injurePlayer ();

			bool otherStunned = other.gameObject.GetComponent <EnemyHealthController> ().isStunned;
			if (otherStunned == false) {
				this.injurePlayer ();
			}

		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Ending Flag") {
			gc.completeScene ();
		}
	}

	public void groundCheck () {
		bool onGround = false;
		foreach (GameObject foot in this.feet) {
			RaycastHit2D hitFoot = Physics2D.Raycast (foot.transform.position, Vector2.down,
				1000, LayerMask.GetMask ("Environment"));
			if (hitFoot.distance < this.airborneOffset) {
				onGround = true;
			}
		}
		this.onGround = onGround;
	}

	bool isGrounded () {
		/*RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down,
			1000, LayerMask.GetMask ("Environment"));
		return (hit.distance < this.airborneOffset);*/
		return this.onGround;
	}

	private void injurePlayer () {
		if (this.canBeInjured) {
			Vector2 force = Vector2.up * this.injurePlayerForce.y;
			this.rbody.AddForce (force, ForceMode2D.Impulse);

			this.currentHealth--;
			gc.updateHealth (this.currentHealth);
			if (this.currentHealth <= 0) {
				gc.restartScene ("Be careful around the locals, they can be nasty!");
			} else {
				this.canBeInjured = false;
				Invoke ("makePlayerMortal", injuredFrequency);
			}
		}
	}

	private void makePlayerMortal () {
		this.canBeInjured = true;
	}

	private void applyJump () {
		Vector2 force = Vector2.zero;
		if (this.onGround) {
			force.x = (Input.GetAxisRaw ("Horizontal") * this.jumpBaseForce.x);
			force.y = this.jumpBaseForce.y;
		} else {
			force.x = (Input.GetAxisRaw ("Horizontal") * this.doubleJumpBaseForce.x);
			force.y = this.doubleJumpBaseForce.y;
		}

		Vector2 tempVelocity = this.rbody.velocity;
		tempVelocity.y = 0;
		this.rbody.velocity = tempVelocity;
		this.rbody.AddForce (force, ForceMode2D.Impulse);

		this.canJump = false;
		Invoke ("enableJump", this.jumpDelay);
	}

	void triggerJump () {
		// Make sure they can jump! Use a raycast (or two)
		/*RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down,
			                  1000, LayerMask.GetMask ("Environment"));
		//Debug.Log ("Ground Dist: " + hit.distance);
		if (hit.distance < 0.8) {*/
		if (this.isGrounded ()) {
			//Debug.Log ("On ground");
			this.applyJump ();
		} else {
			//Debug.Log ("In air. Inventory: " + this.inventory.Count);

			if (this.canDoubleJump) {
				if (this.inventory.canBonusJump ()) {
					//Debug.Log ("Can double Jump!");
					this.applyJump ();

					this.inventory.removeFlower ();

					if (this.gc) {
						gc.spawnFlowerParticles (this.transform.position);
					}
				}
			}
		}
	}

	void walk (float speed) {
		Vector2 force = Vector2.right * (speed * Time.deltaTime);
		this.rbody.AddForce (force);
		this.characterFace (force.x);
	}

	void characterFace (float dir) {
		// don't assume DIR is 1 or -1
		if (dir > 0) {
			transform.localRotation = Quaternion.Euler (0, 0, 0);
			this.facingRight = true;
		} else {
			transform.localRotation = Quaternion.Euler (0, 180, 0);
			this.facingRight = false;
		}
	}

	public Vector2 facingDirection () {
		if (this.facingRight) {
			return Vector2.right;
		}
		return Vector2.left;
	}

	public bool facesRight () {
		if (this.facingRight) {
			return true;
		}
		return false;
	}

	public void suspendThrow (float t) {
		this.canThrow = false;
		Invoke ("reenableThrow", t);
	}

	public void reenableThrow () {
		this.canThrow = true;
	}
	public void enableJump () {
		this.canJump = true;
	}
	public void enableSkill (string type) {
		if (type == "throw") {
			this.canThrow = true;
		} else if (type == "jump") {
			this.canJump = true;
		}
	}
	public void disableSkill (string type) {
		if (type == "throw") {
			this.canThrow = false;
		} else if (type == "jump") {
			this.canJump = false;
		}
	}
}
