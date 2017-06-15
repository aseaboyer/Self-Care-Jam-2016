using UnityEngine;
using System.Collections;

public class FlowerController : MonoBehaviour {

	public Flower flower;
	public float rebloomDelay;
	private float lastPicked;
	public GameObject head;
	public bool bloomed = false;

	// Use this for initialization
	void Start () {
		SpriteRenderer headSprite = this.head.GetComponent <SpriteRenderer> ();
		if (headSprite) {
			headSprite.sprite = this.flower.sprite;
			headSprite.color = this.flower.color;
		}

		this.bloom ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D (Collider2D other) {
		if (this.bloomed) {
			if (other.tag == "Player") {
				bool playerPicksFlower = other.GetComponent <InventoryController> ().offerFlower (this.flower);
				if (playerPicksFlower) {
					this.pick ();
				}
			} else if (other.tag == "Basic Enemy") {
				this.pick ();
			}
		}
	}

	public void pick () {
		if (this.bloomed) {
			this.bloomed = false;
			this.head.GetComponent<SpriteRenderer> ().enabled = false;

			GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
			if (gcObj) {
				gcObj.GetComponent <GameController> ().spawnFlowerParticles (this.head.transform.position);
			}

			Invoke ("bloom", this.rebloomDelay);
		}
	}

	public void bloom () {
		this.bloomed = true;
		this.head.GetComponent<SpriteRenderer> ().enabled = true;
	}

}
