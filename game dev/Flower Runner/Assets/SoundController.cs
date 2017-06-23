using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	public List<LabelledSound> sounds = new List<LabelledSound> ();
	private AudioSource source = new AudioSource ();

	void Start () {
		// Create an audio source on start
		this.attachAudioSource ();
	}

	public void playSound (string name) {
		bool foundSound = false;
		foreach (LabelledSound sound in sounds) {
			if (sound.label == name) {
				foundSound = true;
				Debug.Log ("Exit loop, found: " + sound.label);
				// play sound.sound
				this.source.PlayOneShot (sound.sound);

			}
		}

		if (!foundSound) {
			Debug.Log ("Sound not found");
		}
	}

	public void attachAudioSource () {
		AudioSource thisSource = gameObject.GetComponent<AudioSource> ();
		if (!thisSource) {
			thisSource = gameObject.AddComponent<AudioSource> ();
		}

		this.source = thisSource;
	}
}
