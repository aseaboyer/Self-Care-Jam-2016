using UnityEngine;
using System.Collections;

public class EnemyHealthController : MonoBehaviour {

	private Animator ani;

	public int startingHealth = 1;
	private int health = 1;
	public bool isStunned = false;
	public float stunDuration = 1f;
	
	public bool isAlive = true;
	public bool respawnsAtDeath = false;
	public float deathDelay = 1f;

	// Use this for initialization
	void Start () {
		this.health = this.startingHealth;

		this.ani = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (this.isAlive) {
			if (other.tag == "Thrown Flower") {
				this.takeDamage ();
			}
		}
	}

	public void respawn () {
		this.changeAnimation ("Stunned", false);
	}

	public void stun () {
		this.changeAnimation ("Stunned", true);
	}

	public void die () {
		this.isAlive = false;
		this.changeAnimation ("Dead", true);
		Debug.Log ("Need to be able to yield a death.");
	}

	public void takeDamage () {
		this.takeDamage (1);
	}

	public void takeDamage (int d) {
		this.health--;

		if (this.health > 0) {
			this.stun ();
		} else {
			this.die ();
		}
	}

	public void changeAnimation (string n, bool v) {
		if (this.ani) {
			this.ani.SetBool (n, v);
		}
	}
	public void changeAnimation (string n, float v) {
		if (this.ani) {
			this.ani.SetFloat (n, v);
		}
	}
}
