using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PressKeyToStart : MonoBehaviour {

	public float startDelay = 3f;
	public GameObject instructionText;
	public string firstLevelName = "Level 1";
	private bool canStart;

	// Use this for initialization
	void Start () {
		this.canStart = false;

		if (this.instructionText) {
			setTextOpacity (this.instructionText, 0f);
		}

		Invoke ("enableStart", startDelay);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown && this.canStart) {
			SceneManager.LoadScene (this.firstLevelName);
		
		}

		if (!this.canStart) {
			float perc = Time.timeSinceLevelLoad / this.startDelay;
			this.setTextOpacity (this.instructionText, perc);
		}
	}

	public void setTextOpacity (GameObject obj, float opac) {
		Text textObj = obj.GetComponent<Text> ();
		Color textColor = textObj.color;
		textColor.a = opac;
		textObj.color = textColor;

		//Debug.Log (textColor);
	}

	public void enableStart () {
		this.canStart = true;
		if (this.instructionText) {
			setTextOpacity (this.instructionText, 1f);
		}
	}
}
