using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianSearchPuzzleStarts : MonoBehaviour {

	[SerializeField] private GameObject guardianSearchPuzzleUI;
	[SerializeField] private GameObject openPuzzleEffect;
	[SerializeField] private Transform particleEffectLocation;

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player") {
			Instantiate (openPuzzleEffect, particleEffectLocation.position, particleEffectLocation.rotation);
			StartCoroutine(DisplayPuzzle ());
		}
	}

	void OnTriggerExit(Collider col)  {
		if (col.tag == "Player")  {
			Time.timeScale = 1f;
			guardianSearchPuzzleUI.SetActive (false);
		}
	}

	IEnumerator DisplayPuzzle()  {
		yield return new WaitForSeconds (1f);
		guardianSearchPuzzleUI.SetActive (true);
	}
}
