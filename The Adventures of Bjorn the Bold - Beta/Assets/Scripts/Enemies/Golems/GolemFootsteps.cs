using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemFootsteps : MonoBehaviour {

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip stepSound;

	private void GolemStep()  {
		if (GamePreferences.GetFXState () == 1) {
			audioSource.PlayOneShot (stepSound, 1f);
		}
	}
}
