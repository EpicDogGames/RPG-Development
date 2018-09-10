using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveTheBell : MonoBehaviour {

	[SerializeField] private GameObject bellTolling;

	void OnTriggerEnter2D(Collider2D other)  {
		if (other.name == "Bjorn2D")  {
			CaveMazePuzzleController.haveBell = true;
			bellTolling.SetActive (false);
			Destroy (gameObject);
		}
	}
}
