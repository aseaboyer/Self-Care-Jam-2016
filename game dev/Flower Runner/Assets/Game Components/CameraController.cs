using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject target; // typically, the player
	public float panSpeed = 1f;
	public float zOffset;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// move the camera into position
		if (this.target) {
			Vector3 targetPosition = this.target.transform.position;
			targetPosition.z = zOffset;
			float dist = Vector3.Distance (transform.position, targetPosition);
			transform.position = Vector3.MoveTowards (transform.position, targetPosition,
				(panSpeed * dist * Time.deltaTime));
		}
	}
}
