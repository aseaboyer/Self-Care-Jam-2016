using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootController : MonoBehaviour {

	public List<GameObject> currentlyTouching = new List<GameObject> ();

	void OnCollisionEnter (Collision other) {
		Debug.Log ("Foot hit a " + other.gameObject.tag);
		if (other.gameObject.tag == "Environment") {
			this.currentlyTouching.Add (other.gameObject);
		}
	}
	void OnCollisionExit (Collision other) {
		if (other.gameObject.tag == "Environment") {
			this.currentlyTouching.Remove (other.gameObject);
		}
	}

	public bool isGrounded () {
		if (this.currentlyTouching.Count > 0) {
			return true;
		}

		return false;
	}
}
