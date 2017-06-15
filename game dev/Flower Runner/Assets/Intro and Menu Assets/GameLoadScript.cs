using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadScript : MonoBehaviour {

	public float startDelay = 3f;

	// Use this for initialization
	void Start () {
		Invoke ("loadMenuScene", this.startDelay);
	}

	public void loadMenuScene () {
		SceneManager.LoadScene ("MainMenu");
	}
}
