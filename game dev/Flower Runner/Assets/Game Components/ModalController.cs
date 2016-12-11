using UnityEngine;
using System.Collections;

public class ModalController : MonoBehaviour {

	private GameController gc;
	public string text;

	void Start () {
		GameObject gcgo = GameObject.FindGameObjectWithTag ("GameController");
		this.gc = gcgo.GetComponent<GameController> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			gc.displayModal (text);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Player") {
			gc.clearModal (text);
		}
	}
}
