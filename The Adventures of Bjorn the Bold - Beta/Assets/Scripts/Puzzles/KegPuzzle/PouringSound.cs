using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringSound : MonoBehaviour {

	[SerializeField] private AudioSource AudioSource;
	[SerializeField] private AudioClip sandPouring;

	private void PourPowderIntoKeg()  {
		if (GamePreferences.GetFXState() == 1)  {
			AudioClip clip = sandPouring;
			AudioSource.PlayOneShot (clip, 0.5f);
		}
	}
}
