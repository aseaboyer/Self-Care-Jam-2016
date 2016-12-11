﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {

	private PlayerController pc;
	public Flower flowerEmptyAppearance;
	public List<GameObject> inventoryDisplays;

	private List<Flower> inventory = new List<Flower> ();
	public GameObject visualInventoryPrefab;
	public List<GameObject> visualInventory;
	public int inventoryLimit = 3;

	public Vector2 visualInventoryTargetOffset;
	public float visualInventorySpeed = 1f;

	public Vector2 thrownForce;
	public GameObject flowerProjectile;
	public float throwDelay = 0.2f;

	// Use this for initialization
	void Start () {
		this.emptyInventoryDisplay ();

		pc = this.gameObject.GetComponent <PlayerController> ();
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Fire2")) {
			if (this.pc.canThrow) {
				this.throwFlower ();
			}
		}

		if (this.visualInventory.Count > 0) {
			this.updateVisualInventory ();
		}
	}

	public bool canBonusJump () {
		if (this.inventory.Count > 0) {
			return true;
		}

		return false;
	}

	public void emptyInventoryDisplay () {
		foreach (GameObject dis in this.inventoryDisplays) {
			Image sr = dis.GetComponent <Image> ();
			sr.color = flowerEmptyAppearance.color;
			sr.sprite = flowerEmptyAppearance.sprite;
		}
	}

	public void updateInventory (List<Flower> flowers) {
		this.emptyInventoryDisplay ();
		for (int i = 0; i < flowers.Count; i++) {
			Image sr = this.inventoryDisplays [i].GetComponent <Image> ();
			sr.color = flowers [i].color;
			sr.sprite = flowers [i].sprite;
		}
		Canvas.ForceUpdateCanvases ();
	}

	public void throwFlower () {
		if (this.inventory.Count > 0 && this.flowerProjectile) {
			Flower thrownFlower = this.inventory [0];
			GameObject newFlower = GameObject.Instantiate (this.flowerProjectile);
			Vector2 force = this.thrownForce;
			if (!this.pc.facesRight ()) {
				force.x = -this.thrownForce.x;
			}

			newFlower.transform.position = (Vector2) (this.transform.position) + this.pc.facingDirection ();
			Rigidbody2D rb = newFlower.GetComponent<Rigidbody2D> ();
			if (rb) {
				rb.AddForce (force, ForceMode2D.Impulse);
			}
			SpriteRenderer sp = newFlower.GetComponent<SpriteRenderer> ();
			if (sp) {
				sp.sprite = thrownFlower.sprite;
				sp.color = thrownFlower.color;
			}

			this.removeFlower (thrownFlower);

			this.pc.suspendThrow (this.throwDelay);
		}
	}


	public void removeFlower () { // override that assumes last item
		this.removeFlower ( this.inventory [this.inventory.Count - 1]);
	}
	public void removeFlower (Flower f) {
		int fIndex = this.inventory.IndexOf (f);
		//GameObject removedItem = this.visualInventory.RemoveAt (fIndex);
		Destroy (this.visualInventory[fIndex]);

		this.inventory.Remove (f);
		this.updateInventory (this.inventory);
	}

	public bool offerFlower (Flower flowerType) {
		if (inventory.Count < inventoryLimit) {
			this.inventory.Add (flowerType);
			this.updateInventory (this.inventory);

			// create a new visual inventory item
			GameObject newVisualItem = GameObject.Instantiate (this.visualInventoryPrefab);
			newVisualItem.transform.position = this.transform.position;
			this.visualInventory.Add (newVisualItem);

			return true;
		}

		return false;
	}

	void updateVisualInventory () {

		Vector2 previousPos = (Vector2)this.transform.position + 
			this.visualInventoryTargetOffset + -this.pc.facingDirection ();

		for (int i = 0; i < this.visualInventory.Count; i++) {
			Vector2 currentPos = this.visualInventory [i].transform.position;
			float speedMod = (visualInventorySpeed * Time.deltaTime);
			// Needs to have a nice accordian effect in the end
			// multiply by (0.1 * i)
			// or, instead of a fixed var, use above and * player velocity

			//this.visualInventory [i].transform.position = firstPosition;
			this.visualInventory [i].transform.position = Vector2.MoveTowards (
				currentPos, previousPos, speedMod);
			previousPos = currentPos; // point next at this's last pos
		}
	}
}