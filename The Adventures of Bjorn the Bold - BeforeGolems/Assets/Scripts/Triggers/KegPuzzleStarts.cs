using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegPuzzleStarts : MonoBehaviour {

	[SerializeField] private GameObject kegPuzzleUI;
	[SerializeField] private GameObject openPuzzleEffect;
	[SerializeField] private Transform particleEffectLocation;

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			Instantiate (openPuzzleEffect, particleEffectLocation.position, particleEffectLocation.rotation);
			StartCoroutine (DisplayPuzzle ());
		}
	}

	void OnTriggerExit(Collider col)  {
		if (col.tag == "Player")  {
			Time.timeScale = 1f;
			kegPuzzleUI.SetActive (false);
		}
	}

	IEnumerator DisplayPuzzle() {
		yield return new WaitForSeconds (1f);
		kegPuzzleUI.SetActive (true);
	}
}
