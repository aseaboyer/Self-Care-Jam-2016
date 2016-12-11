using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	
	public List<GameObject> healthDisplays;
	public Sprite fullHeart;
	public Sprite emptyHeart;
	public float restartDelay = 3f;
	public GameObject restartWindow;
	public GameObject restartWindowText;
	public GameObject helpModal;
	public GameObject helpModalText;

	public GameObject flowerParticalEffect;

	// Use this for initialization
	void Start () {
		this.restartWindow.SetActive (false);
		this.clearModal ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void spawnFlowerParticles (Vector2 location) {
		if (flowerParticalEffect) {
			GameObject newExplosion = GameObject.Instantiate (this.flowerParticalEffect);
			newExplosion.transform.position = location;
		}
	}

	public void displayModal (string txt) {
		Text textObj = helpModalText.GetComponent <Text> ();
		if (textObj) {
			textObj.text = txt;
			helpModal.SetActive (true);
		}
	}

	public void clearModal (string txt) {
		// only clear the text if it matches the sender
		Text textObj = helpModalText.GetComponent <Text> ();
		if (textObj) {
			if (textObj.text == txt) {
				textObj.text = "";
				helpModal.SetActive (false);
			}
		}
	}

	public void clearModal () {
		// only clear the text if it matches the sender
		Text textObj = helpModalText.GetComponent <Text> ();
		if (textObj) {
			if (textObj.text == "") {
				textObj.text = "";
				helpModal.SetActive (false);
			}
		}
	}

	public void emptyHealthDisplay () {
		foreach (GameObject heart in this.healthDisplays) {
			Image heartImage = heart.GetComponent <Image> ();
			heartImage.sprite = this.emptyHeart;
		}
	}

	public void updateHealth (int currentHealth) {
		this.emptyHealthDisplay ();
		for (int i = 0; i < currentHealth; i++) {
			Image heartImage = this.healthDisplays [i].GetComponent <Image> ();
			heartImage.sprite = this.fullHeart;
		}
		Canvas.ForceUpdateCanvases ();
	}

	public void restartScene (string reason) {
		Text textObj = this.restartWindowText.GetComponent<Text> ();
		if (textObj) {
			textObj.text = reason;
			this.restartWindow.SetActive (true);
		}

		Invoke ("loadSceneThis", this.restartDelay);
	}

	public void loadSceneThis () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
