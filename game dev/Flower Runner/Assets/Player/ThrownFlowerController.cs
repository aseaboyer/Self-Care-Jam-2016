using UnityEngine;
using System.Collections;

public class ThrownFlowerController : MonoBehaviour {

	public float maxLifeSpan = 3f;
	private float removeAfterTime = 0f;

	// Use this for initialization
	void Start () {
		this.removeAfterTime = Time.time + this.maxLifeSpan;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= this.removeAfterTime) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Basic Enemy") {
			Debug.Log ("A thrown flower hit an enemy!");
		}
	}
}
