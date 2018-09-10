using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour {

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] clips;

	private void Step()  {
		if (GamePreferences.GetFXState () == 1) {
			AudioClip clip = GetRandomClip ();
			audioSource.PlayOneShot (clip, 0.01f);
		}
	}

	private AudioClip GetRandomClip()  {
		return clips [Random.Range (0, clips.Length)];
	}
}
