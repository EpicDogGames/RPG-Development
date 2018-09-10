using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTheCave : MonoBehaviour {

	// dynamic instruction panels
	[SerializeField] GameObject losePanel;

	void Start()  {
		losePanel.gameObject.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D other)  {
		if (other.name == "Bjorn2D")  {
			if (CaveMazePuzzleController.haveBell) {
				CaveMazePuzzleController.exitCave = true;
				Destroy (gameObject);
			}
			else  {
				CaveMazePuzzleController.exitCave = false;
				losePanel.gameObject.SetActive (true);
				StartCoroutine (NoBellNoGo ());
			}
		}
	}

	IEnumerator NoBellNoGo()  {
		yield return new WaitForSeconds (3f);
		losePanel.gameObject.SetActive (false);
	}
}
