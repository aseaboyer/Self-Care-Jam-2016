using UnityEngine;
using System.Collections;

public class AutoKillScript : MonoBehaviour {

	public float lifetime = 1f;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, lifetime);
	}
}
