using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class startMenuController : MonoBehaviour {

	public List<GameObject> buttons = new List<GameObject> ();
	public int activeButton = 0;
	public GameObject cursorSprite;
	public Vector2 cursorPosition;

	// Use this for initialization
	void Start () {
		cursorSprite = GameObject.FindGameObjectWithTag ("Cursor");

		foreach (GameObject btn in buttons) {
			if (btn.GetComponent <Button> ()) {
				Debug.Log ("Has button.");
			} else {
				Debug.Log ("No button.");
			}
		}

		this.setActiveButton (this.activeButton);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp ("Vertical")) {
			float keyDir = Input.GetAxis ("Vertical");
			Debug.Log ("Change " + keyDir);
			if (keyDir > 0) {
				this.changeActiveButton (-1);
			} else {
				this.changeActiveButton (1);
			}
		}
	}

	public void moveCursor () {
		if (this.cursorSprite) {
			this.cursorSprite.GetComponent<RectTransform> ().position = this.cursorPosition;
		}
	}

	public void setCursorLocation (Button btn) {
		RectTransform rect = btn.GetComponent<RectTransform> ();
		Vector2 pos = rect.position;
			pos.x -= (rect.sizeDelta.x * 0.45f);
			pos.y += (rect.sizeDelta.y * 0.15f);
		this.cursorPosition = pos;

		this.moveCursor ();
	}

	public void changeActiveButton (int changeVal) {
		int newVal = this.activeButton + changeVal;
		this.setActiveButton (newVal);
	}

	public void setActiveButton (int newVal) {
		newVal = Mathf.Clamp (newVal, 0, this.buttons.Count - 1);

		// Deselect old buttons
		/*foreach (GameObject btn in buttons) {
			btn.GetComponent<Image> ().sprite = inactiveButtonSprite;
		}*/

		// Select button
		this.activeButton = newVal;
		Button activeBtn = this.buttons [this.activeButton].GetComponent<Button> ();
		if (activeBtn) {
			activeBtn.Select ();
			this.setCursorLocation (activeBtn);
		}
		//Canvas.ForceUpdateCanvases ();
	}

	public void levelLoad (string levelName) {
		if (SceneManager.GetSceneByName (levelName) != null) {
			SceneManager.LoadScene (levelName);
		} else {
			Debug.Log ("Scene doesn't exist, named: " + levelName);
		}
	}
}
