using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMazeTrigger : MonoBehaviour {

	[SerializeField] private GameObject caveMazeSearchPuzzleUI;

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			caveMazeSearchPuzzleUI.SetActive (true);
		}
	}

	void OnTriggerExit(Collider col)  {
		if (col.tag == "Player")  {
			Time.timeScale = 1f;
			caveMazeSearchPuzzleUI.SetActive (false);
		}
	}
}
